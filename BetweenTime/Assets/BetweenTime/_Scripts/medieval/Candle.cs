using BetweenTime._Scripts.@base;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

namespace BetweenTime._Scripts.medieval
{
    public class Candle : MonoBehaviour
    {
        public enum CandleColor
        {
            Red,
            Green,
            Blue,
            Cyan,
            Magenta,
            Yellow,
        }

        public CandleColor color;
        public bool isPlaced;

        public void Start()
        {
            GameController.Instance.mainState.onChange.AddListener(value =>
            {
                if (!isPlaced || value != MainState.CandlesPlaced) return;
                GetComponent<XRGrabInteractable>().enabled = false;
            });
        }
    }
}