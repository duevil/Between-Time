using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
        public static readonly ReadOnlyCollection<Color> ColorOrder = new(new List<Color>
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

        // The colors that have been assigned to a candle
        private static readonly List<Color> AssignedColors = new();


        // The index of the candle's color in the candles state
        private int _colorIndex;

        /// <summary>
        ///     The color of the candle; randomly assigned on enable
        /// </summary>
        public Color Color { get; private set; }


        /// <summary>
        ///     Initializes the candle's state to be unlit on start and subscribes to the candles state changes to light the candle
        /// </summary>
        private void Start()
        {
            SetLitState(false);
            if (_colorIndex == -1) return;
            GameController.Instance.candlesState.onChange.AddListener(value => SetLitState(value[_colorIndex]));
        }

        /// <summary>
        ///     Initializes the candle's state to be unlit on start and subscribes to the candles state changes to light the candle
        /// </summary>
        private void OnEnable()
        {
            Color = GetColor();
            GetComponentInChildren<MeshRenderer>().material.color = Color;
            _colorIndex = ColorOrder.IndexOf(Color);
        }

        /// <summary>
        ///     Removes the color from the taken colors list when the candle is destroyed
        /// </summary>
        private void OnDestroy()
        {
            AssignedColors.Remove(Color);
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