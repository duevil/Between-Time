using System;
using System.Collections;
using System.Globalization;
using System.Text;
using BetweenTime._Scripts.@base;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using uPLibrary.Networking.M2Mqtt;

namespace BetweenTime._Scripts
{
    /// <summary>
    ///     Controller for the game states and timer and handles MQTT communication
    /// </summary>
    public class GameController : MonoBehaviour
    {
        [Tooltip("MQTT host address")] [SerializeField]
        private string mqttHost;

        [Tooltip("Topic to publishing current timer value to")] [SerializeField]
        private string timerTopic;

        [Tooltip("Duration of the game's timer in seconds")] [SerializeField]
        private int timerDuration = 300;

        [Tooltip("The current time on the game's timer")] [ReadOnly] [SerializeField]
        private float timer;

        [Tooltip("The game's main state machine's current state")]
        public State<MainState, MainStateParser> mainState = new();

        [Tooltip("The currently set timecode")]
        public State<ushort, BasicParser<ushort>> timecodeState = new();

        [Tooltip("The current state of the candles")]
        public State<Candles, Candles.Parser> candlesState = new();

        [Tooltip("The current position of the player in the maze")]
        public State<MazePosition, MazePosition.Parser> mazePositionsState = new();

        [Tooltip("The number of items scanned by the player")]
        public State<byte, BasicParser<byte>> scannedItemsState = new();

        [Tooltip("Event that is invoked when the timer value changes")] [SerializeField]
        private UnityEvent<float> timerEvent = new(); // Event for timer value changes

        private float _intervalTimer; // For publishing timer value every second


        /// <summary>
        ///     The current instance of the GameController
        /// </summary>
        public static GameController Instance => GameObject.FindWithTag("GameController").ConvertTo<GameController>();

        /// <summary>
        ///     Whether the game is currently running, i.e. not in the Idle, GameWon or GameLost state
        /// </summary>
        public bool Running => mainState.Value is not (MainState.Idle or MainState.GameWon or MainState.GameLost);

        /// <summary>
        ///     The remaining time on the game's timer (in seconds)
        /// </summary>
        private float Timer
        {
            get => timer;
            set
            {
                timer = Mathf.Max(0, value);
                timerEvent.Invoke(timer);
            }
        }


        /// <summary>
        ///     Initializes state change listeners, MQTT communication and states values
        /// </summary>
        private void Awake()
        {
            IState[] states = { mainState, timecodeState, candlesState, mazePositionsState, scannedItemsState };

            // Add a listener to the main state to handle state changes
            mainState.onChange.AddListener(value =>
            {
                Debug.Log($"Main state changed to {value}");
                if (value != MainState.Idle) return;
                foreach (var state in states)
                {
                    if (state.Equals(mainState)) continue; // Skip the main state
                    state.Reset(); // Reset all states to their initial values
                    state.PublishValue(Mqtt.Client());
                }

                _intervalTimer = 1; // Force publishing the timer value when reset
                Timer = timerDuration + 0.999f; // Reset the timer
            });

            // Connect to the MQTT broker and set up the MQTT communication for all states
            try
            {
                var client = Mqtt.Client(mqttHost); // Set up MQTT communication for all states
                // Set up MQTT communication for all states
                foreach (var state in states)
                {
                    // Reset to and publish the initial value to the topic to ensure it's the latest retained value
                    state.Reset();
                    state.PublishValue(client);
                    state.SetupMqtt(client);
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Error setting up MQTT communication: {e}");
            }

            Timer = timerDuration + 0.999f; // Set the timer to the initial duration
            timerEvent.AddListener(value =>
            {
                if (_intervalTimer < 1) return;
                var message = Encoding.UTF8.GetBytes(value.ToString("0."));
                Mqtt.Client().Publish(timerTopic, message, 0, false);
                _intervalTimer = 0;
            }); // Add listener to timer event to publish the timer value every second
        }

        /// <summary>
        ///     Updates the timer, publishes the current time value to the MQTT broker every second
        ///     and checks if the game is lost
        /// </summary>
        private void Update()
        {
            if (!Running) return;
            Timer -= Time.deltaTime; // Count down the timer
            if (_intervalTimer < 1) _intervalTimer += Time.deltaTime;
            if (Timer > 0) return;
            // Timer has run out while the game was not won, so the game is lost
            Debug.Log("Game over");
            mainState.Value = MainState.GameLost;
        }


        /// <summary>
        ///     Helper method to set the main state from debug console commands
        /// </summary>
        /// <param name="value">The new main state's value; must be parsable to an integer or the enum name</param>
        public void SetMainState(string value)
        {
            if (Enum.TryParse<MainState>(value, true, out var enumValue))
                mainState.Value = enumValue;
            else
                Debug.LogWarning($"Could not parse '{value}' to MainState");
        }

        /// <summary>
        ///     Helper method to set the timecode from debug console commands
        /// </summary>
        /// <param name="value">
        ///     The new timecode value as a string; must be parsable to an hexadecimal number
        /// </param>
        public void SetTimecode(string value)
        {
            if (ushort.TryParse(value, NumberStyles.HexNumber, null, out var ushortValue))
                timecodeState.Value = ushortValue;
            else
                Debug.LogWarning($"Could not parse '{value}' to ushort");
        }

        /// <summary>
        ///     Helper method to set the timer from debug console commands
        /// </summary>
        /// <param name="value">The new timer value as a string; must be parsable to an integer</param>
        public void SetTimer(string value)
        {
            if (int.TryParse(value, out var intValue))
            {
                Timer = intValue;
                if (!Running) mainState.Value = MainState.Started;
            }
            else
            {
                Debug.LogWarning($"Could not parse '{value}' to int");
            }
        }

        /// <summary>
        ///     Helper to quit the game from debug console commands
        /// </summary>
        public static void QuitGame()
        {
            Debug.Log("Quitting game");
#if UNITY_EDITOR
            // Application.Quit() does not work in the editor so to end the game
            // UnityEditor.EditorApplication.isPlaying needs to be set to false
            EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        /// <summary>
        ///     Restarts the game by reloading the current scene and resetting the main state; the loading is animated
        /// </summary>
        public void RestartGame()
        {
            Debug.Log("Restarting game");
            StartCoroutine(Coroutine());
        }

        /// <summary>
        ///     Coroutine to restart the game; waits for the warp animation to halfway finish before reloading the scene
        /// </summary>
        /// <returns>Coroutine enumerator</returns>
        private IEnumerator Coroutine()
        {
            Warp.Instance.Trigger();
            yield return new WaitUntil(() => Mathf.Approximately(Warp.Instance.Value, 1f));
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            mainState.Value = MainState.Idle;
        }


        /// <summary>
        ///     Singleton wrapper for the MQTT client to ensure only one client is used
        ///     and connection can persist between scenes
        /// </summary>
        private static class Mqtt
        {
            private static MqttClient _client; // The MQTT client instance

            /// <summary>
            ///     Returns the MQTT client instance, creating a new one if it doesn't exist yet
            /// </summary>
            /// <param name="mqttHost">
            ///     The MQTT host address to connect to;
            ///     if not provided, the existing client is returned
            /// </param>
            /// <returns>The MQTT client instance</returns>
            public static MqttClient Client(string mqttHost = default)
            {
                if (mqttHost == default || _client != null) return _client;
                _client = new MqttClient(mqttHost);
                _client.Connect("unity-" + Guid.NewGuid());
                return _client;
            }
        }
    }
}