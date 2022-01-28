using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dogs : MonoBehaviour
{
    // Start is called before the first frame update
    public Team redTeam; 
    public Team blueTeam; 
    public Team yellowTeam; 
    public Team greenTeam; 
       
      
    public Team[] teams;
    
    int current = 0; 

    public int chosenPlayer;
    public int steps;

    public bool yourTurn = true; 

    void Start(){
        teams = new Team[] {redTeam, yellowTeam, greenTeam, blueTeam };   // OK

    }

    // Update is called once per frame
    void Update()
    {              
        
        chosenPlayer =  Random.Range(0,3); 

        if( teams[0].allowMovement() && teams[1].allowMovement() && teams[2].allowMovement() && teams[3].allowMovement()  )
        {  
            if(current == 0){
                // if(teams[current].color == "yellow"){
            //     steps = 1; 
            // }
            // int steps = Input.GetKeyDown(KeyCode.Space)

            // if (current == 0){
            //     teams[current].Move(chosenPlayer, steps);
            // }else{
            //     teams[current].Move(Random.Range(0,4), Random.Range(1,9));                
            // }

            // teams[current].Move(Random.Range(0,4), Random.Range(2,9));    
                // if(current < 3){
                //     current++; }
                // else
                // {
                //     current = 0; 
                // }
            }
            

        }   
    }


    void Script(){
        Debug.Log("Which card do you wanna give your partner?");
        // at the beginning of the game switch one of you cards with your partner


        Debug.Log("Which piece do you wanna play?");
        // at the beginning of each move select which piece you watnt

        Debug.Log("Which card do you wanna play?");

    }
}
