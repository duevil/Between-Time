using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

namespace BetweenTime._Scripts
{
    /// <summary>
    ///     Class that represents a timecore in the game
    /// </summary>
    public class Timecore : MonoBehaviour
    {
        // The emission color property ID
        private static readonly int EmissionColor = Shader.PropertyToID("_EmissionColor");

        [Tooltip("The color of the core")] [SerializeField]
        private Color color;

        /// <summary>
        ///     Initializes the core's color on enable; sets each materials color and emission color
        ///     and the light color to the color of the core
        /// </summary>
        private void OnEnable()
        {
            if (color == default) return; // Keep color unchanged if default is selected

            foreach (var material in GetComponentInChildren<MeshRenderer>().materials)
            {
                material.color = color;
                material.SetColor(EmissionColor, color);
            }

            GetComponentInChildren<Light>().color = color;
        }
        
        /// <summary>
        ///     Freezes the timecore in place and disables its interaction
        /// </summary>
        public void Freeze()
        {
            GetComponent<XRGrabInteractable>().enabled = false;
            GetComponent<Rigidbody>().isKinematic = true;
            GetComponent<Oscillator>().enabled = false;
        }
    }
}