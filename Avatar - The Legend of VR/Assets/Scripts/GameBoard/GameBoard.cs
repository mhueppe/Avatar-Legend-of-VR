using System;
using UnityEngine;

[Serializable]
public class GameBoard
{
    public Field startField;
    private Field currentField;

    private bool _initialized = false;

    public bool ReachedGoal => currentField.nextField == null;
    
    
    public Field Walk(int nFields, out GameObject[] avatars)
    {
        if (!_initialized) Init();
        
        // set avatars per default to null so we can return anytime
        avatars = null;

        for (var i = 0; i < nFields; i++)
        {
            // move one field further
            currentField = currentField.nextField;
            
            // if there is no next field we reached the end
            if (currentField.nextField == null)
            {
                Debug.Log("Last field reached: " + currentField.gameObject.name);
                return currentField;
            }
            
            // if there are avatars available we will stop walking any further
            if (AvatarsAvailable(out avatars))
                return currentField;
        }

        return currentField;
    }

    public bool AvatarsAvailable(out GameObject[] avatars)
    {
        avatars = currentField.GetComponent<AvField>()?.avatars;
        return currentField.isAvatarField;
    }

    private void Init()
    {
        _initialized = true;
        currentField = startField;
    }
}
