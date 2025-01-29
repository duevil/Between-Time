using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace BetweenTime._Scripts
{
    /// <summary>
    ///     Class implementing a warp effect that obscures the camera and applies a vignette effect to the screen
    ///     and warps the player back to the starting position if they move too far
    /// </summary>
    [RequireComponent(typeof(Animator))]
    public class Warp : MonoBehaviour
    {
        private static readonly int WarpID = Animator.StringToHash("Warp");

        [Tooltip("The maximum distance the player can move from the starting position")] [SerializeField]
        private float maxDistance;

        [Tooltip("The post-processing volume to apply the vignette to")] [SerializeField]
        private Volume postProcessVolume;

        [Tooltip("The object that blocks the camera")] [SerializeField]
        private GameObject cameraBlocker;

        private Animator _animator; // The animator component
        private bool _isHalfway; // Indicates if the warp is halfway done


        private Material _material; // The material of the camera blocker
        private Vector3 _startPosition; // The player's starting position
        private bool _toFar; // Indicates if the player moved too far
        private Vignette _vignette; // The vignette effect to apply


        /// <summary>
        ///     The value of the warp effect; gets the intensity of the vignette effect
        ///     and sets the vignette intensity and the alpha value of the camera blocker
        /// </summary>
        public float value
        {
            get => _vignette.intensity.value;
            set
            {
                _vignette.intensity.value = value;
                var color = _material.color;
                _material.color = new Color(color.r, color.g, color.b, value);
            }
        }

        /// <summary>
        ///     The current instance of the Warp; finds the player object and gets the warp component
        /// </summary>
        public static Warp instance => GameObject.FindWithTag("Player").GetComponent<Warp>();


        /// <summary>
        ///     Initializes the warp effect; gets the vignette effect and the camera blocker's material
        /// </summary>
        private void Start()
        {
            _vignette = postProcessVolume.profile.TryGet<Vignette>(out var vignette) ? vignette : null;
            _material = cameraBlocker.GetComponent<Renderer>().material;
            _startPosition = transform.position;
            _animator = GetComponent<Animator>();
            value = 1f;
            _animator.Play("Warp", -1, 0.5f);
        }

        /// <summary>
        ///     Checks if the player moved too far from the starting position and triggers the warp effect
        /// </summary>
        private void FixedUpdate()
        {
            if (_toFar || Mathf.Abs(transform.position.y - _startPosition.y) < maxDistance) return;
            TryGetComponent(out AudioSource audioSource);
            audioSource?.Play();
            Trigger();
            _toFar = true;
        }

        /// <summary>
        ///     Triggers the warp effect
        /// </summary>
        public void Trigger()
        {
            _animator.SetTrigger(WarpID);
        }

        /// <summary>
        ///     Marks the warp as halfway done to reset the player's position the first time the warp
        ///     exceeds its halfway point
        /// </summary>
        /// <param name="isHalfway">Whether the warp is halfway done</param>
        public void MarkHalfway(bool isHalfway)
        {
            if (!_isHalfway && isHalfway)
            {
                transform.position = _startPosition;
                _toFar = false;
            }

            _isHalfway = isHalfway;
        }
    }
}