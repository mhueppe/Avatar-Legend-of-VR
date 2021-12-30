using System;
using UnityEngine;

/// <summary>
/// Wrapper implementation of <see cref="IRecordable"/> to make it easy to use for simple recordings. 
/// </summary>
public class DelegateRecordable : IRecordable
{
    // Variables to store all delegates.
    private readonly Func<string[]> _recordingFunc;
    private readonly Func<string[]> _getHeaderFunc;
    private readonly Func<Exception, string> _onRecordingExceptionCaughtAction;
    private readonly Action<Exception, int> _onGetHeaderExceptionCaughtAction;

    /// <summary>
    /// Create delegates returning values you wish to record with a <see cref="Recorder"/>.
    /// </summary>
    /// <param name="record">Delegate function which returns a string array with values to record.</param>
    /// <param name="getHeader">Delegate function which returns as many headers as values are returned from the <paramref name="record"/> delegate.></param>
    /// <param name="numberOfColumnsNeeded">How many elements <paramref name="record"/> and <paramref name="getHeader"/> are returning.</param>
    /// <param name="onRecordingExceptionCaught">Specify a handle if an exception occurs during <paramref name="record"/>.</param>
    /// <param name="onGetHeaderExceptionCaught">Specify a handle if an exception occurs in <paramref name="getHeader"/></param>
    public DelegateRecordable(
        Func<string[]> record,
        Func<string[]> getHeader,
        int numberOfColumnsNeeded,
        Func<Exception, string> onRecordingExceptionCaught,
        Action<Exception, int> onGetHeaderExceptionCaught
    )
    {
        _recordingFunc = record;
        _getHeaderFunc = getHeader;
        NumberOfColumnsNeeded = numberOfColumnsNeeded;
        _onRecordingExceptionCaughtAction = onRecordingExceptionCaught;
        _onGetHeaderExceptionCaughtAction = onGetHeaderExceptionCaught;
    }

    /// <summary>
    /// Create delegates returning values you wish to record with a <see cref="Recorder"/>.
    /// </summary>
    /// <param name="record">Delegate function which returns a string array with values to record.</param>
    /// <param name="getHeader">Delegate function which returns as many headers as values are returned form the <paramref name="record"/> delegate.</param>
    /// <param name="numberOfColumnsNeeded">How many elements <paramref name="record"/> and <paramref name="getHeader"/> are returning.</param>
    public DelegateRecordable(Func<string[]> record, Func<string[]> getHeader, int numberOfColumnsNeeded) :
        this(record, getHeader, numberOfColumnsNeeded, 
            onRecordingExceptionCaught: e => "Exception occured", 
            onGetHeaderExceptionCaught: (e, i) => Debug.LogAssertion($"Caught exception while generating a header for an object of type {nameof(DelegateRecordable)} with header-id: {i}")) { }

    /// <summary>
    /// Create delegates returning values you wish to record with a <see cref="Recorder"/>.
    /// </summary>
    /// <param name="record">Delegate function which returns a string array with values to record.</param>
    /// <param name="header">An array of headers for the values returned by <paramref name="record"/>.</param>
    /// <param name="numberOfColumnsNeeded">How many elements <paramref name="record"/> and <paramref name="header"/> are returning.</param>
    public DelegateRecordable(Func<string[]> record, string[] header, int numberOfColumnsNeeded) :
        this(record, () => header, numberOfColumnsNeeded) { }
    
    #region Interface Implementation
    
    /// <inheritdoc/>
    public int NumberOfColumnsNeeded { get; }
    
    /// <inheritdoc/>
    public string[] Record() => _recordingFunc?.Invoke();

    /// <inheritdoc/>
    public string[] GetHeader() => _getHeaderFunc?.Invoke();

    /// <inheritdoc/>
    public string OnRecordingExceptionCaught(Exception e) => _onRecordingExceptionCaughtAction?.Invoke(e);

    /// <inheritdoc/>
    public void OnGetHeaderExceptionCaught(Exception e, int failedHeaderNo) =>
        _onGetHeaderExceptionCaughtAction?.Invoke(e, failedHeaderNo);
    
    #endregion
}
