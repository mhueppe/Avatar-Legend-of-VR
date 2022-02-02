using UnityEngine;


/// <summary>
/// Helper class to get perceived distance between two colors.
/// Wikipedia: https://de.wikipedia.org/wiki/Lab-Farbraum#Umrechnung_von_RGB_zu_Lab
/// </summary>
public static class RgBtoCielab

{   
    /// 1. Make RGB values to XYZ values
    private static Vector3 RgbtoXYZ(Color c)
    {
        var x = 0.4124564f * c.r + 0.3575761f * c.g + 0.1804375f * c.b;
        var y = 0.2126729f * c.r + 0.7151522f * c.g + 0.0721750f * c.b;
        var z = 0.0193339f * c.r + 0.1191920f * c.g + 0.9503041f * c.b;
        
        return new Vector3(x, y, z);
    }

   
    /// Helper method to adapt XYZtoLab formula respectively
    private static float Division(float division)
    {
        var epsilon = 0.008856;
        var k = 903.3f;

        if (division > epsilon)
            return Mathf.Pow(division, 1/3f);
        
        return (k * division + 16f) / 116f;
    }
    
    /// 2. Make XYZ values to Cielab values
    private static Vector3 XYZtoLab(Vector3 xyz)
    {
        // get vector items
        var x = xyz.x;
        var y = xyz.y;
        var z = xyz.z;

        // strange values for formula
        var y_n = 95.047f;
        var x_n = 100;
        var z_n = 108.883f;

        var fx = Division(x / x_n);
        var fy = Division(y / y_n);
        var fz = Division(z / z_n);
        
        // Final L* a* b* values
        var l = (116 * fy) - 16;
        var a = 500 * (fx - fy);
        var b = 200 * (fy - fz);

        return new Vector3(l, a, b);
    }
    /// Bundles whole conversion
    private static Vector3 RgbtoLab(Color c)
    {
        
        return XYZtoLab(RgbtoXYZ(c));
    }


    /// <summary>
    /// Absoult geile function.
    /// </summary>
    /// <param name="c1">Absolut geile Farbe Nummer 1</param>
    /// <param name="c2">Absolut noch geilere Farbe Nummer 2</param>
    /// <returns>Den heftig geilsten Abstand den die Welt jemals auf der ganzen großen weiten Welt gesehen hat.</returns>
    public static float ColorDistance(Color c1, Color c2)
    {
        // RGB to Lab
        var c1Lab = RgbtoLab(c1);
        var c2Lab = RgbtoLab(c2);

        // Calculate Distance
        var lDif = Mathf.Pow((c2Lab.x - c1Lab.x), 2);
        var aDif = Mathf.Pow((c2Lab.y - c1Lab.y), 2);
        var bDif = Mathf.Pow((c2Lab.z - c1Lab.z), 2);

        var distance = Mathf.Sqrt(lDif + aDif + bDif);
        
        return distance;
    }
    
}