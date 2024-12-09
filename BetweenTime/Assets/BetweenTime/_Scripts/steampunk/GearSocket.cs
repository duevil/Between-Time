using BetweenTime._Scripts;
using BetweenTime._Scripts.@base;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class GearSocket : MonoBehaviour
{
    [SerializeField]
    private MachineDoor door;

    [SerializeField]
    private GameObject gear;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        var socket = GetComponent<XRSocketInteractor>();
        socket.enabled = false;
        socket.selectEntered.AddListener(OnSelectEntered);
    }

    private void OnSelectEntered(SelectEnterEventArgs args)
    {
        door.moveDoor(false);
        GetComponent<XRSocketInteractor>().enabled = false;
        GameController.Instance.mainState.Value = MainState.MazeActive;
        gear.GetComponent<Collider>().enabled = false;
        gear.GetComponent <Outline>().OutlineWidth = 0;
    }
}
