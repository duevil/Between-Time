using BetweenTime._Scripts.@base;
using UnityEngine;

public class isButton : MonoBehaviour
{
    public buttonManager bM;
    public int buttonNumber;
    public bool isPressed;

    public void triggered()
    {
        isPressed = true;
        print("button " +  buttonNumber + " was pressed");
        bM.input += buttonNumber;
        Press();
        bM.increaseCount();
    }

    // visibly presses the button    
    public void Press()
    {
        move(-0.15f);
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

        move(0.15f);
        Collider(true);
        isPressed = false;
    }

    public void move(float distance)
    {
        Vector3 currentPosition = transform.localPosition;
        transform.localPosition = new Vector3(currentPosition.x, currentPosition.y + 0.35f * distance, currentPosition.z + 1 * distance);
    }
}
