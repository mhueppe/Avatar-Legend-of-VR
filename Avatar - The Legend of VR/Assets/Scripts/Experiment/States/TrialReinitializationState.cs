// using Valve.VR;
//
// /// <summary>
// /// Does nothing until the transition condition is true;
// /// </summary>
// public class TrialReinitialization : IState
// {
//     private readonly TrialManager _manager;
//     public bool ParticipantSignaledReady { get; private set; }
//     private readonly SteamVR_Action_Boolean beginButton;
//
//
//     public TrialReinitialization(TrialManager manager, SteamVR_Action_Boolean beginButton)
//     {
//         _manager = manager;
//         this.beginButton = beginButton;
//     }
//
//     public void Tick()
//     {
//         // Save if player signals that he is ready
//         if (beginButton.state)
//         {
//             ParticipantSignaledReady = true;
//             // todo give auditory feedback.
//         }
//     }
//
//     public void OnStateEnter()
//     {
//         _manager.trialNumber++;
//         
//         _manager.currentTrial = new TrialInfo
//         {
//             avatarSetupInfos = _manager.trialSetups[_manager.trialNumber].avatarSetupInfos,
//             cardSetupInfos = _manager.trialSetups[_manager.trialNumber].cardSetupInfos,
//             participantPreferences = _manager.participantPreferences,
//             trialNumber = _manager.trialNumber,
//             participantID = _manager.ParticipantID
//         };
//     }
//     
//     
//
//     public void OnStateExit()
//     {
//         ParticipantSignaledReady = false;
//     }
// }
