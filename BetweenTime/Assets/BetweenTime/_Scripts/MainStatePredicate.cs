using BetweenTime._Scripts.@base;
using UnityEngine;
using UnityEngine.Events;

namespace BetweenTime._Scripts
{
    /// <summary>
    ///     Helper class for invoking an event when the main state matches a set value
    /// </summary>
    public class MainStatePredicate : MonoBehaviour
    {
        [Tooltip("The main state to check for")] [SerializeField]
        private MainState state;

        [Tooltip("The event to invoke when the main state is accepted")] [SerializeField]
        private UnityEvent<MainState> onStateAccepted = new();

        /// <summary>
        ///     Adds a listener to the main state change event;
        ///     invokes the event if the main state matches the predicate
        /// </summary>
        /// <param name="value">The main state to check for</param>
        public void Listener(MainState value)
        {
            if (value == state) onStateAccepted.Invoke(value);
        }
    }
}