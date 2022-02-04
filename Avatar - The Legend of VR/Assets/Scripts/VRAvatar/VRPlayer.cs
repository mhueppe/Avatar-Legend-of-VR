using UnityEngine;
using Valve.VR;

namespace VRAvatar
{

    /// <summary>
    /// Script interface to the Player controlled by the participant.
    /// </summary>
    public class VRPlayer : MonoBehaviour
    {
        public LeftHand leftHand;
        public RightHand rightHand;
        public SteamVR_Behaviour_Pose leftControllerPose;
        public SteamVR_Behaviour_Pose rightControllerPose;
    }
}
