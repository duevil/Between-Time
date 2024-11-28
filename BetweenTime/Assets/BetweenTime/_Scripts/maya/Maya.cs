using BetweenTime._Scripts.@base;
using UnityEngine;

// manages the atztec puzzle
namespace BetweenTime._Scripts.maya
{
    public class Maya : MonoBehaviour
    {
        private const ushort Timecode = 0xdc3e;

        [SerializeField]
        private MayaController mayaController;

        public GameObject timeCube;
        private bool _isSolved;
        private bool _isSynced;

        private bool _symbolsActivated;

        // handles the change of the timecode and activates or deactivates the puzzle
        public void SyncManager(ushort value)
        {
            if (_isSolved) return;

            if (Timecode != value)
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
            mayaController.RotateStairs(0);
            mayaController.MoveButtons(0.3f);
            mayaController.ButtonColliders(true);
        }

        // deactivates and resets the puzzle
        public void DisableNumPad()
        {
            mayaController.ResetNumPad();
            mayaController.MoveButtons(-0.3f);
            mayaController.ButtonColliders(false);
            mayaController.RotateStairs(45);
        }

        public void HandleMainState(MainState value)
        {
            if (_symbolsActivated) return;
            if (value != MainState.InputFieldOpened) return;

            mayaController.ActivateSymbols();
            _symbolsActivated = true;
        }

        // finishes the puzzle
        public void FinishMaya()
        {
            DisableNumPad();
            timeCube.GetComponent<Collider>().enabled = true;
            timeCube.GetComponent<Rigidbody>().isKinematic = false;
            mayaController.Hint();
            _isSolved = true;
            GameController.Instance.mainState.Value = MainState.InputFieldSolved;
        }
    }
}