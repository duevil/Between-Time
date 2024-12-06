using UnityEngine;

namespace BetweenTime._Scripts
{
    /// <summary>
    ///     Oscillates the object up and down when it is not moving
    /// </summary>
    public class Oscillator : MonoBehaviour
    {
        [Tooltip("The speed of the object's oscillation")] [SerializeField]
        private float speed = 1.5f;

        [Tooltip("The amplitude of the object's oscillation")] [SerializeField]
        private float amplitude = 0.00001f;

        private Rigidbody _rb; // The object's rigidbody

        /// Random offset to prevent all objects from oscillating in sync
        private float _yOffset;

        /// <summary>
        ///     Initializes the object's rigidbody
        /// </summary>
        private void Start()
        {
            TryGetComponent(out _rb);
            _yOffset = Random.Range(0f, 2f * Mathf.PI);
        }

        /// <summary>
        ///     If the object is not moving, oscillates the object's position based on the current time
        ///     and set speed and amplitude
        /// </summary>
        private void Update()
        {
            // Skip if the object is moving
            if (_rb is not null && _rb.linearVelocity.sqrMagnitude > 0) return;

            // Oscillate the object up and down
            var position = transform.position;
            position.y += Mathf.Sin(Time.time * speed + _yOffset) * amplitude;
            transform.position = position;
        }
    }
}