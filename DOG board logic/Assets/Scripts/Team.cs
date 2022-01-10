using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Team : MonoBehaviour
{
    // Start is called before the first frame update
    public Player player; 
    public Player player1; 
    public Player player2; 
    public Player player3; 

    public string color;
    
    public Player[] players;
    

    void Start(){
        players = new Player[] { player, player1, player2, player3 }; 
    }

    public void Move(int player, int steps)
    {
        players[player].steps = steps; 
        StartCoroutine(players[player].Move());
        
    }

}
