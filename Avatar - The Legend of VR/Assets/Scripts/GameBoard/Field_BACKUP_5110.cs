using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{
    public bool isAvatarField;

    public Vector3 position => transform.position;
    public Quaternion quaternion => transform.rotation;
    
    public Field prevField;
    public Field nextField;

    public Material defaultMaterial;
<<<<<<< HEAD:Avatar - The Legend of VR/Assets/Scripts/Field.cs
    public bool isAvatarField = false;
    public AvatarField leftOption = null;
    public AvatarField rightOption = null;
=======
>>>>>>> 5f0d6c7002df1c503f193e2723d05adc5e0da564:Avatar - The Legend of VR/Assets/Scripts/GameBoard/Field.cs

    void Start(){
         defaultMaterial = this.GetComponent<Renderer>().material;
     }
}
