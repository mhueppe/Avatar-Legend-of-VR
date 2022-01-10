using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeField : Field
{
    
    public static int RED= 0;
    public static int BLUE= 1;
    public static int YELLOW= 2;    
    public static int GREEN= 3;

    public Field passedField; 

    public int team; 

    public Dictionary<int, bool> playerState = new Dictionary<int, bool>()
    {
        [0] = false,
        [1] = false,
        [2] = false,
        [3] = false,
    };


}
