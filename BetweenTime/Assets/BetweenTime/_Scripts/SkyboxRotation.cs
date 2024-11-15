using UnityEngine;

namespace BetweenTime._Scripts
{
    /// <summary>
    ///     Rotates the skybox to give the appearance of time passing
    /// </summary>
    public class SkyboxRotation : MonoBehaviour
    {
        // Shader property ID for the rotation value
        private static readonly int Rotation = Shader.PropertyToID("_Rotation");

        [Tooltip("The factor for the skybox rotation speed")] [SerializeField] [Range(0, 6)]
        private int speedFactor;

        [Tooltip("The speed at which the skybox rotates")] [SerializeField] [ReadOnly]
        private float speed;

        /// <summary>
        ///     Updates the skybox rotation based on the current time
        /// </summary>
        private void Update()
        {
            speed = speedFactor == 0 ? 0f : 0.01f * Mathf.Pow(10f, speedFactor - 1);
            RenderSettings.skybox.SetFloat(Rotation, Time.time * speed);
        }
    }
}