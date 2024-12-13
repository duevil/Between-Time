using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using BetweenTime._Scripts.maya;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using Random = UnityEngine.Random;

namespace BetweenTime._Scripts.medieval
{
    /// <summary>
    ///     Class that represents a candle in the game
    /// </summary>
    public class Candle : MonoBehaviour
    {
        /// <summary>
        ///     The awaited colors for the candles in the awaited order
        /// </summary>
        public static readonly ReadOnlyCollection<Color> k_ColorOrder = new(new List<Color>
        {
            Color.magenta,
            Color.blue,
            Color.green,
            Color.cyan
        });

        /// <summary>
        ///     Possible colors for the candle
        /// </summary>
        private static readonly ReadOnlyCollection<Color> Colors = new(new List<Color>
        {
            Color.blue,
            Color.green,
            Color.red,
            Color.cyan,
            Color.magenta,
            Color.yellow,
            /*Purple*/ new(0.5f, 0, 1),
            /*Orange*/ new(1, 0.5f, 0),
            /*Turquoise*/ new(0, 1, 0.5f),
            /*Pink*/ new(1, 0, 0.5f),
            /*Lime*/ new(0.5f, 1, 0),
            /*Azure*/ new(0, 0.5f, 1)
        });

        // Mapping of the candle colors to their corresponding maya symbol
        private static readonly Dictionary<Color, Symbol> Symbols = new()
        {
            { k_ColorOrder[0], Symbol._7 },
            { k_ColorOrder[1], Symbol._8 },
            { k_ColorOrder[2], Symbol._0 },
            { k_ColorOrder[3], Symbol._6 },
            { Colors[2], Symbol._1 },
            { Colors[5], Symbol._2 },
            { Colors[6], Symbol._3 },
            { Colors[7], Symbol._4 },
            { Colors[8], Symbol._5 },
            { Colors[9], Symbol._9 },
            { Colors[10], Symbol._10 },
            { Colors[11], Symbol._11 }
        }; 

        // The colors that have been assigned to a candle
        private static readonly List<Color> AssignedColors = new();

        [Tooltip("The audio clip to play when the candle is ignited")] [SerializeField]
        private AudioClip igniteSound;

        [Tooltip("The audio clip to play when the candle is extinguished")] [SerializeField]
        private AudioClip extinguishSound;


        // The index of the candle's color in the candles state
        private int _colorIndex;

        private bool _isLit;

        /// <summary>
        ///     The color of the candle; randomly assigned on enable
        /// </summary>
        public Color color { get; private set; }

        private bool isLit
        {
            get => _isLit;
            set
            {
                SetLitState(value);
                _isLit = value;
            }
        }

        /// <summary>
        ///     Initializes the candle's state to be unlit on start and subscribes to the candles state changes to light the candle
        /// </summary>
        private void Awake()
        {
            color = GetColor();
            GetComponentInChildren<MeshRenderer>().material.color = color;
            _colorIndex = k_ColorOrder.IndexOf(color);
            GetComponentInChildren<SpriteRenderer>().sprite = Symbols[color].GetSprite();
        }


        /// <summary>
        ///     Initializes the candle's state to be unlit on start and subscribes to the candles state changes to light the candle
        /// </summary>
        private void Start()
        {
            isLit = false;
            if (_colorIndex == -1) return;
            GameController.instance.candlesState.onChange.AddListener(value => isLit = value[_colorIndex]);
        }

        /// <summary>
        ///     Removes the color from the taken colors list when the candle is destroyed
        /// </summary>
        private void OnDestroy()
        {
            AssignedColors.Remove(color);
        }

        /// <summary>
        ///     Toggles the candle's light and particle effects
        /// </summary>
        private void SetLitState(bool value)
        {
            GetComponentInChildren<Light>().enabled = value;
            foreach (var ps in GetComponentsInChildren<ParticleSystem>())
                if (value) ps.Play();
                else ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            if (value == isLit) return;
            AudioSource.PlayClipAtPoint(value ? igniteSound : extinguishSound, transform.position);
            if (value) GetComponent<AudioSource>().Play();
            else GetComponent<AudioSource>().Stop();
        }

        /// <summary>
        ///     Freezes the candle in place and disables its interaction
        /// </summary>
        public void Freeze()
        {
            GetComponent<XRGrabInteractable>().enabled = false;
            GetComponent<Rigidbody>().isKinematic = true;
            GetComponent<Oscillator>().enabled = false;
        }


        /// <summary>
        ///     Gets a random color that has not been assigned to a candle
        /// </summary>
        /// <returns>The color of the candle</returns>
        /// <exception cref="InvalidOperationException">Thrown when no more colors are available</exception>
        private static Color GetColor()
        {
            var availableColors = Colors.Except(AssignedColors).ToList();
            if (availableColors.Count == 0) throw new InvalidOperationException("No more colors available");
            var color = availableColors[Random.Range(0, availableColors.Count)];
            AssignedColors.Add(color);
            return color;
        }
    }
}