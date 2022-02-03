using System;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class InstantiateAvatar : MonoBehaviour
{   
    
    [SerializeField] private GameObject avatarPrefab;
    [SerializeField] private Material eyeMaterial;
    [SerializeField] private Field avatarField; 
    [SerializeField] private int questionnaireMatch; 
    
    private GameObject _avatar;
    private ParticipantPreferences ParticipantPreferences = new()
    {
        Age = 19, ClothingStyle = ClothingStyle.Casual, EyeColor = Color.black, FavouriteColor = Color.green, LikesHats = Opinion.Yes,
        HairColor = Color.yellow, HairLength = HairLength.Middle, LikesGlasses = Opinion.Yes, SkinColor = Color.black,
    };

    private AvatarColors _avatarColors;
    private const string ShortHair = "hair0";
    private readonly string[] _middleHair = { "hair1", "hair2", "hair3", "hair4"};
    private readonly string[] _longHair= { "hair5", "hair6", "hair7"};

    // Start is called before the first frame update
    private void SetBasedOnPreferenceLevel(bool randomClothingStyle, bool randomClothingColor, bool randomHairLength, bool randomHairColor, bool randomHat, bool randomSkinColor, bool randomEyeColor, bool randomGlasses)
    {
        SetClothing(randomClothingStyle, randomClothingColor);
        SetHair(randomHairLength, randomHairColor, SetHat(randomHat));
        SetSkinColor(randomSkinColor);
        SetEyeColor(randomEyeColor);
        SetGlasses(randomGlasses);
    }
    void Start()
    {   
        // make the avatar object and instantiate it at the position of the field
        _avatar = Instantiate(avatarPrefab, avatarField.position, avatarField.quaternion).GameObject();
        _avatar.GetComponent<global::Avatar>().QuestionnaireMatch = questionnaireMatch;

        switch (questionnaireMatch)
        {
            case 0:
                SetBasedOnPreferenceLevel(true, true, true, true, true, true, true, true);
                break;
                
            case 20:
                SetBasedOnPreferenceLevel(true, true, true, false, false, true, true, Random.value > 0.5);
                break;
            
            case 40:
                SetBasedOnPreferenceLevel(true, false, true, false, Random.value > 0.5, true, true, false);
                break;
            case 60:
                SetBasedOnPreferenceLevel(true, false, true, false, Random.value > 0.5, true, false, false);
                break;
            
            case 80:
                SetBasedOnPreferenceLevel(false, false, false, true, Random.value > 0.5, true, false, Random.value > 0.5);
                break;
                
            case 100:
                SetBasedOnPreferenceLevel(false, false, false, false, false, false, false, false);
                break;
        }

    }

    private void SetEyeColor(bool randomEyeColor)
    {
        // set skin color based on preferences
        Color eyeColor = randomEyeColor ? RandomFromList(AvatarColors._eyeColors) : ParticipantPreferences.EyeColor;
        eyeMaterial.color = eyeColor;
    }
    
    private bool SetHat(bool setBasedOnPreference)
    {
        switch (ParticipantPreferences.LikesHats)
        {
            case Opinion.Yes:
                if (setBasedOnPreference){MakeActive("hat", RandomColor());}

                return true;
            case Opinion.Neutral:
            {
                if (Random.value > 0.5)
                {
                    MakeActive("hat", RandomColor());
                    return true;
                }

                break;
            }
            case Opinion.No:
                if (!setBasedOnPreference)
                {
                    MakeActive("hat", RandomColor());
                    return true;
                }
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        return false;
    }

    private void SetGlasses(bool setBasedOnPreference)
    {
        switch (ParticipantPreferences.LikesGlasses)
            {
                case Opinion.Yes:
                    if (setBasedOnPreference){MakeActive("glasses");}
                    break;
                case Opinion.Neutral:
                {
                    if (Random.value > 0.5)
                    {
                        MakeActive("glasses");
                    }

                    break;
                }
                case Opinion.No:
                    if (setBasedOnPreference){MakeActive("glasses");}
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
    }
    
    private void SetClothing(bool randomClothingStyle, bool randomClothingColor)
    {
        // differentiate the two clothing types
        Enum clothingStyleTop, clothingStyleBottom;
        
        // if random set both of the two types of clothes to random types (random element of the enumeration of types)
        if (randomClothingStyle)
        {
            clothingStyleTop = GetRandomElement<ClothingStyle>();
            clothingStyleBottom = GetRandomElement<ClothingStyle>();
        }
        else
        {
            clothingStyleTop = clothingStyleBottom = ParticipantPreferences.ClothingStyle;

        }
        
        // set color based on preferences
        Color colorTop, colorBottom; 
        if (randomClothingColor)
        {
            colorTop = RandomColor();
            colorBottom = RandomColor();
        }
        else
        {
            colorTop = ParticipantPreferences.FavouriteColor;
            colorBottom = RandomFromList(new []{ParticipantPreferences.FavouriteColor * (float) 0.3, Color.black, ParticipantPreferences.FavouriteColor * (float) 1.1});
        }
        
        // activate top clothing style (based on the preset preferences)
        switch (clothingStyleTop)
        {
            case ClothingStyle.Casual:
                MakeActive("casual_shirt", colorTop);
                break;
            case ClothingStyle.Chique:
                MakeActive("chique_shirt", colorTop);
                break;
            case ClothingStyle.Sporty:
                MakeActive("sporty_shirt", colorTop);
                break;
        }
        
        // activate bottom clothing style (based on the preset preferences)
        switch (clothingStyleBottom)
        {
            case ClothingStyle.Casual:
                MakeActive("pants", colorBottom);
                break;
            case ClothingStyle.Chique:
                MakeActive("pants", colorBottom);
                break;
            case ClothingStyle.Sporty:
                MakeActive("sporty_pants", colorBottom);
                break;
        }
    }
    
    private void SetSkinColor(bool randomSkinColor)
    {
        // set skin color based on preferences
        Color skin = RandomFromList(AvatarColors._skinColors);
        var skinColor = randomSkinColor ? skin : ParticipantPreferences.SkinColor;
        SetColor(skinColor, FindChild("avatar_mesh"));
    }

    private void SetHair(bool randomHairLength, bool randomHairColor, bool hat)
    {
        // set hair length based on preferences
        var hairLength = randomHairLength ? GetRandomElement<HairLength>() : ParticipantPreferences.HairLength;
        
        // set color based on preferences
        var hairColor = randomHairColor ? RandomFromList(AvatarColors._hairColors) : ParticipantPreferences.HairColor;
        
        switch (hairLength)
        {
            case HairLength.Short:
                MakeActive(ShortHair, hairColor);
                break;
            case HairLength.Middle:
                if (hat)
                {
                    MakeActive("hair1", hairColor);
                }
                else
                {
                    MakeActive(RandomFromList(_middleHair), hairColor);
                }
                break;
            default:
            {
                if (hat)
                {
                    MakeActive("hair5", hairColor);
                }
                else
                {
                    MakeActive(RandomFromList(_longHair), hairColor);
                }
                break;
            }
        }

    }

    private static Color RandomColor()
    {
        return Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f)*0.5f;
    }
    
    private static T RandomFromList<T>(T[] List)
    {
        return List[Random.Range(0, List.Length)];
    }
    
    private GameObject FindChild(string childName)
    {
        return _avatar.transform.Find(childName).gameObject;
    }
    
    private static void SetColor(Color color, GameObject child)
    {
        child.GetComponent<Renderer>().material.color = color;
    }
    
    private void MakeActive(string childName)
    {
        FindChild(childName).SetActive(true);
    }
    
    private void MakeActive(string childName, Color color)
    {
        GameObject child = FindChild(childName);
        child.SetActive(true);
        SetColor(color, child);
    }
    
    private static Enum GetRandomElement<T>()where T:Enum
    {
        var names = Enum.GetNames(typeof(T));
        return Enum.Parse<T>(names[Random.Range(0, names.Length)]);
    }

}
