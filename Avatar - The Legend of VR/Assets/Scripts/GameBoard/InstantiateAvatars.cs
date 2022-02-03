using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class InstantiateAvatars : MonoBehaviour
{   
    [SerializeField] private Field[] avatarFields;
    [SerializeField] private GameObject avatarPrefab;
    [SerializeField] private Material eyeMaterial;
    // [SerializeField] private ParticipantPreferences _participantPreferences;

    
    private BuildAvatar _buildAvatar = new ();
    private ParticipantPreferences _participantPreferences = new()
    {
        Age = 19, ClothingStyle = ClothingStyle.Casual, EyeColor = Color.black, FavouriteColor = Color.green, LikesHats = Opinion.Yes,
        HairColor = Color.yellow, HairLength = HairLength.Middle, LikesGlasses = Opinion.Yes, SkinColor = Color.black,
    };

    // Start is called before the first frame update
    void Start()
    {
        foreach (var avatarField in avatarFields)
        {
            _buildAvatar.build(avatarPrefab, eyeMaterial, avatarField, _participantPreferences);
        }
    }
    
}
