using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(GameObjectRecordable))]
public class GameObjectRecordablePropertyDrawer : PropertyDrawer
{
    /// <summary>
    /// The number of exposed variables/ lines in the Editor. Important to calculate the height of the UI Rect of this component.
    /// </summary>
    private const int NUMBER_OF_SERIALIZED_PROPERTIES = 3;
    /// <summary>
    /// The width in pixels around each property.
    /// </summary>
    private const int BORDER_WIDTH = 2;
    
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return base.GetPropertyHeight(property, label) * NUMBER_OF_SERIALIZED_PROPERTIES + 2 * BORDER_WIDTH * NUMBER_OF_SERIALIZED_PROPERTIES;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var propHeight = (position.height - BORDER_WIDTH * 2 * NUMBER_OF_SERIALIZED_PROPERTIES) / NUMBER_OF_SERIALIZED_PROPERTIES;
        EditorGUI.BeginProperty(position, label, property);
        
            #region Expose ObjToRecord

            // position has height of the whole area but we will use this variable for positioning all other properties.
            position.height = propHeight;
            position.y += BORDER_WIDTH;
            var objToRecProperty = property.FindPropertyRelative("objectToRecord");
            EditorGUI.PropertyField(position, objToRecProperty, new GUIContent("Object to record"));

            #endregion

            #region Expose Popup to select the variable to record

            // increment rect one property down.
            position.y += propHeight + 2 * BORDER_WIDTH;
            var objToRec = objToRecProperty.objectReferenceValue as GameObject;
            var selectedIndex = property.FindPropertyRelative("selectedIdx");
            if (objToRec == null) selectedIndex.intValue = 0; // reset the index to 0 if the gameObject is deselected/ changed.
            
            var metaInfos = ListMembersMetaInfos(objToRec);
            
            selectedIndex.intValue = EditorGUI.Popup(
                position,
                "Variable to record",
                selectedIndex.intValue,
                metaInfos.PopUpInfos.ToArray());

            
            #endregion
            
            #region Transfere MetaInfo to RecorderObj
            // Pass all details needed for value retrieval via reflection.
            property.FindPropertyRelative("componentName").stringValue =
                metaInfos.ComponentTypeNames[selectedIndex.intValue];
            property.FindPropertyRelative("propertyName").stringValue =
                metaInfos.PropertyNames[selectedIndex.intValue];
            property.FindPropertyRelative("returnTypeName").stringValue =
                metaInfos.ReturnTypes[selectedIndex.intValue];

            #endregion
            
            #region Expose headerName
            
            // increment position rect down once more.
            position.y += propHeight + 2 * BORDER_WIDTH;
            EditorGUI.PropertyField(position, property.FindPropertyRelative("headerName"),
                new GUIContent("Display Name:"));
            
            #endregion

        EditorGUI.EndProperty();
    }

    
    /// <summary>
    /// Collects meta information for for the members of all components of <see cref="GameObject"/>. 
    /// </summary>
    /// <param name="gameObject">The gameObject to scan.</param>
    /// <returns></returns>
    private MetaInfos ListMembersMetaInfos(GameObject gameObject)
    {
        var metaInfoLists = new MetaInfos();
        if (gameObject == null) return metaInfoLists;
        
        var attachedComponents = gameObject.GetComponents<Component>();
        
        // iterate through each component.
        foreach (var component in attachedComponents)
        {
            Type type = component.GetType();
            var propertyInfos = type.GetProperties(RecorderDefinitions.ReflectionBindingFlags);
            // iterate through each member of this component (Methods, Properties, Fields...)
            foreach (var info in propertyInfos)
            {
                // if not defined or marked as obsolete/ deprecated skip the property.
                if (!RecorderDefinitions.IsDefinedType(info.PropertyType) 
                    || info.CustomAttributes.Any(attr => attr.AttributeType == typeof(ObsoleteAttribute)))
                    continue;
                metaInfoLists.AddEntry(type, info.PropertyType, info.Name);
            }
        }

        return metaInfoLists;
    }

    /// <summary>
    /// Stores meta information for the popup to select members from components of some gameObject and
    /// stores more useful information in other lists so the popup string does not have to be parsed.
    /// </summary>
    private class MetaInfos
    {
        public readonly List<string> PopUpInfos = new List<string>(), 
            ComponentTypeNames = new List<string>(), 
            PropertyNames = new List<string>(),
            ReturnTypes = new List<string>();

        public MetaInfos()
        {
            // Add default dummy/ empty selection
            PopUpInfos.Add("No function");
            ComponentTypeNames.Add("");
            PropertyNames.Add("");
            ReturnTypes.Add("");
        }
        
        /// <summary>
        /// Collects all important meta information in separate lists.
        /// </summary>
        /// <param name="componentType">type of the component holding the member</param>
        /// <param name="propertyType">type of the member</param>
        /// <param name="memberName">name of the member</param>
        public void AddEntry(Type componentType, Type propertyType, string memberName)
        {
            PopUpInfos.Add($"{componentType.Name}/{propertyType.Name} {memberName}");
            ComponentTypeNames.Add(componentType.Name);
            PropertyNames.Add(memberName);
            ReturnTypes.Add(propertyType.Name);
        }
    }
}
