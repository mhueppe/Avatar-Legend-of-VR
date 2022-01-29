using UnityEngine;

public struct ParticipantPreferences
{
    public int Age { get; set; }
    // public Sex Sex { get; set; }

    public Color SkinColor { get; set; }

    public Color HairColor { get; set; }
    public HairLength HairLength { get; set; }
    // public HairType HairType { get; set; }
    //
    // public BodyType BodyType { get; set; }

    public Color EyeColor { get; set; }
    public ClothingStyle ClothingStyle { get; set; }

    public Color FavouriteColor { get; set; }

    public Opinion LikesGlasses { get; set; }
    // public Opinion LikesBraces { get; set; }
    // public Opinion LikesPiercings { get; set; }
    // public Opinion LikesTattoos { get; set; }
    // public Opinion LikesJewelry { get; set; }

}

// public enum Sex
// {
//     Male,
//     Female,
//     Divers
// }

public enum HairLength
{
    Short,
    Middle,
    Long
}

// public enum BodyType
// {
//     Skinny,
//     Petite,
//     Muscular,
//     Adipose
// }

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
