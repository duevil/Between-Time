using System;
using BetweenTime._Scripts.maya;
using UnityEngine;

public class Stair : MonoBehaviour
{
    [SerializeField]
    private Maya maya;
    
    private void Start()
    {
        // register the stair in Maya
        maya.stairs.Add(this);
    }

    // rotates the stair
    // currently in x-direction
    // TODO: adjust Rotation
    public void RotateStair(float xR)
    {
        Vector3 currentRotation = transform.localEulerAngles;
        transform.localEulerAngles = new Vector3(xR, currentRotation.y, currentRotation.z);
    }
}
