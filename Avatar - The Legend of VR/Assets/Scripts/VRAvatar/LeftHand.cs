using UnityEngine;
using UnityEngine.Events;
using Valve.VR;

namespace VRAvatar
{
    [RequireComponent(typeof(SteamVR_Behaviour_Pose))]
    public class LeftHand : MonoBehaviour
    {
        #region Debug
        
        [Range(-1, 1)] public float padPositionDummy;
        public bool isTouchPadTouched;
        private bool _isTouchedLastFrame = true;
        public bool pressGrab;
        
        #endregion
        
        private SteamVR_Behaviour_Pose _pose;
        
        public HandCards handCards;
        
        #region SteamVR Actions and Events
        
        [SerializeField] private SteamVR_Action_Vector2 padPosition;
        public UnityEvent<LeftHand, Vector2> onTouchpadChanged;
        
        [SerializeField] private SteamVR_Action_Boolean isTouchingPad;
        public UnityEvent<LeftHand, bool> onTouchpadTouchedChanged;
        
        [SerializeField] private SteamVR_Action_Boolean grabPressed;
        public UnityEvent<LeftHand> onGrabPressedDown;

        #endregion
        
        
        // Start is called before the first frame update
        private void Start() => _pose = GetComponent<SteamVR_Behaviour_Pose>();

        // Update is called once per frame
        void Update()
        {
            #region Is Touchpad Pressed
            
            if (isTouchingPad.GetChanged(_pose.inputSource))
            {
                onTouchpadTouchedChanged?.Invoke(this, isTouchingPad.state);
            }else if (!SteamVR.active)
            {
                if (isTouchPadTouched != _isTouchedLastFrame)
                {
                    _isTouchedLastFrame = isTouchPadTouched;
                    onTouchpadTouchedChanged?.Invoke(this, isTouchPadTouched);
                }
            }
            
            #endregion
            
            #region Touchpad Position
            
            if (padPosition.GetChanged(_pose.inputSource))
            {
                var touchedPos = padPosition.GetAxis(_pose.inputSource);
                onTouchpadChanged?.Invoke(this, touchedPos);
            }
            else if (!SteamVR.active)
            {
                onTouchpadChanged?.Invoke(this, new Vector2(padPositionDummy, 0f));
            }
            
            #endregion

            #region PressGrab
            
            if (grabPressed.GetStateDown(_pose.inputSource))
            {
                onGrabPressedDown?.Invoke(this);
            } else if (!SteamVR.active)
            {
                if (pressGrab)
                {
                    pressGrab = false;
                    onGrabPressedDown?.Invoke(this);
                }
            }
            #endregion
        }
    }
}
