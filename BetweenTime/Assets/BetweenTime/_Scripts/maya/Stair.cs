using System;
using BetweenTime._Scripts.maya;
using UnityEngine;

public class Stair : MonoBehaviour
{
    [SerializeField]
    private MayaController mayaController;
    
    private void Start()
    {
        MayaController.stairs.Add(this);
    }

    public void RotateStair(float xR)
    {
        Vector3 currentRotation = transform.localEulerAngles;
        transform.localEulerAngles = new Vector3(xR, currentRotation.y, currentRotation.z);
    }
}
