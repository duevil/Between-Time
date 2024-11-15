using System;
using System.Text;
using BetweenTime._Scripts.@base;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using uPLibrary.Networking.M2Mqtt;

namespace BetweenTime._Scripts
{
    /// <summary>
    ///     Singleton class that controls the game states and timer and handles MQTT communication
    /// </summary>
    public class GameController : MonoBehaviour
    {
        private static GameController _instance; // Underlying field for the singleton instance


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

        private MqttClient _client; // The MQTT client to use for communication
        private float _intervalTimer; // For publishing timer value every second


        /// <summary>
        ///     The singleton instance of the GameController
        /// </summary>
        public static GameController Instance
        {
            get
            {
                if (_instance == null) Debug.LogWarning("GameController is null");
                return _instance;
            }
        }

        /// <summary>
        ///     Whether the game is currently running, i.e. not in the Idle, GameWon or GameLost state
        /// </summary>
        public bool Running => mainState.Value is not (MainState.Idle or MainState.GameWon or MainState.GameLost);

        /// <summary>
        ///     The remaining time on the game's timer (in seconds)
        /// </summary>
        public float Timer
        {
            get => timer;
            private set
            {
                timer = Mathf.Max(0, value);
                timerEvent.Invoke(timer);
            }
        }


        /// <summary>
        ///     Initializes the GameController singleton instance, sets up state change listeners and MQTT communication
        ///     and initializes states values
        /// </summary>
        private void Awake()
        {
            // Singleton pattern to ensure only one instance of GameController exists
            if (Instance != null)
            {
                if (Instance == this) return;
                Debug.LogWarning("GameController already exists, destroying this instance");
                Destroy(gameObject); // An instance already exists, destroy this one
                return;
            }

            // No instance exists, set this as the instance and ensure it persists between scenes
            _instance = this;
            DontDestroyOnLoad(gameObject);

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
                    state.PublishValue(_client);
                }

                _intervalTimer = 1; // Force publishing the timer value when reset
                Timer = timerDuration; // Reset the timer
            });

            // Connect to the MQTT broker and set up the MQTT communication for all states
            try
            {
                _client = new MqttClient(mqttHost);
                _client.Connect("unity-" + Guid.NewGuid());
                // Set up MQTT communication for all states
                foreach (var state in states)
                {
                    // Reset to and publish the initial value to the topic to ensure it's the latest retained value
                    state.Reset();
                    state.PublishValue(_client);
                    state.SetupMqtt(_client);
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Error setting up MQTT communication: {e}");
            }

            Timer = timerDuration; // Set the timer to the initial duration
            timerEvent.AddListener(value =>
            {
                if (_intervalTimer < 1) return;
                var message = Encoding.UTF8.GetBytes(value.ToString("0."));
                _client.Publish(timerTopic, message, 0, false);
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
    }
}