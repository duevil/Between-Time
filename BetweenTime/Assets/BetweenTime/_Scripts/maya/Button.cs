using UnityEngine;

namespace BetweenTime._Scripts.maya
{
    public class Button : MonoBehaviour
    {
        [SerializeField]
        private Maya maya;

        public int buttonNumber;
        public bool isPressed;

        // registers the button in Maya
        private void Start()
        {
            maya.buttons.Add(this);
        }

        // called when a button is pressed
        public void Triggered()
        {
            isPressed = true;
            print("button " + buttonNumber + " was pressed");
            maya.input += buttonNumber;
            Press();
            maya.IncreaseCount();
        }

        // visibly presses the button    
        public void Press()
        {
            Move(-0.15f);
            Collider(false);
        }

        // changes the collider being activated or not
        public void Collider(bool value)
        {
            GetComponent<Collider>().enabled = value;
        }

        // visibly unpresses the button
        public void Unpress()
        {
            if (!isPressed) return;

            Move(0.15f);
            Collider(true);
            isPressed = false;
        }

        // moves the button in y and z direction
        // hardcoded y - z - proportion
        public void Move(float distance)
        {
            var currentPosition = transform.localPosition;
            transform.localPosition = new Vector3(currentPosition.x, currentPosition.y + 0.35f * distance, currentPosition.z + 1 * distance);
        }
    }
}