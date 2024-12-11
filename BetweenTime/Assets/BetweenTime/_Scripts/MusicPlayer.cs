using BetweenTime._Scripts.@base;
using UnityEngine;

namespace BetweenTime._Scripts
{
    public class MusicPlayer : MonoBehaviour
    {
        private AudioSource _audioSource;

        private void Start()
        {
            _audioSource = GetComponent<AudioSource>();
            _audioSource.clip.LoadAudioData();
        }

        public void MainStateLister(MainState value)
        {
            switch (value)
            {
                case MainState.Idle or MainState.GameWon:
                    _audioSource.Stop();
                    break;
                case MainState.GameLost:
                    // do nothing
                    break;
                default:
                    if (!_audioSource.isPlaying) _audioSource.Play();
                    break;
            }
        }
    }
}