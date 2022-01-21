using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    // Start is called before the first frame update

    public int steps; 
    private bool highlightAdded = false; 

    void Start(){
        this.highlightAdded = false;
    }

    public void highlight(){
        Outline outline; 
        Debug.Log("In highlight");
        outline = this.gameObject.GetComponent<Outline>();
        outline.OutlineMode = Outline.Mode.OutlineAndSilhouette;
        outline.OutlineColor = new Color(0.78f, 0.41f, 0.39f);
        outline.OutlineWidth = 10f;
        this.highlightAdded = true; 
    }

    public void dim(){
        Outline outline;    
        outline = this.gameObject.GetComponent<Outline>();
        outline.OutlineMode = Outline.Mode.OutlineAll;
        outline.OutlineColor = Color.yellow;
        outline.OutlineWidth = 0f;
        this.highlightAdded = true; 
    }

}
