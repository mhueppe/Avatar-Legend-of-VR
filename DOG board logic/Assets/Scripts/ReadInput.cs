using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadInput : MonoBehaviour
{   
    public void ReadIntegerInput(string s){
        int piece;
        // attempt to parse the value using the TryParse functionality of the integer type
        int.TryParse(s, out piece);
        
        Debug.Log(piece);
    }
}
