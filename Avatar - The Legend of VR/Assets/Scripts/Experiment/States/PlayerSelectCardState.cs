using VRAvatar;

public class PlayerSelectCardState : IState
{
    private readonly VRPlayer _player;
    private readonly TrialInfo _trialInfo;
    
    public bool CardSelected { get; private set; }
    
    public PlayerSelectCardState(VRPlayer player, TrialInfo trialInfo)
    {
        _player = player;
        _trialInfo = trialInfo;
    }
    
    public void OnStateEnter()
    {
        CardSelected = false;
        _player.leftHand.onGrabPressedDown.AddListener(OnHandelLeftHandGrabDown);
        _player.leftHand.handCards.CanSelect = true;
    }
  

    private void OnHandelLeftHandGrabDown(LeftHand leftHand)
    {
        var selectedCard = leftHand.handCards.CurrentlySelectedCard;
        
        if (selectedCard == null) return;
        
        _trialInfo.cardsPicked.Add(selectedCard.steps);

        leftHand.handCards.DeleteCard(selectedCard);
        
        CardSelected = true;
    }

    public void Tick() { }

    public void OnStateExit()
    {
        _player.leftHand.handCards.CanSelect = false;
        _player.leftHand.onGrabPressedDown.RemoveListener(OnHandelLeftHandGrabDown);
    }
}
