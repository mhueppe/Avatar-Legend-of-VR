using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
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
    
    private const string ShortHair = "hair0";
    private readonly string[] _middleHair = { "hair1", "hair2", "hair3", "hair4"};
    private readonly string[] _longHair= { "hair5", "hair6", "hair7"};
    private static readonly int EyeMaterial = Shader.PropertyToID("EyeMaterial");


    private Color[] _skinColors = {new (65f/255f, 31f/255f, 29f/255f),
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

    private Color[] _hairColors =
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

    private Color[] _eyeColors = { 
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
        Color eyeColor = randomEyeColor ? RandomFromList(_eyeColors) : ParticipantPreferences.EyeColor;
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
        Color skin = RandomFromList(_skinColors);
        var skinColor = randomSkinColor ? skin : ParticipantPreferences.SkinColor;
        SetColor(skinColor, FindChild("avatar_mesh"));
    }

    private void SetHair(bool randomHairLength, bool randomHairColor, bool hat)
    {
        // set hair length based on preferences
        var hairLength = randomHairLength ? GetRandomElement<HairLength>() : ParticipantPreferences.HairLength;
        
        // set color based on preferences
        var hairColor = randomHairColor ? RandomFromList(_hairColors) : ParticipantPreferences.HairColor;
        
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
