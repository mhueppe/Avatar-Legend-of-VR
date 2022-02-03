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
    private void SetBasedOnPreferenceLevel(int preferenceLevel, bool randomClothingStyle, bool randomHairLength, bool randomHat, bool randomGlasses)
    {
        SetClothing(randomClothingStyle, preferenceLevel);
        SetHair(randomHairLength, preferenceLevel, SetHat(randomHat));
        SetSkinColor(preferenceLevel);
        SetEyeColor(preferenceLevel);
        SetGlasses(randomGlasses);
    }
    void Start()
    {   
        // make the avatar object and instantiate it at the position of the field
        _avatar = Instantiate(avatarPrefab, avatarField.position, avatarField.quaternion).GameObject();
        _avatar.GetComponent<global::Avatar>().QuestionnaireMatch = questionnaireMatch;

        bool randomSwitch = Random.value > 0.5; 
        switch (questionnaireMatch)
        {
            case 0:
                SetBasedOnPreferenceLevel(0,true, true, true, true);
                break;
                
            case 1:
                SetBasedOnPreferenceLevel(1,true, true, randomSwitch, !randomSwitch);
                break;
            
            case 2:
                SetBasedOnPreferenceLevel(2,true, true, false, false);
                break;

            case 3:
                SetBasedOnPreferenceLevel(3,true, false, false, false);
                break;

            case 4:
                SetBasedOnPreferenceLevel(4,false, false, false, false);
                break;
        }

    }

    private void SetEyeColor(int preferenceLevel)
    {
        // set skin color based on preferences
        Color eyeColor = AvatarColors.GetColorOnPreference(preferenceLevel, ParticipantPreferences.EyeColor, AvatarColors._eyeColors);
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
    
    private void SetClothing(bool randomClothingStyle, int preferenceLevel)
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
        colorTop = AvatarColors.GetColorOnPreference(preferenceLevel, ParticipantPreferences.FavouriteColor, AvatarColors._eyeColors);
        colorBottom = RandomFromList(new []{colorTop * (float) 0.3, Color.black, colorTop * (float) 1.1});

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
    
    private void SetSkinColor(int preferenceLevel)
    {
        // set skin color based on preferences
        var skinColor = AvatarColors.GetColorOnPreference(preferenceLevel, ParticipantPreferences.SkinColor, AvatarColors._skinColors);
        SetColor(skinColor, FindChild("avatar_mesh"));
    }

    private void SetHair(bool randomHairLength, int preferenceLevel, bool hat)
    {
        // set hair length based on preferences
        var hairLength = randomHairLength ? GetRandomElement<HairLength>() : ParticipantPreferences.HairLength;
        
        // set color based on preferences
        var hairColor = AvatarColors.GetColorOnPreference(preferenceLevel, ParticipantPreferences.HairColor, AvatarColors._hairColors);
        
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
