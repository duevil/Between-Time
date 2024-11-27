using UnityEngine;

namespace BetweenTime._Scripts.maya
{
    public class ButtonManager : MonoBehaviour
    {
        // Button Manager
        // manages the characteristics and the input of the buttons

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private void Start()
        {
            // hide buttons at the beginning
            MoveButtons(-0.15f);
            ButtonColliders(false);
        }

        public void MoveButtons(float distance)
        {
            foreach (Transform child in transform)
            {
                child.gameObject.GetComponent<Button>().Move(distance);
            }
        }

        public Maya maya;

        private const string Solution = "485361";
        public string input = "";
        public int count;

        public void IncreaseCount()
        {
            count++;

            if (count != 6) return;
            if (Solution == input)
            {
                maya.FinishesMaya();
            }
            else
            {
                ResetNumPad();
            }
        }

        public void ResetNumPad()
        {
            input = "";
            count = 0;

            ResetButtons();
        }

        public void ButtonColliders(bool value)
        {
            foreach (Transform child in transform)
            {
                child.GetComponent<Collider>().enabled = value;
            }
        }

        private void ResetButtons()
        {
            foreach (Transform child in transform)
            {
                child.GetComponent<Button>().Unpress();
            }
        }

        public void ActivateSymbols()
        {
            foreach (Transform child in transform)
            {
                 // TODO
            }
        }

        public void Hint()
        {
            foreach (Transform child in transform)
            {
                print("in hint");
                var button = child.GetComponent<BetweenTime._Scripts.maya.Button>();
                var number = button.buttonNumber;
                print(number);

                if (number is < 5 or > 8) continue;


                button.Move(0.3f);
            }
        }
    }
}
