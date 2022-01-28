using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Route : MonoBehaviour
{
    // Start is called before the first frame update

    Field[] childObjects; // store all the child objects in a Transform array
    public List<Field> childObjectsList = new List<Field>(); 
    
    void Start()
    {
        FillNodes();
    }

    #if UNITY_EDITOR
    void OnDrawGizmos()
    {
        Gizmos.color = Color.green; // create a line to draw between each field
        FillNodes(); // call the FillNodes method

        // draw the line from each Field to it's previous one
        for (int i = 0; i < childObjectsList.Count; i++)
        {
            Vector3 currentPos = childObjectsList[i].position; // current field
            if(i > 0){
                Vector3 prevPos = childObjectsList[i-1].position;
                Gizmos.DrawLine(prevPos, currentPos); // previous field
            }
        }
    }
    #endif

    void FillNodes()
    {   

        childObjectsList.Clear();

        // get the Components in the Children
        childObjects = GetComponentsInChildren<Field>();
        
        // populate the ChildObjects list
        foreach (Field child in childObjects)
        {
            if(child != this.transform){
                childObjectsList.Add(child);
        
            }
        }
    }
}
