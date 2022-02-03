using System;
using UnityEngine;


public class AvatarColors

{
    public static Color[] _skinColors = {new (65f/255f, 31f/255f, 29f/255f),
        new (43f/255f, 0f/255f, 1f/255f),
        new (117f/255f, 68f/255f, 61f/255f),
        new (196f/255f, 133f/255f, 99f/255f),
        new (191f/255f, 121f/255f, 90f/255f),
        new (225f/255f, 147f/255f, 125f/255f),

        new (88f/255f, 48f/255f, 57f/255f),
        new (62f/255f, 27f/255f, 18f/255f),
        new (190f/255f, 131f/255f, 105f/255f),
        new (213f/255f, 150f/255f, 121f/255f),
        new (201f/255f, 136f/255f, 103f/255f),
        new (244f/255f, 188f/255f, 170f/255f),

        new (112f/255f, 68f/255f, 69f/255f),
        new (77f/255f, 36f/255f, 11f/255f),
        new (236f/255f, 171f/255f, 138f/255f),
        new (230f/255f, 170f/255f, 147f/255f),
        new (219f/255f, 149f/255f, 118f/255f),
        new (254f/255f, 219f/255f, 199f/255f),

        new (184f/255f, 111f/255f, 111f/255f),
        new (92f/255f, 46f/255f, 23f/255f),
        new (188f/255f, 114f/255f, 107f/255f),
        new (230f/255f, 192f/255f, 173f/255f),
        new (250f/255f, 194f/255f, 163f/255f),
        new (252f/255f, 231f/255f, 220f/255f),

        new (240f/255f, 197f/255f, 184f/255f),
        new (104f/255f, 48f/255f, 33f/255f),
        new (222f/255f, 149f/255f, 133f/255f),
        new (240f/255f, 198f/255f, 189f/255f),
        new (248f/255f, 203f/255f, 171f/255f),
        new (252f/255f, 222f/255f, 207f/255f)};

    public static Color[] _hairColors =
    {
        new(250f/255f,228f/255f,169f/255f),
        new(220f/255f,192f/255f,157f/255f),
        new(213f/255f,190f/255f,163f/255f),
        new(130f/255f,101f/255f,80f/255f),
        new(132f/255f,100f/255f,77f/255f),
        new(88f/255f,67f/255f,59f/255f),
        new(68f/255f,48f/255f,42f/255f),
        new(48f/255f,28f/255f,22f/255f),
        new(151f/255f,90f/255f,63f/255f),
        new(103f/255f,52f/255f,53f/255f),
        new(69f/255f,41f/255f,40f/255f),
        new(66f/255f,34f/255f,42f/255f),

        new(40f/255f,43f/255f,49f/255f),
        new(20f/255f,20f/255f,16f/255f),
        new(177f/255f,173f/255f,169f/255f),
        new(206f/255f,12f/255f,133f/255f),
        new(40f/255f,71f/255f,231f/255f),
        new(90f/255f,104f/255f,37/255f)
    };

    public static Color[] _eyeColors = { 
        new (160f/255f,180f/255f,199f/255f),
        new(92f/255f,123f/255f,162f/255f),
        new(77f/255f,107f/255f,136f/255f),
        new(63f/255f,89f/255f,93f/255f),
        new(128f/255f,138f/255f,114f/255f),
        new(112f/255f,117f/255f,81f/255f),
        new(168f/255f,132f/255f,89f/255f),
        new(130f/255f,109f/255f,81f/255f),
        new(67f/255f,52f/255f,52f/255f),
        new(53f/255f,36f/255f,34f/255f),
        new(145f/255f,151f/255f,146f/255f),
        new(96f/255f,101f/255f,110f/255f)};
    
    /// <summary>
    ///  Iterates through the Colors in @colorsToChooseFrom and calculates the Distance to the @favoriteColor.
    /// The Color which distance is closest to the one attributed to the preferenceLevel is returned. 
    /// </summary>
    /// <param name="preferenceLevel">Level in how far the returned Color should match the favorite one</param>
    /// <param name="favoriteColor">Color which was chosen in the questionnaire</param>
    /// <param name="colorsToChooseFrom">List of Color to return the best matching one</param>
    /// <returns>Color which matches the favoriteColor to the Degree of preferenceLevel</returns>
    public static Color GetColorOnPreference(int preferenceLevel, Color favoriteColor, Color[] colorsToChooseFrom)
    {
        if (preferenceLevel == 4) return favoriteColor;
        
        Color closestColorToPreference = default;
        float lastClosestDistance = 30;
        float wantedDistance = WantedDistance(preferenceLevel);
        
        foreach (var color in colorsToChooseFrom)
        {
            float colorDistance = distance(RgBtoCielab.ColorDistance(favoriteColor, color), wantedDistance);
            if (colorDistance < lastClosestDistance)
            {
                closestColorToPreference = color;
                lastClosestDistance = colorDistance;
            }
        }   
        
        return closestColorToPreference;
    }
    

    /// <param name="preferenceLevel"> preference Level you want the attributed Cielab distance from</param>
    /// <returns>Cielab distances based on preference level</returns>
    private static float WantedDistance(int preferenceLevel)
    {
        return preferenceLevel switch
        {
            0 => 30,
            1 => 22.5f,
            2 => 15,
            3 => 7.5f,
            _ => throw new ArgumentOutOfRangeException(nameof(preferenceLevel), preferenceLevel, null)
        };
    }

    
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns>distance between two floats</returns>
    private static float distance(float x, float y)
    {
        return Mathf.Abs(x-y);
    }
}
