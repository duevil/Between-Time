using BetweenTime._Scripts;
using BetweenTime._Scripts.@base;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class Maze : MonoBehaviour
{
    [SerializeField] private GameObject timecore;

    private new GameObject _light;

    void Start()
    {
        _light = timecore.transform.Find("Point Light").gameObject;
        DeactivateCore();
    }

    public void HandleMainState(MainState value)
    {
        if (value != MainState.MazeActive) return;

        ActivateCore();
    }

    private void DeactivateCore()
    {
        _light.SetActive(false);
        //timecore.GetComponent<Timecore>().SetFreeze(true);
    }

    private void ActivateCore()
    {
        _light.SetActive(true);
        Debug.Log(timecore.name + " wurde aktiviert.");
        //timecore.transform.position = transform.position;
        //timecore.GetComponent<Timecore>().SetFreeze(true);
    }


    private static Vector3 MazePosToLocal(MazePosition mazePosition, Vector3 origin)
    {
        const float offset = 0.3837f;
        return origin + new Vector3(
            mazePosition.x + 1 * offset,
            mazePosition.y + 1 * offset
        );
    }
    
    private class PosAnim : SmoothLerpAnimation<Vector3, Vector3Lerp>
    {
        private Vector3 _start;
        
        protected override void OnLerp(Vector3 value)
        {
            // Todo set timecore pos
        }

        public void Anim(MazePosition mazePosition)
        {
            var pos = MazePosToLocal(mazePosition, _start);
            Lerp(0.5f, pos, _start);
        }
    }
}