using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

namespace BetweenTime._Scripts
{
    /// <summary>
    ///     Class that represents a timecore in the game
    /// </summary>
    [RequireComponent(typeof(XRGrabInteractable))]
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Oscillator))]
    public class Timecore : MonoBehaviour
    {
        // The emission color property ID
        private static readonly int EmissionColor = Shader.PropertyToID("_EmissionColor");

        [Tooltip("The color of the core")] [SerializeField]
        private Color color;

        private Light _childLight;

        private Rigidbody _getComponent;
        private Oscillator _oscillator;

        private XRGrabInteractable _xrGrabInteractable;

        /// <summary>
        ///     Initializes the core's color on enable; sets each materials color and emission color
        ///     and the light color to the color of the core
        /// </summary>
        private void Awake()
        {
            if (color == default) return; // Keep color unchanged if default is selected

            foreach (var material in GetComponentInChildren<MeshRenderer>().materials)
            {
                material.color = color;
                material.SetColor(EmissionColor, color);
            }

            _childLight = GetComponentInChildren<Light>();
            if (_childLight != null) _childLight.color = color;

            _xrGrabInteractable = GetComponent<XRGrabInteractable>();
            _getComponent = GetComponent<Rigidbody>();
            _oscillator = GetComponent<Oscillator>();
        }

        /// <summary>
        ///     Sets the freeze state of the timecore;
        ///     if frozen, the timecore is locked in place and interaction is disabled
        /// </summary>
        /// <param name="frozen">Whether the timecore should be frozen</param>
        /// <param name="disableLight">Whether the light should be disabled when frozen</param>
        public void SetFreeze(bool frozen, bool disableLight = true)
        {
            _xrGrabInteractable.enabled = !frozen;
            _getComponent.isKinematic = frozen;
            _oscillator.enabled = !frozen;
            if (!_childLight) return;
            _childLight.enabled = disableLight ? !frozen : _childLight.enabled;
        }
    }
}