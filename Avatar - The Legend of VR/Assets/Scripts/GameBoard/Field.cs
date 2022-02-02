using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{
    public Vector3 position;
    
    public Field prevField; 
    public Field nextField; 

    public Player occupiedBy = null;

    public Material defaultMaterial;
    public bool isAvatarField = false;
    public AvatarField leftOption = null;
    public AvatarField rightOption = null;

    void Start(){
         position = transform.position;
         defaultMaterial = this.GetComponent<Renderer>().material;
     }
}