using UnityEngine;

namespace BetweenTime._Scripts
{
    public class TempAudioPlay : MonoBehaviour
    {
        [Tooltip("The audio clip to play")] [SerializeField]
        private AudioClip audioClip;

        private void Awake()
        {
            AudioSource.PlayClipAtPoint(audioClip, transform.position);
        }
    }
}