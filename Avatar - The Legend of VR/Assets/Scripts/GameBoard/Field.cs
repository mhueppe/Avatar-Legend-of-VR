using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{
    public bool isAvatarField;
    
    public enum LevelOfMatch
    {
        Opposite,
        LessOpposite,
        SlightMatch,
        BetterMatch,
        FullMatch
    }

    public LevelOfMatch questionnaireMatch;
    public Vector3 position => transform.position;
    public Quaternion quaternion => transform.rotation;

    public Field prevField;
    public Field nextField;

    public Player occupiedBy = null;

    public Material defaultMaterial;
    public AvatarField leftOption = null;
    public AvatarField rightOption = null;

    void Start(){
         defaultMaterial = this.GetComponent<Renderer>().material;
     }
}
