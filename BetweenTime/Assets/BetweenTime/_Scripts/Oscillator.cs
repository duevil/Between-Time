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

        [Tooltip("The velocity of the object")] [SerializeField] [ReadOnly]
        private float velocity;

        private Rigidbody _rb;

        /// <summary>
        ///     Initializes the object's rigidbody
        /// </summary>
        private void Start()
        {
            _rb = GetComponent<Rigidbody>();
        }

        /// <summary>
        ///     If the object is not moving, oscillates the object's position based on the current time
        ///     and set speed and amplitude
        /// </summary>
        private void Update()
        {
            // Skip if the object is moving
            velocity = _rb.linearVelocity.sqrMagnitude;
            if (velocity > 0) return;

            // Oscillate the object up and down
            var position = transform.position;
            position.y += Mathf.Sin(Time.time * speed) * amplitude;
            transform.position = position;
        }
    }
}