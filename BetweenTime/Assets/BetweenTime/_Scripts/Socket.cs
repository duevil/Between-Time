using System.Collections.Generic;
using System.Linq;
using BetweenTime._Scripts.@base;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace BetweenTime._Scripts
{
    /// <summary>
    ///     Class for handling a placement of a timecore in a socket
    /// </summary>
    public class Socket : MonoBehaviour
    {
        private static readonly HashSet<Timecore> PlacedCores = new(); // The timecores that have been placed
        private static readonly HashSet<Timecore> AllCores = new(); // All timecores to be placed

        [Tooltip("The timecore that this socket is connected to")] [SerializeField]
        private Timecore timecore;

        private bool _isPlaced; // Whether the timecore has been placed

        
        /// <summary>
        ///     Registers the timecore to the list of all timecores on enable
        /// </summary>
        private void OnEnable()
        {
            AllCores.Add(timecore);
        }

        /// <summary>
        ///     Unregisters the timecore from the list of all timecores and all placed timecores on destroy
        /// </summary>
        private void OnDestroy()
        {
            AllCores.Remove(timecore);
            PlacedCores.Remove(timecore);
        }

        
        /// <summary>
        ///     XR Interaction Toolkit event handler for when an object is selected;
        ///     adds the timecore to the placed list
        ///     and if all timecores are placed in the correct sockets, sets the game state to GameWon
        /// </summary>
        /// <param name="args">The event arguments for the select enter event</param>
        public void OnEvent(SelectEnterEventArgs args)
        {
            if (args.interactableObject.ConvertTo<Timecore>() != timecore) return;
            PlacedCores.Add(timecore);

            if (!PlacedCores.SetEquals(AllCores)) return;
            GameController.Instance.mainState.Value = MainState.GameWon;
            foreach (var placedCore in PlacedCores.ToList()) placedCore.Freeze();
        }

        /// <summary>
        ///     XR Interaction Toolkit event handler for when an object is deselected;
        ///     removes the timecore from the placed list
        /// </summary>
        /// <param name="args">The event arguments for the select exit event</param>
        public void OnEvent(SelectExitEventArgs args)
        {
            if (args.interactableObject.ConvertTo<Timecore>() != timecore) return;
            PlacedCores.Remove(timecore);
        }
    }
}