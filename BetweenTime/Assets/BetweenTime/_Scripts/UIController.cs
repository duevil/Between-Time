using System;
using BetweenTime._Scripts.@base;
using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace BetweenTime._Scripts
{
    /// <summary>
    ///     Controller for the game's UI elements
    /// </summary>
    public class UIController : MonoBehaviour
    {
        [Tooltip("The text element for the timer")] [SerializeField]
        private TMP_Text timerText;

        [Tooltip("The text element for the timecode")] [SerializeField]
        private TMP_Text timecodeText;

        [Tooltip("The text element for the info text")] [SerializeField]
        private TMP_Text infoText;

        /// <summary>
        ///     Initializes the UI elements with the initial values
        /// </summary>
        private void Start()
        {
            UpdateInfoFromMainState(MainState.Idle);
            UpdateTimecodeText(0);
            // timer will be updated by GameController event
        }

        /// <summary>
        ///     Updates the info text based on the current main state
        /// </summary>
        /// <param name="value">The current main state</param>
        public void UpdateInfoFromMainState(MainState value)
        {
            // TODO: Add info text for the other main states

            infoText.text = value switch
            {
                MainState.Idle => "Press main trigger to start the game",
                MainState.GameWon => "Congratulations You have saved the timelines!",
                MainState.GameLost => "You have run out of time! All is lost...",
                _ => ""
            };
        }

        /// <summary>
        ///     Updates the timecode text with the current value as a hexadecimal string
        /// </summary>
        /// <param name="value">The current timecode value</param>
        public void UpdateTimecodeText(ushort value)
        {
            timecodeText.text = value.ToString("X4");
        }

        /// <summary>
        ///     Updates the timer text with the current value as a time string
        /// </summary>
        /// <param name="value">The current timer value</param>
        public void UpdateTimerText(float value)
        {
            timerText.text = TimeSpan.FromSeconds(value).ToString(@"m\:ss\.ff");
        }

        /// <summary>
        ///     Event handler for the main trigger being activated; starts the game if the game is idle,
        ///     restarts the game if the game is won or lost
        /// </summary>
        /// <param name="_">Activate event arguments; not used</param>
        public static void OnActivate(ActivateEventArgs _)
        {
            var gc = GameController.instance;
            switch (gc.mainState.Value)
            {
                case MainState.Idle:
                    gc.mainState.Value = MainState.Started;
                    break;
                case MainState.GameWon or MainState.GameLost:
                    gc.RestartGame();
                    break;
            }
        }
    }
}