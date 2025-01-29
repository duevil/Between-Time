using System.Collections.Generic;
using System.Linq;
using BetweenTime._Scripts.@base;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

namespace BetweenTime._Scripts.medieval
{
    /// <summary>
    ///     Class for handling a placement of a candle in a socket
    /// </summary>
    public class CandleSocket : MonoBehaviour
    {
        private const ushort Timecode = 0x4001; // The timecode to sync for this puzzle
        private static readonly List<Candle> Placed = new(); // The list of candles placed in the socket


        /// <summary>
        ///     Initializes the socket's state to be inactive on start and subscribes to the game state changes
        ///     to enable the socket when the input field is solved and the timecode is correct
        /// </summary>
        private void Start()
        {
            var socket = GetComponent<XRSocketInteractor>();
            socket.socketActive = false;
            GameController.instance.mainState.onChange.AddListener(value =>
                SetSocketState(socket, value, GameController.instance.timecodeState.value));
            GameController.instance.timecodeState.onChange.AddListener(value =>
                SetSocketState(socket, GameController.instance.mainState.value, value));
        }

        /// <summary>
        ///     XR Interaction Toolkit event handler for when an object is selected; adds the candle to the placed list,
        ///     disables the socket collider trigger to avoid ugly highlighting, and if the correct candles are placed,
        ///     sets the game state to CandlesPlaced and disables grabbing the placed candles
        /// </summary>
        /// <param name="args">The event arguments for the select enter event</param>
        public void OnSelectEntered(SelectEnterEventArgs args)
        {
            var candle = args.interactableObject.ConvertTo<Candle>();
            Placed.Add(candle);
            GetComponent<Collider>().isTrigger = false;

            // Check if all the correct candles are placed
            if (!Candle.k_ColorOrder.All(color => Placed.Exists(c => c.color == color))) return;
            Placed.ToList().ForEach(c => c.Freeze()); // Freeze all placed candles
            GameController.instance.mainState.value = MainState.CandlesPlaced;
        }

        /// <summary>
        ///     XR Interaction Toolkit event handler for when an object is deselected; removes the candle from the placed list
        ///     and enables the socket collider to allow for placing another candle
        /// </summary>
        /// <param name="args">The event arguments for the select exit event</param>
        public void OnSelectExited(SelectExitEventArgs args)
        {
            var candle = args.interactableObject.ConvertTo<Candle>();
            Placed.Remove(candle);
            GetComponent<Collider>().isTrigger = true;
        }


        /// <summary>
        ///     Sets the socket's state to active if the main state is InputFieldSolved and the timecode is correct
        /// </summary>
        private static void SetSocketState(XRSocketInteractor socket, MainState mainState, ushort timecode)
        {
            socket.socketActive = mainState == MainState.InputFieldSolved && timecode == Timecode;
        }
    }
}