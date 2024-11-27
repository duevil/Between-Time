using BetweenTime._Scripts.@base;
using UnityEngine;
using UnityEngine.Serialization;

// manages the atztec puzzle
namespace BetweenTime._Scripts.maya
{
    public class Maya : MonoBehaviour
    {
        private const ushort Timecode = 0xdc3e;
        private bool _isSynced;
        private bool _isSolved;

        private bool _symbolsActivated;
    
        public stairManager stairManager;
        public ButtonManager buttonManager;

        public GameObject timeCube;

        // handles the change of the timecode and activates or deactivates the puzzle
        public void SyncManager(ushort value)
        {
            if (_isSolved) return;

            if(Timecode != value)
            {
                if (!_isSynced) return;
            
                DisableNumPad();
                _isSynced = false;
                return;
            }
        
            EnableNumPad();
            _isSynced = true;
        }

        // activates the puzzle
        public void EnableNumPad()
        {   
            // movement to the right position
            stairManager.rotateStairs(0);
            buttonManager.MoveButtons(0.3f);
            buttonManager.ButtonColliders(true);
        }

        // deactivates and resets the puzzle
        public void DisableNumPad() 
        {
            buttonManager.ResetNumPad();
            buttonManager.MoveButtons(-0.3f);
            buttonManager.ButtonColliders(false);
            stairManager.rotateStairs(45);
        }

        public void HandleMainState(MainState value)
        {
            if (_symbolsActivated) return;
            if (value != MainState.InputFieldOpened) return;
        
            buttonManager.ActivateSymbols();
            _symbolsActivated = true;
        }

        // finishes the puzzle
        public void FinishesMaya()
        {
            DisableNumPad();
            timeCube.GetComponent<Collider>().enabled = true;
            timeCube.GetComponent<Rigidbody>().isKinematic = false;
            buttonManager.Hint();
            _isSolved = true;
            GameController.Instance.mainState.Value = MainState.InputFieldSolved;
        }
    }
}
