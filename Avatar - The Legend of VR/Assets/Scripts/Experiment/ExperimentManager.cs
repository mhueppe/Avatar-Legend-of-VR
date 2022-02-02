using UnityEngine;
using Avatar = VRAvatar.Avatar;

public class ExperimentManager : MonoBehaviour
{
    #region Readonly
    
    private readonly StateMachine _stateMachine = new();
    
    #endregion
    
    #region Editor Exposed
    
    public Avatar Avatar;

    public CardInformationScriptableObject[] cardInformation;
    
    #endregion
    
    public int ParticipantID { get; private set; }
    public ParticipantPreferences participantPreferences = new();
    
    
    private int NumberOfTrials => cardInformation?.Length ?? 0;
    public int trialNumber;

    public TrialInfo currentTrial;

    public void Awake()
    {
        currentTrial = new TrialInfo
        {
            cardInformation = cardInformation[0],
            participantPreferences = participantPreferences,
            trialNumber = trialNumber,
            participantID = ParticipantID
        };
        
        
        // give him cards
        var refillHandCards = new RefillHandCards(
            currentTrial.cardInformation.cardStaple,
            Avatar,
            currentTrial.cardInformation.numberOfHandCardsAtStart,
            0.5f);

        // see cards - preview them - take them into the right hand - play them
        
        // Move Player - activate navmesh from potentially following avatars
        
        // if reached selection state, select/swap
        
        // if not reached but cards are empty, refill hand cards
        
        // done state
        var done = new DoneState();

        var trialReinitialization = new TrialReinitialization(this, Avatar.triggerPressed);


        _stateMachine.AddTransition(refillHandCards, done, () => refillHandCards.AreRefilled);
        // _stateMachine.AddTransition(trialReinitialization, refillHandCards, () => trialReinitialization.ParticipantSignaledReady);
        
        
        // Start state
        _stateMachine.SetState(refillHandCards);
    }

    private void Start()
    {
        // Other setups which are not related to state machine
    }

    private void Update() => _stateMachine.Tick();
    
}

