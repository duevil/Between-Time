using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class buttonManager : MonoBehaviour
{
    // Button Manager
    // manages the characteristics and the input of the buttons

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // hide buttons at the beginning
        moveButtons(-0.15f);
        buttonColliders(false);
    }

    public void moveButtons(float distance)
    {
        foreach (Transform child in transform)
        {
            child.gameObject.GetComponent<isButton>().move(distance);
        }
    }

    public Maya maya;

    string solution = "485361";
    public string input = "";
    public int count = 0;

    public void increaseCount()
    {
        count++;

        if (count == 6)
        {
            if (solution == input)
            {
                maya.finishesMaya();
            }
            else
            {
                resetNumPad();
            }
        }
    }

    public void resetNumPad()
    {
        input = "";
        count = 0;

        resetButtons();
    }

    public void buttonColliders(bool value)
    {
        foreach (Transform child in transform)
        {
            child.GetComponent<Collider>().enabled = value;
        }
    }

    private void resetButtons()
    {
        foreach (Transform child in transform)
        {
            child.GetComponent<isButton>().Unpress();
        }
    }

    public void activateSymbols()
    {
        foreach (Transform child in transform)
        {
            
        }
    }

    public void hint()
    {
        foreach (Transform child in transform)
        {
            print("in hint");
            isButton button = child.GetComponent<isButton>();
            int number = button.buttonNumber;
            print(number);

            if (number < 5 || number > 8) continue;


            button.move(0.3f);
        }
    }
}
