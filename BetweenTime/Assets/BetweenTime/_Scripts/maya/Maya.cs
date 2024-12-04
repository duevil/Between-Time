using BetweenTime._Scripts.@base;
using System.Collections.Generic;
using UnityEngine;

// manages the atztec puzzle
namespace BetweenTime._Scripts.maya
{
    public class Maya : MonoBehaviour
    {
        private const ushort Timecode = 0xdc3e;

        [SerializeField]
        private GameObject timeCube;

        private bool _isSolved;
        private bool _isSynced;
        private bool _symbolsActivated;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private void Start()
        {
            // hides buttons at the beginning
            MoveButtons(-0.15f);
            ButtonColliders(false);
        }

        // handles the change of the timecode and activates or deactivates the puzzle
        public void HandleTimeCode(ushort value)
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

        // handles the change of the MainState
        public void HandleMainState(MainState value)
        {
            if (value != MainState.InputFieldOpened || _symbolsActivated) return;

            ActivateSymbols();
            _symbolsActivated = true;
        }

        // activates the puzzle
        public void EnableNumPad()
        {
            // movement to the right position
            RotateStairs(0);
            MoveButtons(0.3f);
            ButtonColliders(true);
        }

        // deactivates and resets the puzzle
        public void DisableNumPad()
        {
            ResetNumPad();
            MoveButtons(-0.3f);
            ButtonColliders(false);
            RotateStairs(45);
        }

        // finishes the maya puzzle
        public void Finish()
        {
            DisableNumPad();
            timeCube.GetComponent<Collider>().enabled = true;
            timeCube.GetComponent<Rigidbody>().isKinematic = false;
            Hint();
            _isSolved = true;
            GameController.Instance.mainState.Value = MainState.InputFieldSolved;
        }


        private const string Solution = "485361";
        public string input = "";
        public int count;

        public List<Button> buttons = new();
        public List<Stair> stairs = new();

        // moves every registered Button
        public void MoveButtons(float distance)
        {
            foreach (var button in buttons)
            {
                button.Move(distance);
            }
        }

        // called whenever a Button is triggered
        // increases count to 6 and compares the input to the solution
        // handles wrong or right input
        public void IncreaseCount()
        {
            count++;

            if (count != 6) return;
            if (Solution == input)
                Finish();
            else
                ResetNumPad();
        }

        // resets the NumPad
        // only called when input is not matching solution
        public void ResetNumPad()
        {
            input = "";
            count = 0;

            foreach (var button in buttons)
            {
                button.Unpress();
            }
        }

        // enables/disables all Button Collider
        public void ButtonColliders(bool value)
        {
            foreach (var button in buttons)
            {
                button.Collider(value);
            }
        }

        // activates the symbols
        // called from mayaCommunicator when MainState == MainState.InputFieldOpened
        public void ActivateSymbols()
        {
            foreach (var button in buttons)
            {
                // TODO
            }
        }

        // give hint for next puzzle
        public void Hint()
        {
            foreach (var button in buttons)
            {
                var number = button.buttonNumber;

                if (number is < 5 or > 8) continue;

                button.Move(0.3f);
            }
        }

        // rotates every registered stair
        public void RotateStairs(float xR)
        {
            foreach (var stair in stairs)
            {
                stair.RotateStair(xR);
            }
        }
    }
}