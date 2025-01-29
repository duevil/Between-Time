using BetweenTime._Scripts.@base;
using UnityEngine;

namespace BetweenTime._Scripts
{
    /// <summary>
    ///     Controls the music player in the game
    /// </summary>
    public class MusicPlayer : MonoBehaviour
    {
        [Tooltip("The music to play when the game is won")] [SerializeField]
        private AudioClip victoryMusic;

        private AudioSource _audioSource; // The audio source component of the music player

        /// <summary>
        ///     Initializes the audio source component on enable
        /// </summary>
        private void Start()
        {
            _audioSource = GetComponent<AudioSource>();
            _audioSource.clip.LoadAudioData();
        }

        /// <summary>
        ///     Adds a listener to the main state change event;
        ///     plays the normal music when the game enters any running state,
        ///     stops the music when the game is idle
        ///     and plays the victory music when the game is won
        /// </summary>
        /// <param name="value"></param>
        public void MainStateLister(MainState value)
        {
            switch (value)
            {
                case MainState.Idle:
                    _audioSource.Stop();
                    break;
                case MainState.GameLost:
                    // do nothing
                    break;
                case MainState.GameWon:
                    _audioSource.clip = victoryMusic;
                    _audioSource.Play();
                    break;
                default:
                    if (!_audioSource.isPlaying) _audioSource.Play();
                    break;
            }
        }
    }
}