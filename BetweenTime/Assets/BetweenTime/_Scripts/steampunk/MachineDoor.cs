using BetweenTime._Scripts;
using BetweenTime._Scripts.@base;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class MachineDoor : MonoBehaviour
{

    private const ushort _Timecode = 0x14ea;

    [SerializeField]
    private XRSocketInteractor _socket;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    public void HandleTimeCode(ushort value)
    {
        if (value != _Timecode || GameController.Instance.mainState.Value != MainState.BookBinarySolved) return;

        moveDoor(true);
        _socket.enabled = true;
    }

    public void moveDoor(bool value)
    {
        var z = 140.0f;
        if(!value) z = 0f;
        transform.localEulerAngles = new Vector3(0, 0, z);
    }
}
