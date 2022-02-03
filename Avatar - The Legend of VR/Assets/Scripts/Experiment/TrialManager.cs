using UnityEngine;
using VRAvatar;

public class TrialManager : MonoBehaviour
{
    private readonly StateMachine _stateMachine = new();
    
    [SerializeField] private VRPlayer player;
    
    private TrialInfo _currentTrial;

    public void Awake()
    {
        _currentTrial = ExperimentController.Instance.InitNewTrial();

        // give him cards
        var refillHandCards = new RefillHandCards(
            _currentTrial.cardSetupInfos.cardStaple,
            player,
            _currentTrial.cardSetupInfos.numberOfHandCardsAtStart,
            0.5f);

        // see cards - preview them - select one
        var selectCard = new PlayerSelectCardState(player, _currentTrial);

        // Move VRPlayer - activate navmesh from potentially following avatars
        var movePlayer = new MovePlayerState(player, _currentTrial);
        
        // if reached selection state, select/swap
        
        // if not reached but cards are empty, refill hand cards
        
        // done state
        var done = new DoneState();


        // refill hand cards --()--> select card
        _stateMachine.AddTransition(refillHandCards, selectCard, () => refillHandCards.AreRefilled);
        // select card --()--> move player
        _stateMachine.AddTransition(selectCard, movePlayer, () => selectCard.CardSelected);
        
        
        // move player --(if enough handcards)--> select card
        _stateMachine.AddTransition(movePlayer, selectCard, () => movePlayer.FinishedMovement && player.leftHand.handCards.NumberOfCards > 0);
        // move player --(if no more cards on hand)--> refill cards
        _stateMachine.AddTransition(movePlayer, refillHandCards, () => movePlayer.FinishedMovement && player.leftHand.handCards.NumberOfCards == 0);
        // move player --(if goal reached)--> save trial
        _stateMachine.AddTransition(movePlayer, done, () => movePlayer.ReachedGoalField);
        
        
        // Start state
        _stateMachine.SetState(refillHandCards);
    }

    private void Start()
    {
        // Other setups which are not related to state machine
    }

    private void Update() => _stateMachine.Tick();
    
}

