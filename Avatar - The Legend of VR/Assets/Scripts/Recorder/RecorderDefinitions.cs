using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

/// <summary>
/// Holds basic functionalities to make recording gameObjects easier.
/// E.g. it can decompose data types into their single components as long as the type was defined once.
/// 
/// <example><code>IsDefinedType(typeof(Vector3)) -> true</code></example>
/// <example><code>NumberOfColumnsNeeded(nameof(Vector3)) -> 3</code></example>
/// <example><code>GenerateHeader("MyVector3", nameof(Vector3)) -> new string[] {"MyVector3_x", "MyVector3_y", "MyVector3_z"}</code></example>
/// <example><code>GenerateOutput(Vector3) -> new string[] {Vector3.x, Vector3.y, Vector3.z}</code></example>
/// 
/// If more types are needed, make sure to add it to the <see cref="HashSet{T}"/> <see cref="DefinedTypes"/>
/// and add a case in the switch statements in <see cref="NumberOfColumnsNeeded"/>, <see cref="GenerateOutput"/> and <see cref="GenerateHeader"/>.
/// Afterwards it should be visible and supported by the <see cref="GameObjectRecordable"/> and <see cref="Recorder"/>.
/// </summary>
public static class RecorderDefinitions
{
    /// <summary>
    /// Define what kind of members should be found in type.GetMembers calls.
    /// </summary>
    public const BindingFlags ReflectionBindingFlags =
        BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

    /// <summary>
    /// The set of types which will be exposed in the Editor (and recordable). Primitives are always possible (int, string, ...)
    /// If you want to define another type, add it here and in the <see cref="GameObjectRecordable"/> for correct recording.
    /// </summary>
    private static readonly HashSet<Type> DefinedTypes = new HashSet<Type>(
        new[]{
            typeof(string), typeof(Vector2), typeof(Vector3), typeof(Vector4), typeof(Quaternion), typeof(Color), typeof(Matrix4x4)
        });
    
    /// <summary>
    /// Checks if this type is recordable.
    /// </summary>
    /// <param name="type"></param>
    /// <returns>true if it is defined else false</returns>
    public static bool IsDefinedType(Type type)
    {
        return type.IsPrimitive || DefinedTypes.Contains(type);
    }
    
    /// <summary>
    /// Returns the number of columns which need to be occupied to save this type.
    /// This number will the length of the returned array from <see cref="GenerateOutput"/> and <see cref="GenerateHeader"/>.
    /// </summary>
    /// <param name="typeName"></param>
    /// <returns></returns>
    public static int NumberOfColumnsNeeded(string typeName)
    {
        var type = Type.GetType("System." + typeName);
        if (type != null && 
            (type.IsPrimitive || type == typeof(string))) return 1;
        switch (typeName)
        {
            case nameof(Vector3):
                return 3;
            case nameof(Quaternion):
            case nameof(Color):
                return 4;
            case nameof(Vector2):
                return 2;
            case nameof(Matrix4x4):
                return 16;
            default:
                Debug.LogWarning($"Not a defined recording type '{typeName}', please define the number of returned values in RecorderDefinitions.NumberOfColumnsNeeded");
                return 1;
        }
    }
    
    /// <summary>
    /// Formats a value depending on its type.
    /// </summary>
    /// <param name="value">the value to be formatted</param>
    /// <returns>formatted and possibly decomposed value</returns>
    public static string[] GenerateOutput(object value)
    {
        if (value == null)
            throw new ArgumentNullException(nameof(value));
        // if value is one of the primitive types we just want to convert it to string directly.
        var type = value.GetType();
        if (type.IsPrimitive || type == typeof(string)) 
            return RecorderUtils.DumpToString(value);
        var x = value switch
        {
            Vector3 v3 => RecorderUtils.DumpToString(v3.x, v3.y, v3.z),
            Quaternion q => RecorderUtils.DumpToString(q.x, q.y, q.z, q.w),
            Vector2 v2 => RecorderUtils.DumpToString(v2.x, v2.y),
            Vector4 v4 => RecorderUtils.DumpToString(v4.x, v4.y, v4.z, v4.w),
            Color c => RecorderUtils.DumpToString(c.r, c.g, c.b, c.a),
            Matrix4x4 m => RecorderUtils.DumpToString(m.m00, m.m01, m.m02, m.m03,
                                                               m.m10, m.m11, m.m12, m.m13, 
                                                               m.m20, m.m21, m.m22, m.m23, 
                                                               m.m30, m.m31, m.m32, m.m33),
            _ => RecorderUtils.DumpToString(value)
        };
        return x;
    }

    /// <summary>
    /// Generates multiple column names based on the <see cref="typeName"/> separated with an underscore.
    /// F.e. for a Vector2 it will return: {"typeName_x", "typeName_y"} 
    /// </summary>
    /// <param name="headerNamePrefix"></param>
    /// <param name="typeName">The name of the type, not the full name. (int32 instead of System.Int32)</param>
    /// <returns></returns>
    public static string[] GenerateHeader(string headerNamePrefix, string typeName)
    {
        // check if the type is primitive, if so we just convert them to string.
        Type returnType = Type.GetType("System." + typeName);
        if (returnType != null && 
            (returnType.IsPrimitive || returnType == typeof(string))) 
            return RecorderUtils.DumpToString(headerNamePrefix);

        switch (typeName)
        {
            case nameof(Vector3):
                return RecorderUtils.AppendMany(headerNamePrefix, "x", "y", "z");
            case nameof(Vector2):
                return RecorderUtils.AppendMany(headerNamePrefix, "x", "y");
            case nameof(Quaternion): // same as Vector4
            case nameof(Vector4):
                return RecorderUtils.AppendMany(headerNamePrefix, "x", "y", "z", "w");
            case nameof(Color):
                return RecorderUtils.AppendMany(headerNamePrefix, "r", "g", "b", "a");
            case nameof(Matrix4x4):
                return RecorderUtils.AppendMany(headerNamePrefix, "m00", "m01", "m02", "m03",
                    "m10", "m11", "m12", "m13",
                    "m20", "m21", "m22", "m23",
                    "m30", "m31", "m32", "m33");
            default:
                Debug.LogWarning($"Fell back to the default header dump with type: '{typeName}'");
                return RecorderUtils.DumpToString(headerNamePrefix);
        }
    }
}
