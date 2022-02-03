
using UnityEngine;
using Valve.VR;

/// <summary>
/// Does nothing until the transition condition is true;
/// </summary>
public class TrialReinitialization : IState
{
    private readonly ExperimentManager _manager;
    public bool ParticipantSignaledReady { get; private set; }
    private readonly SteamVR_Action_Boolean beginButton;


    public TrialReinitialization(ExperimentManager manager, SteamVR_Action_Boolean beginButton)
    {
        _manager = manager;
        this.beginButton = beginButton;
    }

    public void Tick()
    {
        // Save if player signals that he is ready
        if (beginButton.state)
        {
            ParticipantSignaledReady = true;
            // todo give auditory feedback.
        }
    }

    public void OnStateEnter()
    {
        _manager.trialNumber++;
        
        _manager.currentTrial = new TrialInfo
        {
            condition = Condition.Custom,
            cardInformation = _manager.cardInformation[_manager.trialNumber],
            participantPreferences = _manager.participantPreferences,
            trialNumber = _manager.trialNumber,
            participantID = _manager.ParticipantID
        };

        // Save to _manager.CurrentTrial to file
    }
    
    

    public void OnStateExit()
    {
        // Play start sound?
        Debug.Log("Left wait");
        ParticipantSignaledReady = false;
    }
}
