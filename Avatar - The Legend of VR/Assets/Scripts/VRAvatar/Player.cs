using UnityEngine;
using Valve.VR;

namespace VRAvatar
{

    /// <summary>
    /// Script interface to the Player controlled by the participant.
    /// </summary>
    public class Player : MonoBehaviour
    {
        public LeftHand leftHand;
        public SteamVR_Behaviour_Pose leftControllerPose;
        public SteamVR_Behaviour_Pose rightControllerPose;

        public SteamVR_Action_Boolean triggerPressed;

    }
}
