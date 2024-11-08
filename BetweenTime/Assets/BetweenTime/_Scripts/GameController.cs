using System;
using BetweenTime._Scripts.@base;
using UnityEditor;
using UnityEngine;
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

        [Tooltip("Duration of the game's timer in seconds")] [SerializeField]
        private int timerDuration = 300;

        [Tooltip("The current time on the game's timer")] [ReadOnly] [SerializeField]
        private float timer;

        [Tooltip("The game's main state machine's current state")]
        public MutableState<MainState, MainStateParser> mainState = new();

        [Tooltip("The currently set timecode")]
        public State<short, BasicParser<short>> timecodeState = new();

        [Tooltip("The current state of the candles")]
        public State<Candles, Candles.Parser> candlesState = new();

        [Tooltip("The current position of the player in the maze")]
        public State<MazePosition, MazePosition.Parser> mazePositionsState = new();

        [Tooltip("The number of items scanned by the player")]
        public State<byte, BasicParser<byte>> scannedItemsState = new();


        private MqttClient _client; // The MQTT client to use for communication


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
            private set => timer = value;
        }


        private void Awake()
        {
            // Singleton pattern to ensure only one instance of GameController exists
            if (Instance == null)
            {
                // No instance exists, set this as the instance and ensure it persists between scenes
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else if (Instance != this)
            {
                Debug.LogWarning("GameController already exists, destroying this instance");
                Destroy(gameObject); // An instance already exists, destroy this one
            }
        }

        /// <see cref="MonoBehaviour" />
        /// Start method
        private void Start()
        {
            // Listen to main state changes to reset the timer when the game starts
            mainState.onChange.AddListener(value =>
            {
                Debug.Log($"Main state changed to {value}");
                if (value == MainState.Idle) Timer = timerDuration; // 5 minutes
            });

            // Connect to the MQTT broker and set up the MQTT communication for all states
            try
            {
                _client = new MqttClient(mqttHost);
                _client.Connect(Guid.NewGuid().ToString());
                mainState.SetupMqtt(_client);
                timecodeState.SetupMqtt(_client);
                candlesState.SetupMqtt(_client);
                mazePositionsState.SetupMqtt(_client);
                scannedItemsState.SetupMqtt(_client);
            }
            catch (Exception e)
            {
                Debug.LogError($"Error setting up MQTT communication: {e.Message}");
            }

            // Set the initial state of the game to Idle
            mainState.Value = MainState.Idle;
            // Set the initial timer value
            Timer = timerDuration;
        }

        /// <see cref="MonoBehaviour" />
        /// Update method
        private void Update()
        {
            if (!Running) return;

            Timer -= Time.deltaTime; // Count down the timer

            if (Timer > 0) return;
            // Timer has run out while the game was not won, so the game is lost
            Debug.Log("Game over");
            mainState.Value = MainState.GameLost;
        }


        /// <summary>
        ///     Helper method to set the main state from debug console commands
        /// </summary>
        /// <param name="value">The new main state's value; must be parsable to an integer or the enum name/// </param>
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
        /// <param name="value">The new timer value as a string; must be parsable to an integer/// </param>
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