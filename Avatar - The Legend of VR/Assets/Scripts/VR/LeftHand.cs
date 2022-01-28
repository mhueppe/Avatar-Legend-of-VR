using UnityEngine;
using UnityEngine.Events;
using Valve.VR;

[RequireComponent(typeof(SteamVR_Behaviour_Pose))]
public class LeftHand : MonoBehaviour
{
    [Range(-1, 1)]
    public float padPositionDummy;
    
    private SteamVR_Behaviour_Pose _pose;
    public SteamVR_Action_Boolean isTouchingPad;
    public SteamVR_Action_Vector2 padPosition;
    
    public UnityEvent<LeftHand, Vector2> onTouchpadChanged;
    public UnityEvent<LeftHand, int> onCardSelectionChanged;
    
    private int _selectedCardIdx = - 1;
    
    public void SetSelectedCard(int value)
    {
        if (value == _selectedCardIdx)
            return;

        _selectedCardIdx = value;
        onCardSelectionChanged?.Invoke(this, _selectedCardIdx);
        Debug.Log(_selectedCardIdx);
    }
    
    // Start is called before the first frame update
    private void Start() => _pose = GetComponent<SteamVR_Behaviour_Pose>();

    // Update is called once per frame
    void Update()
    {
        if (padPosition.GetChanged(_pose.inputSource))
        {
            var touchedPos = padPosition.GetAxis(_pose.inputSource);
            onTouchpadChanged?.Invoke(this, touchedPos);
        } else if (!SteamVR.active)
        {
            onTouchpadChanged?.Invoke(this, new Vector2(padPositionDummy, 0f));
        }
    }
}
