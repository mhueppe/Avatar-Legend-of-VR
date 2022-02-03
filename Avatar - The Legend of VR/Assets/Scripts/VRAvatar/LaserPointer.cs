using UnityEngine;
using UnityEngine.Events;
using Valve.VR.Extras;

[RequireComponent(typeof(SteamVR_LaserPointer))]
public class LaserPointer : MonoBehaviour
{
    public bool LaserPointerEnabled { get => _laserPointer.active; set => _laserPointer.active = value; }
    public UnityEvent<GameObject> onPointerClick;
    private SteamVR_LaserPointer _laserPointer;

    private void Start()
    {
        _laserPointer = GetComponent<SteamVR_LaserPointer>();
        _laserPointer.PointerIn += OnPointerIn;
        _laserPointer.PointerOut += OnPointerOut;
        _laserPointer.PointerClick += OnPointerClick;
    }

    
    private void OnPointerIn(object sender, PointerEventArgs e)
    {
        if (e.target.CompareTag("Avatar"))
        {
            _laserPointer.color = Color.blue;
        }        
    }
    
    private void OnPointerOut(object sender, PointerEventArgs e)
    {
        _laserPointer.color = Color.black;
    }
    
    private void OnPointerClick(object sender, PointerEventArgs e)
    {
        onPointerClick?.Invoke(e.target.gameObject);
    }

    
}
