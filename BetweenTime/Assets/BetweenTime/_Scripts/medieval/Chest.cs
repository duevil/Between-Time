using BetweenTime._Scripts.@base;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

namespace BetweenTime._Scripts.medieval
{
    /// <summary>
    ///     Class that represents a chest in the game
    /// </summary>
    [RequireComponent(typeof(Animator))]
    public class Chest : MonoBehaviour
    {
        // Property ID for the animator's open trigger
        private static readonly int Open = Animator.StringToHash("Open");

        // Property ID for the animator's close trigger
        private static readonly int Close = Animator.StringToHash("Close");
        private Animator _animator; // The animator component of the chest

        /// <summary>
        ///     Subscribes to the main state change event to open the chest when the book binary is solved
        ///     and close it when the game is won
        /// </summary>
        private void Start()
        {
            var timecoreGrab = GetComponentInChildren<XRGrabInteractable>();
            timecoreGrab.enabled = false;
            GameController.Instance.mainState.onChange.AddListener(value =>
            {
                if (value != MainState.BookBinarySolved && value != MainState.GameWon) return;
                if (value == MainState.BookBinarySolved) timecoreGrab.enabled = true;
                _animator.SetTrigger(value == MainState.BookBinarySolved ? Open : Close);
            });
        }

        /// <summary>
        ///     Initializes the animator component on enable
        /// </summary>
        private void OnEnable()
        {
            _animator = GetComponent<Animator>();
        }
    }
}