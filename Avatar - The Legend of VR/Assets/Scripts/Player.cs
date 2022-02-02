using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    float speed = 4f; 

    public bool isMoving = false; 

    public Field startingField;

    public Field currentField; 

    public List<Field> currentPath;
    
    public int team; 

    public int pieceNumber;

    public bool started = false;

    public Material defaultMaterial; 
    public Material highlightMaterial;

    private Animator animator;
    private Companion companion;


   void Start()
    {
        currentField = startingField;
        Debug.Log("current field:"+ currentField+ ", position:"+ currentField.position+ ", transform position:"+ currentField.transform.position);
        transform.position = currentField.transform.position;
        defaultMaterial = this.GetComponent<Renderer>().material;
        animator = this.GetComponent<Animator>();
    }

      Field findNextField(Field field, bool previous = false){
            // checks whether or not the current field is a home field. if it is check if it's from your respective team
            if(field.GetType() == typeof(HomeField) && ((HomeField)field).team == this.team && started){
                // if both condition hold check if you've already passed this starting point (if you just started your round or not)
                // if you did a full leap set the next field to the goal field 
                // return ((HomeField)field).passedField;
                return ((HomeField)field).nextField;
            }
            
            started = true; 
            // if one of these condition does not hold the next field is the next in line
            //  return field.nextField; 
            return previous ? field.prevField : field.nextField;
      }

    public List<Field> findPath(int steps, Field pathStartingField){
        // Field[] path =  new Field[steps];
        
        List<Field> path = new List<Field>(); 

        // get the path starting with the Field the piece is currently on
        Field currentFieldInPath = pathStartingField; 

        Field nextField;


        for (int i = 0; i < steps; i++)
        {
            // for each step find the corresponding field and add to the path list
            nextField = findNextField(currentFieldInPath, steps<0);
            path.Add(nextField);
            currentFieldInPath = nextField;
        }

        this.currentPath = path; 

        // return the path aka the list with all fields that are going to be visited
        return path;
    }

    public Vector3 findPositionOfNextField(Field nextField, bool lastStep){
        // checks if next is valid meaning if it is occupied or not -> if it is occupied by a piece of the same Team it is still valid
        // chech if occupied
        if (nextField.occupiedBy != null && lastStep) {
                 // if it's the last step check whether or not you can kick somebody off the playing field 
                Player otherPlayer = nextField.occupiedBy;
                if (otherPlayer.team == this.team){
                    // if they are both the same team they are allowed to be on the same playing field 
                    // thus move the current piece next to the one occupying this field
                    otherPlayer.transform.position = Vector3.MoveTowards(otherPlayer.transform.position, (nextField.position + new Vector3(-0.5f, 0 , -0.5f)), speed * Time.deltaTime);                 
                    return (nextField.position + new Vector3(0.5f, 0 ,0.5f));  

                } else{
                    // if the field is occupied and the piece that it is occupied by is a different team to this piece then this player can kick
                    // the player that occupies it of the field 
                    kickPlayerOff(otherPlayer);
                }                 
        } 

        return nextField.position; 
        
    }

    
    public void kickPlayerOff(Player otherPlayer){
        // kick player off the filed and reset 
        otherPlayer.currentField = otherPlayer.startingField; 
        otherPlayer.transform.position = otherPlayer.startingField.position;
        otherPlayer.started = false;
    }

    public IEnumerator Move(int steps){
        if(isMoving)
        {
            yield break; 
        }

        isMoving = true; 

        // if the path is not already specified calculate it
        if(this.currentPath.Count == 0){
            this.currentPath = findPath(steps, this.currentField);
        }

        Field nextFieldInPath;

        if (animator)
        {
            animator.SetBool("isWalking", true);
        }


        // move from each field in the path to the next one
        while(this.currentPath.Count > 0){
            this.currentField.occupiedBy = null; // leave the field
            nextFieldInPath = this.currentPath[0];
            

            if((this.currentPath.Count == 1 || nextFieldInPath.isAvatarField) && animator)
            {
                animator.SetBool("isWalking", false);
            }

            Vector3 nextPos = findPositionOfNextField(nextFieldInPath, this.currentPath.Count == 1); 
            
            // the next field is going to be occupied by the current player
            nextFieldInPath.occupiedBy = this;
            Quaternion nextRotation = nextFieldInPath.transform.rotation;

            // move to the next Field
            while (MoveToNextField(nextPos, nextRotation)){yield return null;}
            //yield return new WaitForSeconds(0.1f);

            this.currentField = nextFieldInPath;

            if (currentField.isAvatarField)
            {
                // TODO: stop and allow for avatar selection
                yield return new WaitForSeconds(0.5f);
                // resume walking
                if (currentPath.Count > 1 && animator)
                {
                    animator.SetBool("isWalking", true);
                }
            }

            this.currentPath.RemoveAt(0);
        }

        

        // the player finished his path thus it's not longer moving
        isMoving = false;         
    }

    bool MoveToNextField(Vector3 goal, Quaternion targetRotation){
        // moves this player to the given goal position

        // The step size is equal to speed times frame time.
        var step = 200f * Time.deltaTime;

        // Rotate our transform a step closer to the target's.
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, step);

        return goal != (transform.position = Vector3.MoveTowards(transform.position, goal, speed * Time.deltaTime));
    }

}
