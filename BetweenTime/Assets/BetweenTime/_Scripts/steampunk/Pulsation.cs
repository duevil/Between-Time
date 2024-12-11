using UnityEngine;

namespace BetweenTime._Scripts.steampunk
{
    /// <summary>
    ///     Class for pulsating an object
    /// </summary>
    public class Pulsation : MonoBehaviour
    {
        private const float ScaleFactor = 0.0025f; // The scale factor for the pulsation
        private const float Frequency = 10f; // The frequency of the pulsation
        private Vector3 _initialScale; // The object's initial scale

        /// <summary>
        ///     Stores the object's initial scale
        /// </summary>
        private void Start()
        {
            _initialScale = transform.localScale;
        }

        /// <summary>
        ///     Pulsates the object by scaling it along its x- and y-axes
        /// </summary>
        private void Update()
        {
            // Slightly scale the object along its x- and y-axes
            var scale = ScaleFactor * Mathf.Sin(Frequency * Time.time);
            transform.localScale = _initialScale + new Vector3(scale, scale, 0);
        }
    }
}