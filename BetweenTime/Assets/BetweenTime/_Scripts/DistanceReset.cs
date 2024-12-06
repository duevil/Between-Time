using UnityEngine;

namespace BetweenTime._Scripts
{
    /// <summary>
    ///     Resets the object's position and rotation if it moves too far from its initial position
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public class DistanceReset : MonoBehaviour
    {
        private const float MaxDistance = 900f; // The maximum distance the object can move from its initial position

        [Tooltip("The particle effect to play when the object is reset")] [SerializeField]
        public ParticleSystem effect;

        private Vector3 _initialPosition; // The object's initial position
        private Quaternion _initialRotation; // The object's initial rotation
        private Rigidbody _rb; // The object's rigidbody

        /// <summary>
        ///     Stores the object's initial position and rotation
        /// </summary>
        private void Start()
        {
            _initialPosition = transform.position;
            _initialRotation = transform.rotation;
            Debug.Log($"[{name}] Initial position: {_initialPosition}, Initial rotation: {_initialRotation}");
            _rb = GetComponent<Rigidbody>();
        }

        /// <summary>
        ///     Resets the object's position and rotation if it moves too far from its initial position
        ///     (i.e. 1000m) and instantiates the particle effect; if the objects is within the maximum
        ///     distance but not close enough, applies a force to move it further away to cause the reset
        ///     to occur more quickly
        /// </summary>
        private void FixedUpdate()
        {
            var displacement = _initialPosition - transform.position;
            var distanceSquared = displacement.sqrMagnitude;

            switch (distanceSquared)
            {
                // if object is close enough, do nothing
                case <= MaxDistance * 0.01f: break;
                // if object is within the maximum distance, apply a force to move it further away
                case <= MaxDistance:
                {
                    // Calculate the force to apply to the Rigidbody
                    const float forceFactor = 0.01f;
                    var force = displacement.normalized * (forceFactor * distanceSquared);
                    _rb.AddForce(-force); // Force needs to be applied negatively ¯\_(ツ)_/¯
                    break;
                }
                case > MaxDistance:
                    Debug.Log("Object reset");
                    // Past this point, the object has exceeded the maximum distance, so reset it
                    if (effect) Instantiate(effect, transform.position, Quaternion.identity);
                    _rb.linearVelocity = Vector3.zero;
                    _rb.angularVelocity = Vector3.zero;
                    transform.position = _initialPosition;
                    transform.rotation = _initialRotation;
                    break;
            }
        }

        /// <summary>
        ///     Sets the object's initial position and rotation; if no position or rotation is provided,
        ///     the object's current position and rotation are used
        /// </summary>
        /// <param name="position">The position to use as the initial position</param>
        /// <param name="rotation">The rotation to use as the initial rotation</param>
        public void SetInitialTransform(Vector3 position = default, Quaternion rotation = default)
        {
            _initialPosition = position == default ? transform.position : position;
            _initialRotation = rotation == default ? transform.rotation : rotation;
        }
    }
}