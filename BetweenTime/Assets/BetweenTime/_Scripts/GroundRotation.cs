using UnityEngine;

namespace BetweenTime._Scripts
{
    /// <summary>
    ///     Class for randomly rotating the ground to make it look more natural
    /// </summary>
    public class GroundRotation : MonoBehaviour
    {
        /// <summary>
        ///     Randomly rotate the ground object on start
        /// </summary>
        private void Start()
        {
            transform.Rotate(0, Random.Range(0, 360), 0);
        }
    }
}