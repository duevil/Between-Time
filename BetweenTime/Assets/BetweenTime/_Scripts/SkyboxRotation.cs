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

        // Copied instance of the RenderSettings' skybox to apply the rotation override to
        private Material _skybox;

        /// <summary>
        ///     Copies the current skybox from the RenderSettings,
        ///     stores the copy and sets it as the new RenderSettings' skybox
        /// </summary>
        private void Awake()
        {
            var skybox = RenderSettings.skybox;
            _skybox = new Material(skybox);
            RenderSettings.skybox = _skybox;
        }

        /// <summary>
        ///     Updates the skybox rotation based on the current time
        /// </summary>
        private void Update()
        {
            speed = speedFactor == 0 ? 0f : 0.01f * Mathf.Pow(10f, speedFactor - 1);
            _skybox.SetFloat(Rotation, Time.time * speed);
        }
    }
}