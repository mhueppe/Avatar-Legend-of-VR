using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update

    public int steps; 

    float speed = 20f; 

    public bool isMoving = false; 

    public Field startingField;

    public Field currentField; 
    
    public int team; 

    public int pieceNumber;

    float middleHeight;

   void Start()
    {
        currentField = startingField;
        middleHeight = transform.localScale.y; 
        transform.position = currentField.position + new Vector3(0, middleHeight, 0);

    }
    
    public IEnumerator Move()
    {   
        if(isMoving)
        {
            yield break; 
        }
        
        isMoving = true; 

        while (steps > 0)
        {      
             Field next = findNextField();       
             currentField.occupiedBy = null;                                       
             Vector3 nextPos; 
            // checks if next is valid meaning if it is occupied or not -> if it is occupied by a piece of the same Team it is still valid
            // chech if occupied
             if (next.occupiedBy == null){
                 // if no one occupies the field you want to move to move to that field 
                 nextPos = next.position + new Vector3(0,  middleHeight, 0); 
                //  Debug.Log("No one occupies this field so i can go");
             }             
             else {
                // if the field is occupied and the piece that it is occupied by is a different team to this piece then this player can kick
                // the player that occupies it of the field 
                // but only if it's his last step
                 if (steps == 1){
                    if (next.occupiedBy.team == this.team){
                    // but only if it's his last step
                    // if they are both the same team they are allowed to be on the same playing field thus move the current piece next to the one occupying this field
                    nextPos = (next.position + new Vector3(0.5f, middleHeight,0.5f));  
                    next.occupiedBy.transform.position = Vector3.MoveTowards(next.occupiedBy.transform.position, (next.position + new Vector3(-0.5f, middleHeight , -0.5f)), speed * Time.deltaTime);                 
                    }
                    else{
                        Debug.Log("Im schubsen drinne.");
                        nextPos = next.position + new Vector3(0,  middleHeight, 0);             
                        next.occupiedBy.transform.position = next.occupiedBy.startingField.position + new Vector3(0,  middleHeight, 0);
                        next.occupiedBy.currentField = next.occupiedBy.startingField; 
                        ((HomeField)next.occupiedBy.startingField.nextField).playerState[next.occupiedBy.pieceNumber] = false; 
                    }                 
                }
                else{
                 nextPos = next.position + new Vector3(0,  middleHeight, 0); 
                }
             }
             

             next.occupiedBy = this;            
             while (MoveToNextNode(nextPos)){yield return null;}
             yield return new WaitForSeconds(0.1f);
                
             currentField = next;             
             steps--; 

        }

        isMoving = false; 

    }
    
    bool MoveToNextNode(Vector3 goal)
    {
        return goal != (transform.position = Vector3.MoveTowards(transform.position, goal, speed * Time.deltaTime));
    }

    Field findNextField()
    {
        // checks whether or not the current field is a home field. if it is check if it's from your respective team if
            if(currentField.GetType() == typeof(HomeField) && ((HomeField)currentField).team == this.team ){
            // if both condition hold check if you've already passed this starting point (if you just started your round or not)
                 if(((HomeField)currentField).playerState[pieceNumber]){
                    // if you did a full leap set the next field to the goal field 
                    return ((HomeField)currentField).passedField;
                 }else{
                    ((HomeField)currentField).playerState[pieceNumber] = true; 
                    // return currentField.nextField; 
                    return currentField.prevField; 
                 }
             }else{
                 // if one of these condition does not hold the next field is the next in line
                //  return currentField.nextField; 
                 return currentField.prevField; 
             }
    }


}
