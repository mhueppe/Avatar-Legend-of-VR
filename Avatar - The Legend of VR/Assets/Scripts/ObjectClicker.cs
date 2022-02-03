using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class ObjectClicker : MonoBehaviour {

    public float force = 5;
    private Material defaultMaterial; 

    private Transform _selection; 

    private List<Field> lastPath;

    public int steps;

    public bool lastWasCard;

    public Material avatarHighlighMat;
    
    private void Start()
    {
        lastWasCard = false;
    }
    private void Update()
    {   
        if(_selection != null && !this.lastWasCard){
            // decolor (set back to default color) the last selection
            var selectionRenderer = _selection.GetComponent<Renderer>();
            selectionRenderer.material = this.defaultMaterial; 
            _selection = null; 

            // decolor each of the fields and set the last path back to null
            foreach (Field field in lastPath){
                var rendField = field.GetComponent<Renderer>();
                rendField.material = field.defaultMaterial; 
            }

            lastPath = null; 
        }else if (_selection != null && this.lastWasCard) {
            Card selectedCard = (Card)_selection.gameObject.GetComponent(typeof(Card));
            selectedCard.Dim();
            this.lastWasCard = false;
            _selection = null; 

        }

        // initiate a raycast hit (which saves the object that was hit by the ray)
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);


        // if the ray hit something save it in "hit" and continue
        if (Physics.Raycast(ray, out hit, 100.0f) )
        {
            var selection = hit.transform;
            // if the hitted object has a transform continue
            if (selection != null){
                // if the selcection is a card highlight it and if it's pressed on set the steps on the 
                // steps associated with the card
                Card selectedCard = (Card)selection.gameObject.GetComponent(typeof(Card));

                if (selectedCard != null){        
                    HandleCard(selectedCard, selection);
                }

                // if the selcection is a player highlight the player and it's current path
                Player selectedPlayer = (Player)selection.gameObject.GetComponent(typeof(Player));
                if (selectedPlayer != null && this.steps != 0){
                    HandlePlayer(selectedPlayer, selection);
                }                              
                
                // if the selection is an avatar, highlight the field and store it
            }
        }

    }

    private void HandleCard(Card selectedCard, Transform selection){
        this.lastWasCard = true;
        selectedCard.Highlight();
        this._selection = selection; 

        if (Input.GetMouseButtonDown(0)){
            this.steps = (int)selectedCard.steps;
            Debug.Log(this.steps);
            //Destroy(selection.gameObject);
        }
    }

    private void HandlePlayer(Player selectedPlayer, Transform selection){
        var selectionRenderer = selection.GetComponent<Renderer>();
        if (selectionRenderer != null){
            // highlight the player
            this.defaultMaterial = selectedPlayer.defaultMaterial; 
            selectionRenderer.material = selectedPlayer.highlightMaterial;

            if (this.steps == 13)
            {
                handlePath(13, selectedPlayer);
            }

            if (this.steps == 1)
            {
                handlePath(1, selectedPlayer);
                handlePath(11, selectedPlayer);
            }
            
            if (this.steps == 4)
            {
                // handlePath(4, selectedPlayer);
                handlePath(-4, selectedPlayer);
            }
            else if( this.steps != 13)
            {
                handlePath(this.steps, selectedPlayer);
            }
            
            
            
            this.lastWasCard = false; 
            _selection = selection; 
            
            // if click then go the selected path
            if (Input.GetMouseButtonDown(0)){
                StartCoroutine(selectedPlayer.Move(this.steps));
                this.steps = 0;
            }


        }
    }

   

    private void handlePath(int _steps, Player selectedPlayer){
        // find the path and highlight each field in the path 
        List<Field> path = selectedPlayer.findPath(_steps, selectedPlayer.currentField);
        
        foreach (Field field in path){
            var rendField = field.GetComponent<Renderer>();
            rendField.material = selectedPlayer.highlightMaterial; 
        }

        this.lastPath = path;
    }

}
