using UnityEngine;

public class DoneState : IState
{
    public void OnStateEnter()
    {
        Debug.Log("Done");
    }

    public void Tick()
    {
    }

    public void OnStateExit()
    {
    }
}
