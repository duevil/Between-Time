using UnityEngine;

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
        ///     Initializes the core's color on enable; sets each materials color and emission color to the color of the core
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
    }
}