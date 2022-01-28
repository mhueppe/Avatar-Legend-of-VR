using System;
using System.Reflection;
using UnityEngine;

/// <summary>
/// Retrieves a property value of any component of <see cref="objectToRecord"/> using <see cref="System.Reflection"/>.
/// </summary>
[Serializable]
public class GameObjectRecordable : IRecordable
{
    #region Serialized Members
    // !!! If renamed, change RecordedObjPropertyDrawer.OnGUI(), FindPropertyRelative("newname") as well. !!!
    // all of them are private and serialized so the editor can access these properties. (although not all of them will be visible in the Editor)
#if UNITY_EDITOR
#pragma warning disable 0414
    /// <summary>
    /// Important for the RecordedObjPropertyDrawer to save its state.
    /// </summary>
    [SerializeField] private int selectedIdx = 0;
#pragma warning restore 0414
#endif
    
    /// <summary>
    /// The GameObject which holds a reference to the member of interest. 
    /// </summary>
    [SerializeField] private GameObject objectToRecord;

    /// <summary>
    /// Component name of <see cref="objectToRecord"/> that holds the member <see cref="propertyName"/>.
    /// </summary>
    [SerializeField] private string componentName;
    
    /// <summary>
    /// Name of member to be recorded.
    /// </summary>
    [SerializeField] private string propertyName;

    /// <summary>
    /// The returned type of the member.
    /// </summary>
    [SerializeField] private string returnTypeName;

    /// <summary>
    /// The name on the csv header for this object.
    /// </summary>
    [SerializeField] private string headerName;
    
    #endregion

    #region Private/ Cached Members

    /// <summary>
    /// Reference to the component holding the member of interest.
    /// </summary>
    private dynamic _component;
    
    /// <summary>
    /// The property information about <see cref="propertyName"/>.
    /// </summary>
    private PropertyInfo _propertyInfo = null;

    /// <summary>
    /// A getter delegate for the property of interest.
    /// </summary>
    private dynamic _propertyGetter;
    
    #endregion
    
    #region Getter

    public GameObject ObjectToRecord => objectToRecord;
    public string PropertyName => propertyName;

    #endregion

    #region Interface Implementation

    /// <summary>
    /// Retrieves selected data from <see cref="objectToRecord"/> and decomposes it if needed.
    /// E.g. a Vector3 will be returned as {Vector3.x, Vector3.y, Vector3.z}
    /// </summary>
    /// <returns></returns>
    /// <exception cref="Exception">This object is insufficiently instantiated or an <see cref="ArgumentNullException"/> if recorded value was null.</exception>
    public string[] Record()
    {
        #region Setup Validity Checks
        
        if (string.IsNullOrEmpty(componentName) || string.IsNullOrEmpty(propertyName))
            throw new Exception($"Not all fields are sufficiently instantiated in order to record game object: '{objectToRecord}'");

        if (!objectToRecord.activeSelf)
        {
            return RecorderUtils.FillArray($"{objectToRecord.name} is inactive", new string [NumberOfColumnsNeeded]);
        }
        
        #endregion
        
        #region First Initialization (and OnChange processing during Editor)
        
        if (_propertyInfo == null 
            #if UNITY_EDITOR
            || !propertyName.Equals(_propertyInfo.Name) // the recording objective should not change in playmode.
            #endif
            )
        {
            _component = objectToRecord.GetComponent(componentName);
            var componentType = _component.GetType();
            _propertyInfo = componentType.GetProperty(propertyName);
            
            if (_propertyInfo == null)
                throw new ArgumentException("Property not found!");
            
            // Create delegate to avoid multiple security checks to regain speed. 
            _propertyGetter = Delegate.CreateDelegate(
                typeof(Func<,>).MakeGenericType(componentType, _propertyInfo.PropertyType),
                null,
                _propertyInfo.GetGetMethod(true)
            );
        }

        #endregion
        
        var value = _propertyGetter(_component);
        return RecorderDefinitions.GenerateOutput(value);

    }
    
    /// <summary>
    /// The number of values which will be returned from <see cref="GetHeader"/> and <see cref="Record"/>.
    /// </summary>
    public int NumberOfColumnsNeeded => RecorderDefinitions.NumberOfColumnsNeeded(returnTypeName);
    
    /// <summary>
    /// Generates column headers from <see cref="headerName"/> for the selected data from <see cref="objectToRecord"/> given the <see cref="returnTypeName"/>.
    /// E.g. for <c>Vector3</c> three headers will be created for each vector component.
    /// </summary>
    /// <returns></returns>
    public string[] GetHeader() => RecorderDefinitions.GenerateHeader(headerName, returnTypeName);

    /// <inheritdoc/> 
    public string OnRecordingExceptionCaught(Exception e)
    {
        var objectName = "MissingReference";
        try
        {
            objectName = objectToRecord != null ? objectToRecord.name : null; 
        // will be thrown if the gameObject was destroyed. (I don't know how to check for that without this exception being thrown.
        } catch (MissingReferenceException){} 
                
        Debug.LogAssertion($"Header name: {headerName}, member: '{propertyName}' in component: '{componentName}' from game object: '{objectName}' threw an exception: {e.Message}");

        return e is ArgumentNullException ? "null" : "exception occured";
    }
    
    /// <inheritdoc/>
    public void OnGetHeaderExceptionCaught(Exception e, int failedHeaderNo)
    {
        Debug.LogAssertion($"While reading {objectToRecord}'s header definition an exception occured: {e.Message} Fail id in header name will be {failedHeaderNo}.");
    }
    
    #endregion
}