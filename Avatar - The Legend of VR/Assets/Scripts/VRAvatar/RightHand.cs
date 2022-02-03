using UnityEngine;

public class RightHand : MonoBehaviour
{
    public LaserPointer laserPointer;

    private void Start()
    {
        laserPointer = GetComponentInChildren<LaserPointer>();
        laserPointer.onPointerClick.AddListener(go => Debug.Log(go.name));
    }
}
