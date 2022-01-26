using System.Linq;

public class RecorderUtils
{
    /// <summary>
    /// Takes the baseString and appends each suffix to it once, separated with an underscore. Length of the returned string array is equal to the number of supplied suffixes.
    /// E.g. AppendMany("base", "x", "y") -> {"base_x", "base_y"}
    /// </summary>
    /// <param name="baseString"></param>
    /// <param name="suffixes"></param>
    /// <returns></returns>
    public static string[] AppendMany(string baseString, params string[] suffixes) => suffixes.Select(suffix => $"{baseString}_{suffix}").ToArray();

    /// <summary>
    /// Helper method to convert any number of parameters of any type to strings.
    /// (without having to call .ToString() on every element.
    /// </summary>
    /// <param name="objs">objects to convert to strings</param>
    /// <returns></returns>
    public static string[] DumpToString(params object[] objs) => objs.Select(obj => obj.ToString()).ToArray();


    /// <summary>
    /// Fills an array with the value in every position.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="array">Array to fill.</param>
    /// <returns></returns>
    public static T[] FillArray<T>(T value, T[] array)
    {
        for (var i = 0; i < array.Length; i++)
            array[i] = value;
        return array;
    } 
}
