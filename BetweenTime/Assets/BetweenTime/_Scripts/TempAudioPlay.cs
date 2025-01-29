using UnityEngine;

namespace BetweenTime._Scripts
{
    /// <summary>
    ///     Temporary class for playing an audio clip at the position of the object this script is attached to
    /// </summary>
    public class TempAudioPlay : MonoBehaviour
    {
        [Tooltip("The audio clip to play")] [SerializeField]
        private AudioClip audioClip;

        /// <summary>
        ///     Plays the audio clip at the position of the object this script is attached to
        /// </summary>
        private void Awake()
        {
            AudioSource.PlayClipAtPoint(audioClip, transform.position);
        }
    }
}