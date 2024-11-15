using System;
using System.Collections.Concurrent;
using UnityEngine;

namespace BetweenTime._Scripts
{
    /// <summary>
    ///     Class that allows for invoking actions on the main thread from other threads
    /// </summary>
    public class MainThreadInvoker : MonoBehaviour
    {
        // Queue of actions to be invoked on the main thread
        private static readonly ConcurrentQueue<Action> Actions = new();

        /// <summary>
        ///     Checks for enqueued actions and invokes them
        /// </summary>
        private void Update()
        {
            while (Actions.TryDequeue(out var action)) action?.Invoke();
        }

        /// <summary>
        ///     Enqueues an action to be invoked on the main thread
        /// </summary>
        /// <param name="action">The action to invoke</param>
        public static void Enqueue(Action action)
        {
            Actions.Enqueue(action);
        }
    }
}