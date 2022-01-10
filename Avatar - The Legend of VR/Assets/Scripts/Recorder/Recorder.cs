using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;
#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Records a list of <see cref="IRecordable"/>s. Populate this list via the Editor window or by registering them with <see cref="RegisterToRecord"/>.
/// </summary>
public class Recorder : MonoBehaviour
{
    
    [Tooltip("The path relative from Assets/StreamingAssets/ where the recorded data will be saved to. Note: this value might be overridden by some script.")]
    [SerializeField] private string savePath;
    
    [Tooltip("The approximate time interval between each recording frame.")]
    [SerializeField] private float recordEvery = .2f;
    
    /// <summary>
    /// Exposed list of <see cref="GameObjectRecordable"/> for Editor usage.
    /// </summary>
    [SerializeField] private List<GameObjectRecordable> objectsToRecord;
    
    /// <summary>
    /// The list of <see cref="IRecordable"/>s which will be used during the recording.
    /// </summary>
    private List<IRecordable> _recordables = new List<IRecordable>();
    
    /// <summary>
    /// Indicating whether the recording has started or not.
    /// </summary>
    private bool _hasRecordingStarted = false;
    
    /// <summary>
    /// The time the recording started.
    /// </summary>
    private float _recordingStartTime;

    /// <summary>
    /// The writer to save the recording.
    /// </summary>
    private StreamWriter _writer;
    
    

    /// <summary>
    /// Register a new <see cref="GameObjectRecordable"/> to the list of all recorded objects.
    /// </summary>
    /// <param name="recordable">Object to record.</param>
    /// <exception cref="Exception">If recording has already been started.</exception>
    public void RegisterToRecord(IRecordable recordable)
    {
        if (_hasRecordingStarted)
            throw new Exception("Registering is only possible before the recording has started or the header was retrieved.");
        _recordables.Add(recordable);
    }

    /// <summary>
    /// Ensembles all headers into one enumerable from all <see cref="IRecordable"/>.
    /// </summary>
    /// <returns></returns>
    private IEnumerable<string> CreateHeader()
    {
        var failedHeaderNo = 0;
        return _recordables.SelectMany(recordable =>
        {
            try
            {
                return recordable.GetHeader();
            }
            catch (Exception e)
            { // if header is not retrievable an default header will be used with running index to recover data manually afterwards.
                recordable.OnGetHeaderExceptionCaught(e, failedHeaderNo);

                var placeholderHeaders = new string[recordable!.NumberOfColumnsNeeded];
                for (var i = 0; i < recordable.NumberOfColumnsNeeded; i++)
                    placeholderHeaders[i] = $"HeaderNA_{failedHeaderNo}-{i}";
                
                failedHeaderNo++;
                return placeholderHeaders;
            }
        });
        
    }
    
    /// <summary>
    /// Collects recordings from all <see cref="IRecordable"/> into one enumerable.
    /// </summary>
    /// <returns></returns>
    private IEnumerable<string> GatherData()
    {
        // select many to flatten the nested string arrays.
        return _recordables.SelectMany(recordable =>
        {
            try
            {
                return recordable.Record();
            }
            catch (Exception e)
            {
                var msg = recordable.OnRecordingExceptionCaught(e);
                
                // return as many placeholder entries as the object would have returned without an exception.
                return RecorderUtils.FillArray(msg, new string[recordable!.NumberOfColumnsNeeded]);
            }
        });
    }

    /// <summary>
    /// Coroutine to gather data from all <see cref="IRecordable"/>s and write them into a csv.
    /// </summary>
    private IEnumerator Record()
    {
        while (true)
        {
            var data = GatherData();
            var csData = string.Join(",", data);
            _writer?.WriteLine(csData);
            
            yield return new WaitForSeconds(recordEvery);
        }
    }
    
    /// <summary>
    /// Starts recording all registered <see cref="IRecordable"/>s every <see cref="recordEvery"/>.
    /// After the recording ended all <see cref="IRecordable"/> must be registered again except for the ones from the Editor.
    /// The recording time stamp of each frame will be included by default. 
    /// It will be saved to the path, specified from the Editor.
    /// </summary>
    public void StartRecording() => StartRecording(this.savePath);

