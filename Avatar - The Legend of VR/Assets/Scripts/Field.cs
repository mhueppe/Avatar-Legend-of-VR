using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{
    public bool isAvatarField;
    
    public Vector3 position;
    
    public Field prevField; 
    public Field nextField; 

    public Player occupiedBy = null;

    public Material defaultMaterial;
    
    void Start(){
         position = transform.position;
         defaultMaterial = this.GetComponent<Renderer>().material;
     }
}
