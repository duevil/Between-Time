using BetweenTime._Scripts.@base;
using UnityEngine;

namespace BetweenTime._Scripts
{
    /// <summary>
    /// Start the game
    /// </summary>
    public class StartGame : MonoBehaviour
    {
        /// <summary>
        /// Starts the game if not running
        /// </summary>
        public void Invoke()
        {
            if (GameController.Instance.Running) return;
            GameController.Instance.mainState.Value = MainState.Started;
        }
    }
}
