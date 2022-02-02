using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{

    public int steps;

    public void Highlight(){
        var outline = this.gameObject.GetComponent<Outline>();
        outline.OutlineMode = Outline.Mode.OutlineAndSilhouette;
        outline.OutlineColor = new Color(0.78f, 0.41f, 0.39f);
        outline.OutlineWidth = 10f;
    }

    public void Dim(){
        var outline = this.gameObject.GetComponent<Outline>();
        outline.OutlineMode = Outline.Mode.OutlineAll;
        outline.OutlineColor = Color.yellow;
        outline.OutlineWidth = 0f;
    }

}
