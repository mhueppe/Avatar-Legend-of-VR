using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{
    public bool isAvatarField;

    public int questionnaireMatch;
    public Vector3 position => transform.position;
    public Quaternion quaternion => transform.rotation;
    
    public Field prevField;
    public Field nextField;

    public Material defaultMaterial;

    void Start(){
         defaultMaterial = this.GetComponent<Renderer>().material;
     }
}
