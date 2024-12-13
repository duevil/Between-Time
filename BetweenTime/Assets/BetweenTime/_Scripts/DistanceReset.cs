using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

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
        private ParticleSystem effect;

        private Vector3 _initialPosition; // The object's initial position
        private Quaternion _initialRotation; // The object's initial rotation
        private Vector3 _lastPosition; // The object's last position
        private Rigidbody _rb; // The object's rigidbody

        /// <summary>
        ///     Stores the object's initial position and rotation
        /// </summary>
        private void Start()
        {
            _lastPosition = transform.position;
            _initialPosition = transform.position;
            _initialRotation = transform.rotation;
            _rb = GetComponent<Rigidbody>();
            var grabInteractable = GetComponent<XRGrabInteractable>();
            if (grabInteractable) grabInteractable.selectExited.AddListener(SetLastPosition);
        }

        /// <summary>
        ///     Resets the object's position and rotation if it moves too far from its initial position
        ///     (i.e. 1000m) and instantiates the particle effect; if the objects is within the maximum
        ///     distance but not close enough, applies a force to move it further away to cause the reset
        ///     to occur more quickly
        /// </summary>
        private void FixedUpdate()
        {
            var displacement = _lastPosition - transform.position;
            var distanceSquared = displacement.sqrMagnitude;

            switch (distanceSquared)
            {
                // if object is close enough, do nothing
                case <= MaxDistance * 0.025f: break;
                // if object is within the maximum distance, apply a force to move it further away
                case <= MaxDistance:
                {
                    // Calculate the force to apply to the Rigidbody
                    const float forceFactor = 0.075f;
                    var force = displacement.normalized * (forceFactor * distanceSquared);
                    _rb.AddForce(-force); // Force needs to be applied negatively ¯\_(ツ)_/¯
                    break;
                }
                case > MaxDistance:
                    // Past this point, the object has exceeded the maximum distance, so reset it
                    if (effect) Instantiate(effect, transform.position, Quaternion.identity);
                    _rb.linearVelocity = Vector3.zero;
                    _rb.angularVelocity = Vector3.zero;
                    transform.position = _initialPosition;
                    transform.rotation = _initialRotation;
                    _lastPosition = _initialPosition;
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

        /// <summary>
        ///     Updates the last position of the object when it is deselected
        /// </summary>
        /// <param name="exitArgs">The arguments for the deselection event</param>
        private void SetLastPosition(SelectExitEventArgs exitArgs)
        {
            _lastPosition = exitArgs.interactableObject.transform.position;
        }
    }
}