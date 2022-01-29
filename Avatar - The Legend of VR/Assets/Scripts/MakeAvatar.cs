
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeAvatar : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject AvatarPrefab;

    [SerializeField] private static AvatarField avatarField1; 
    [SerializeField] private static AvatarField avatarField2; 
    [SerializeField] private static AvatarField avatarField3; 
    [SerializeField] private static AvatarField avatarField4; 
    [SerializeField] private static AvatarField avatarField5; 
    [SerializeField] private static AvatarField avatarField6; 

    public ParticipantPreferences participantPreferences;

    private AvatarField[] AvatarFields = new AvatarField[]
        {avatarField1, avatarField2, avatarField3, avatarField4, avatarField5, avatarField6};
    
    string[] pants = new string[] { "pants", "sporty_pants"};
    string[] shirts = new string[] { "casual_shirt", "sporty_shirt", "chique_shirt"};

    private string short_hair = "hair0";
    string[] middle_hair = new string[] { "hair1", "hair2", "hair3", "hair4"};
    string[] long_hair = new string[] { "hair5", "hair6", "hair7"};
    

    void Start()
    {

        foreach (var avatarField in AvatarFields)
        {
            if (avatarField.questionnareMatch >= 100)
            {
                setColor(participantPreferences.SkinColor, findChild("avatar"));
                activateGlasses();
                activateClothing();
                activateHat();
            }
        }
    }

    void setColor(Color color, GameObject gameObject)
    {
        gameObject.GetComponent<Renderer>().sharedMaterial.color = color;
    }

    void activateGlasses()
    {
        switch (participantPreferences.LikesGlasses)
        {
            case Opinion.Yes:
                MakeActive("glasses");
                break;
            case Opinion.Neutral:
            {
                if (Random.value > 0.5)
                {
                    MakeActive("glasses");
                }

                break;
            }
        }
    }
    
    
    void activateClothing()
    {
        switch (participantPreferences.ClothingStyle)
        {
            case ClothingStyle.Casual:
                activateHat();
                MakeActive("pants");
                MakeActive("casual_shirt");
                break;
            case ClothingStyle.Chique:
                MakeActive("pants");
                MakeActive("chique_shirt");
                break;
            case ClothingStyle.Sporty:
                activateHat();
                MakeActive("sporty_yoga_pants");
                MakeActive("sporty_shirt");
                break;
        }
    }
    
    void activateAllHair()
    {
        switch (participantPreferences.HairLength)
        {
            case HairLength.Short:
                MakeActive(short_hair, participantPreferences.HairColor);
                break;
            case HairLength.Middle:
                MakeActive(getRandomElement(middle_hair), participantPreferences.HairColor);
                break;
            default:
            {
                if (participantPreferences.HairLength == HairLength.Middle)
                {
                    MakeActive(getRandomElement(long_hair), participantPreferences.HairColor);
                }

                break;
            }
        }
    }

    void activateHat()
    {
        MakeActive("hat", participantPreferences.FavouriteColor);
        switch (participantPreferences.HairLength)
        {
            case HairLength.Short:
                MakeActive(short_hair, participantPreferences.HairColor);
                break;
            case HairLength.Middle:
                MakeActive("hair1", participantPreferences.HairColor);
                break;
            default:
            {
                if (participantPreferences.HairLength == HairLength.Middle)
                {
                    MakeActive("hair5", participantPreferences.HairColor);
                }

                break;
            }
        }
    }

    string getRandomElement(string[] ListofAttributes)
    {
        return ListofAttributes[Random.Range(0, ListofAttributes.Length)];
    }

    GameObject findChild(string childName)
    {
        return this.AvatarPrefab.transform.Find(childName).gameObject;
    }
    void MakeActive(string childName)
    {
       findChild(childName).SetActive(true);
    }
    
    void MakeActive(string childName, Color color)
    {
        GameObject child = findChild(childName);
        child.SetActive(true);
        setColor(color, child);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
