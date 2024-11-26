using BetweenTime._Scripts.@base;
using UnityEngine;

// manages the atztec puzzle
public class Maya : MonoBehaviour
{
    public string syncCode = "dc3e";
    bool isSynced = false;
    bool isSolved = false;

    bool symbolsActivated = false;
    
    public stairManager stairManager;
    public buttonManager buttonManager;

    public GameObject TimeCube;

    // handles the change of the timecode and activates or deactivates the puzzle
    public void syncManager(ushort value)
    {
        if (isSolved) return;

        string timecode = value.ToString("X4");

        if(!(syncCode == timecode))
        {
            if (isSynced)
            {
                disableNumPad();
                isSynced = false;
                return;
            }
            return;
        }
        
        enableNumPad();
        isSynced = true;
        return;
    }

    // activates the puzzle
    public void enableNumPad()
    {   
        // movement to the right position
        stairManager.rotateStairs(0);
        buttonManager.moveButtons(0.3f);
        buttonManager.buttonColliders(true);
    }

    // deactivates and resets the puzzle
    public void disableNumPad() 
    {
        buttonManager.resetNumPad();
        buttonManager.moveButtons(-0.3f);
        buttonManager.buttonColliders(false);
        stairManager.rotateStairs(45);
        return;
    }

    public void handleMainState(MainState value)
    {
        if (symbolsActivated) return;
        if (!(value == MainState.InputFieldOpened)) return;
        
        buttonManager.activateSymbols();
        symbolsActivated = true;
    }

    // finishes the puzzle
    public void finishesMaya()
    {
        disableNumPad();
        TimeCube.GetComponent<Collider>().enabled = true;
        TimeCube.GetComponent<Rigidbody>().isKinematic = false;
        buttonManager.hint();
    }
}
