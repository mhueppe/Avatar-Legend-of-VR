using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class CreateField : MonoBehaviour
{   
    public GameObject fieldPrefab; 
    public bool drawFields;
    public bool destroy; 
    public int nFields = 64;
    public int start = 0; 
    public float distanceBetweenFields = 1.3f; 
    // Start is called before the first frame update
    void Start()
    {   
        
    }

    // Update is called once per frame
    void Update()
    {   
        if(drawFields){
            drawFields = false;
            List<Field> path = new List<Field>(); 
            GameObject turtle = new GameObject();
            turtle.transform.position = transform.position;
            turtle.transform.rotation = transform.rotation;
            
            // Quaternion[] rotations = {Quaternion.Euler(0, 90, 0), Quaternion.Euler(0, -45, 0), Quaternion.Euler(0, -45, 0), Quaternion.Euler(0, 90, 0)};
            Quaternion[] rotations = {Quaternion.Euler(0, 0, 0)};
            int rotIndex = 0;
            int tempSideLength = 16;
    
            for(int i = start; i < start+nFields ; i ++){
                GameObject field = (GameObject) PrefabUtility.InstantiatePrefab(fieldPrefab);
                
                field.transform.position = turtle.transform.position;

                if(i % tempSideLength== 0 && i != 0){
                    turtle.transform.rotation *= rotations[rotIndex++];
                    if (rotIndex == rotations.Length) rotIndex = 0;

       
                }
                
                turtle.transform.position += turtle.transform.forward * distanceBetweenFields;

                // field.transform.SetPositionAndRotation(new Vector3(x*distanceBetweenFields, 0, y*distanceBetweenFields), Quaternion.identity);

                Field fieldLogic = (Field)field.GetComponent(typeof(Field));

                field.name = "Field" + i;
                field.transform.parent = transform; 
                path.Add(fieldLogic);
            }

            DestroyImmediate(turtle);

            for(int i = 0; i < path.Count-1; i++){
                path[i].nextField = path[i+1];
            }

            for(int i = path.Count-1; i > 0; i--){
                path[i].prevField = path[i-1];
            }
            
        }

        if(destroy){
            destroy = false; 
            // foreach(var child in Object.FindObjectsOfType<Field>()){
            //     DestroyImmediate(child);
            // }
            for(int i = 0; i < transform.childCount; i++){
                DestroyImmediate(transform.GetChild(i).gameObject);
            }
        }
        
    }

    [ContextMenu(nameof(CreateBoard))]
    public void CreateBoard()
    {
        drawFields = false;
        List<Field> path = new List<Field>(); 
        GameObject turtle = new GameObject();
        turtle.transform.position = transform.position;
        turtle.transform.rotation = transform.rotation;
            
        Quaternion[] rotations = {Quaternion.Euler(0, 90, 0), Quaternion.Euler(0, -45, 0), Quaternion.Euler(0, -45, 0), Quaternion.Euler(0, 90, 0)};
        int rotIndex = 0; 

        for(int i = 0; i < nFields ; i ++){
            //int y = 0; 
            //int x = i; 
            GameObject field = (GameObject) PrefabUtility.InstantiatePrefab(fieldPrefab);
                
            field.transform.position = turtle.transform.position;

            if(i % 4 == 0 && i != 0){
                turtle.transform.rotation *= rotations[rotIndex++];
                if (rotIndex == rotations.Length) rotIndex = 0;
                Debug.Log(i + "  " + rotations[rotIndex].eulerAngles);
            }
                
            turtle.transform.position += turtle.transform.forward * distanceBetweenFields;

            // field.transform.SetPositionAndRotation(new Vector3(x*distanceBetweenFields, 0, y*distanceBetweenFields), Quaternion.identity);

            Field fieldLogic = (Field)field.GetComponent(typeof(Field));

            field.name = "Field" + i;
            field.transform.parent = transform; 
            path.Add(fieldLogic);
        }

        DestroyImmediate(turtle);

        for(int i = 0; i < path.Count-1; i++){
            path[i].nextField = path[i+1];
        }

        for(int i = path.Count-1; i > 0; i--){
            path[i].prevField = path[i-1];
        }
    }

    [ContextMenu(nameof(DestroyBoard))]
    public void DestroyBoard()
    {
        destroy = false; 
        // foreach(var child in Object.FindObjectsOfType<Field>()){
        //     DestroyImmediate(child);
        // }

        foreach(var child in FindObjectsOfType<Field>()) DestroyImmediate(child);
        
    }
}
