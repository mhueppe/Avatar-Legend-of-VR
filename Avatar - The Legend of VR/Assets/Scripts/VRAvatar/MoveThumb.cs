using UnityEngine;

namespace VRAvatar
{
    /// <summary>
    /// Subscribes to changes on the touchpad and interpolates the animator parameters respectively.
    /// </summary>
    [RequireComponent(typeof(Animator))]
    public class MoveThumb : MonoBehaviour
    {
        [SerializeField] private float interpolationSpeed = 1f;

        private Animator _animator;

        private float _currentValue = 0f;
        private float _targetValue = 0f;
        private static readonly int TouchpadX = Animator.StringToHash("x");

        private void Start() => _animator = GetComponent<Animator>();

        public void OnTouchpadValueChanged(LeftHand leftHand, Vector2 vec)
        {
            // only take the x dim move thumb
            var x = vec.x;
            _targetValue = 1 - ((x + 1) / 2);
        }

        private void Update()
        {
            _currentValue = Mathf.MoveTowards(_currentValue, _targetValue, Time.deltaTime * interpolationSpeed);
            _animator.SetFloat(TouchpadX, _currentValue);
        }
    }
}
