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
        Age = 19, ClothingStyle = ClothingStyle.Casual, EyeColor = Color.black, FavouriteColor = new (155f/255f,231f/255f,70f/255f), LikesHats = Opinion.Yes,
        HairColor = new(250f/255f,228f/255f,169f/255f), HairLength = HairLength.Middle, LikesGlasses = Opinion.Yes, SkinColor = new (104f/255f, 48f/255f, 33f/255f),
    };
    
    // Start is called before the first frame update
    void Start()
    {
        foreach (var avatarField in avatarFields)
        {
            _buildAvatar.Build(avatarPrefab, eyeMaterial, avatarField, _participantPreferences);
        }
    }
    
}
