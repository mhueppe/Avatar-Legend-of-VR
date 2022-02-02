using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    // Start is called before the first frame update

    public int steps; 
    private bool _highlightAdded = false;

    private void Start(){
        this._highlightAdded = false;
    }

    public void Highlight(){
        var outline = this.gameObject.GetComponent<Outline>();
        outline.OutlineMode = Outline.Mode.OutlineAndSilhouette;
        outline.OutlineColor = new Color(0.78f, 0.41f, 0.39f);
        outline.OutlineWidth = 10f;
        this._highlightAdded = true; 
    }

    public void Dim(){
        var outline = this.gameObject.GetComponent<Outline>();
        outline.OutlineMode = Outline.Mode.OutlineAll;
        outline.OutlineColor = Color.yellow;
        outline.OutlineWidth = 0f;
        this._highlightAdded = true; 
    }

}
