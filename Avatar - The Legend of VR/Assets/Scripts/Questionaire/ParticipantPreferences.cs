using UnityEngine;

public struct ParticipantPreferences
{
    public int Age { get; set; }

    public Color SkinColor { get; set; }

    public Color HairColor { get; set; }
    public HairLength HairLength { get; set; }

    public Color EyeColor { get; set; }
    public ClothingStyle ClothingStyle { get; set; }

    public Color FavouriteColor { get; set; }

    public Opinion LikesGlasses { get; set; }
    
    public Opinion LikesHats { get; set; }


}


public enum HairLength
{
    Short,
    Middle,
    Long
}


public enum ClothingStyle
{
    Casual,
    Chique,
    Sporty
}

public enum Opinion
{
    Yes,
    No,
    Neutral
}
