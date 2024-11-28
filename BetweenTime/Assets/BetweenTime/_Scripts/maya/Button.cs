using UnityEngine;

namespace BetweenTime._Scripts.maya
{
    public class Button : MonoBehaviour
    {
        public MayaController mayaController;
        public int buttonNumber;
        public bool isPressed;

        // registers the button in mayaController
        private void Start()
        {
            MayaController.buttons.Add(this);
        }

        public void Triggered()
        {
            isPressed = true;
            print("button " + buttonNumber + " was pressed");
            mayaController.input += buttonNumber;
            Press();
            mayaController.IncreaseCount();
        }

        // visibly presses the button    
        public void Press()
        {
            Move(-0.15f);
            Collider(false);
        }

        private void Collider(bool value)
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

        public void Move(float distance)
        {
            var currentPosition = transform.localPosition;
            transform.localPosition = new Vector3(currentPosition.x, currentPosition.y + 0.35f * distance,
                currentPosition.z + 1 * distance);
        }
    }
}