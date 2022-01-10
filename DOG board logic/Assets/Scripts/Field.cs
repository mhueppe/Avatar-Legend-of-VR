using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{
    public Vector3 position;
    
    public Field prevField; 
    public Field nextField; 

    public Player occupiedBy = null;
    
    void Start(){
         position = transform.position;
     }
}
