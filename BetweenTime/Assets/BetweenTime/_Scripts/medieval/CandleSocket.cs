using System.Collections.Generic;
using System.Linq;
using BetweenTime._Scripts.@base;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

namespace BetweenTime._Scripts.medieval
{
    public class CandleSocket : MonoBehaviour
    {
        private const ushort Timecode = 0x4001;

        private static readonly List<Candle.CandleColor> Colors = new()
        {
            Candle.CandleColor.Blue,
            Candle.CandleColor.Green,
            Candle.CandleColor.Magenta,
            Candle.CandleColor.Cyan,
        };

        private static readonly List<Candle.CandleColor> Placed = new();

        private void Start()
        {
            var socket = GetComponent<XRSocketInteractor>();
            socket.socketActive = false;
            GameController.Instance.mainState.onChange.AddListener(value =>
                SetSocketState(socket, value, GameController.Instance.timecodeState.Value));
            GameController.Instance.timecodeState.onChange.AddListener(value =>
                SetSocketState(socket, GameController.Instance.mainState.Value, value));
        }

        public void OnSelectEntered(SelectEnterEventArgs args)
        {
            var candle = args.interactableObject.ConvertTo<Candle>();
            candle.isPlaced = true;
            Placed.Add(candle.color);
            GetComponent<Collider>().isTrigger = false;
            if (Colors.All(color => Placed.Contains(color)))
            {
                GameController.Instance.mainState.Value = MainState.CandlesPlaced;
            }
        }

        public void OnSelectExited(SelectExitEventArgs args)
        {
            var candle = args.interactableObject.ConvertTo<Candle>();
            candle.isPlaced = false;
            Placed.Remove(candle.color);
            GetComponent<Collider>().isTrigger = true;
        }

        private static void SetSocketState(XRSocketInteractor socket, MainState mainState, ushort timecode)
        {
            socket.socketActive = mainState == MainState.InputFieldSolved && timecode == Timecode;
        }
    }
}