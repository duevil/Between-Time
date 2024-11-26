using UnityEngine;

public class stairManager : MonoBehaviour
{

    public void rotateStairs(float xR)
    {
        foreach (Transform child in transform)
        {
            Vector3 currentRotation = child.localEulerAngles;
            child.localEulerAngles = new Vector3(xR, currentRotation.y, currentRotation.z);
        }
    }

}
