using System.Collections;
using System.Linq;
using UnityEngine;
using VRAvatar;

public class MovePlayerState : IState
{
    private readonly TrialInfo _currentTrial;
    private readonly VRPlayer _player;
    public bool FinishedMovement { get; private set; }
    public bool ReachedGoalField { get; private set; }

    public MovePlayerState(VRPlayer player, TrialInfo currentTrial)
    {
        _player = player;
        _currentTrial = currentTrial;
    }

    public void OnStateEnter()
    {
        var previouslySelectedCard = _currentTrial.cardsPicked.Last();
        // todo subscribe some event
        _player.StartCoroutine(Wait());
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(1f);
        FinishedMovement = true;
    }

    private void HandleSomeEvent()
    {
        FinishedMovement = true;
        // todo handle if avatar is reached.
        
        // todo handle if goal field reached.
    }

    public void Tick() { }

    public void OnStateExit()
    {
        //todo unsubscribe some event
        _player.StopCoroutine(Wait());
        FinishedMovement = false;
    }
    
    
}