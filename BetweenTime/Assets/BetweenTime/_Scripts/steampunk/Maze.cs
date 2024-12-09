using BetweenTime._Scripts;
using BetweenTime._Scripts.@base;
using UnityEngine;

public class Maze : MonoBehaviour
{
    [SerializeField]
    private GameObject _timecore;

    private GameObject light;

    void Start()
    {
        light = _timecore.transform.Find("Point Light").gameObject;
        light.SetActive(false);
        Debug.Log(_timecore.name + " wurde deaktiviert.");
    }

    public void HandleMainState(MainState value)
    {
        if (value != MainState.MazeActive) return;

        activateCore();
    }

    private void activateCore()
    {
        light.SetActive(true);
        Debug.Log(_timecore.name + " wurde aktiviert.");
        _timecore.transform.position = transform.position;
    }
}
