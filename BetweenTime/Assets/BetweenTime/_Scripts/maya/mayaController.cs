using System.Collections.Generic;
using UnityEngine;

namespace BetweenTime._Scripts.maya
{
    // manages changes on physical maya objects
    public class MayaController : MonoBehaviour
    {
        public Maya maya;
        
        private const string Solution = "485361";
        public string input = "";
        public int count;

        public static List<Button> buttons = new();
        public static List<Stair> stairs = new();

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private void Start()
        {
            // hides buttons at the beginning
            MoveButtons(-0.15f);
            ButtonColliders(false);
        }

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
                maya.FinishMaya();
            else
                ResetNumPad();
        }

        // resets the NumPad
        // only called when input is not matching solution
        public void ResetNumPad()
        {
            input = "";
            count = 0;

            foreach (Transform child in transform)
            {
                var button = GetComponent<Button>();
                
                // only allows actual buttons
                if (button == null) continue;
                
                button.Unpress();
            }
        }

        // enables/disables all Button Collider
        public void ButtonColliders(bool value)
        {
            foreach (Transform child in transform)
            {
                var button = GetComponent<Button>();
                
                // only allow actual buttons
                if (button == null) continue;

                child.GetComponent<Collider>().enabled = value;
            }
        }

        // activates the symbols
        // called from mayaCommunicator when MainState == MainState.InputFieldOpened
        public void ActivateSymbols()
        {
            foreach (Transform child in transform)
            {
                // TODO
            }
        }
        
        // give hint for next puzzle
        public void Hint()
        {
            foreach (Transform child in transform)
            {
                var button = child.GetComponent<Button>();
                
                // only allow actual buttons
                if (button == null) continue;
                
                var number = button.buttonNumber;

                
                if (number is < 5 or > 8) continue;
                
                button.Move(0.3f);
            }
        }
        
        // rotates every registered stair
        public void RotateStairs(float xR)
        {
            foreach (Transform child in transform)
            {
                var stair = child.GetComponent<Stair>();
                
                // only allow actual buttons
                if (stair == null) continue;
                
                stair.RotateStair(xR);
            }
        }
    }
}