    /// <summary>
    /// Starts recording all registered <see cref="IRecordable"/>s every <see cref="recordEvery"/>.
    /// After the recording ended all <see cref="IRecordable"/> must be registered again except for the ones from the Editor.
    /// The recording time stamp of each frame will be included by default. 
    /// </summary>
    /// <param name="savePath">The path + file name relative to 'Assets/StreamingAssets/' where the recorded data will be stored to.</param>
    /// <exception cref="ArgumentException">if the savePath isn't valid.</exception>
    public void StartRecording(string savePath)
    {
        if (string.IsNullOrEmpty(savePath))
        {
            if (string.IsNullOrEmpty(this.savePath))
                throw new ArgumentException("Please specify a valid path to save the experiment recordings.");
            Debug.LogWarning("Specified savePath is null or empty, falling back to the path specified in the Editor.");
            savePath = this.savePath;
        }
        
        var fullPath = Path.Combine(Application.streamingAssetsPath, savePath);

        // Register Unity time since recording start.
        RegisterToRecord(new DelegateRecordable(
            () => new []{(Time.time - _recordingStartTime).ToString(CultureInfo.InvariantCulture)},
            new[]{ "time" }, 
            1));

        // Add all objects specified from the editor.
        _recordables.AddRange(objectsToRecord);
        
        // set flags.
        _hasRecordingStarted = true;
        _recordingStartTime = Time.time;
        
        // create the writer and write the header.
        _writer = new StreamWriter(fullPath);
        var header = CreateHeader();
        var csHeader = string.Join(",", header);
        _writer.WriteLine(csHeader);

        StartCoroutine(Record());
        Debug.Log("Start recording...");
    }

    /// <summary>
    /// Ends the current recording and resets all flags.
    /// </summary>
    public void StopRecordingAndReset()
    {
        // Warn if any component calls this method before the recording has even started. (two scripts interfering)
        if (!_hasRecordingStarted)
        {
            Debug.LogWarning($"{nameof(StopRecordingAndReset)} was called before recording has ever started.");
            return;
        }

        StopCoroutine(Record());
        Debug.Log("Recording ended...");

        // Reset writer
        _writer.Flush();
        _writer.Dispose();
        _writer = null;

        // Reset list of recordables.
        _recordables = new List<IRecordable>();

        _hasRecordingStarted = false;
    }

    private void OnDisable()
    {
        // If holding object gets destroyed or application ended we'll try so save everything waiting in the buffer.
        _writer?.Flush();
        _writer?.Dispose();
    }

    #region Gizmos
    
    private float[] _rndOffset;
    private int _oldLength = 0;
    private void OnDrawGizmosSelected()
    {
        if (_oldLength != objectsToRecord.Count)
        { // Add some noise to the bezier anker points so multiple connections do not overlap.
            _rndOffset = new float[objectsToRecord.Count];
            for (var i = 0; i < _rndOffset.Length; i++)
                _rndOffset[i] = Random.value * .5f;
            _oldLength = objectsToRecord.Count;
        }
        #if UNITY_EDITOR
        var idx = 0;
        foreach (var recordable in objectsToRecord)
        {
            if (recordable is GameObjectRecordable obj)
            {
                // if null there is an empty entry in the list or it was added via script and anonymous funcs.
                if (obj.ObjectToRecord == null) continue;
                
                // calculate the intermediate points for the bezier curve with some additive noise.
                var recPos = transform.position;
                var objPos = obj.ObjectToRecord.transform.position;
                float halfHeight = (recPos.y - objPos.y) * .5f + _rndOffset[idx++];
                var offset = Vector3.up * halfHeight;

                // Use different colors for position and rotation recordings.
                Color color = obj.PropertyName.Contains("osition") ? Color.blue :
                    obj.PropertyName.Contains("otation") ? Color.green : Color.white;

                Handles.DrawBezier(
                    recPos,
                    objPos,
                    recPos - offset,
                    objPos + offset,
                    color,
                    EditorGUIUtility.whiteTexture,
                    2f);
            }
        }
        #endif
    }
    
    #endregion
}