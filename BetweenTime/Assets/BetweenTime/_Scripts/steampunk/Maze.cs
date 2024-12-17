using System.Collections;
using BetweenTime._Scripts.@base;
using Unity.VisualScripting;
using UnityEngine;

namespace BetweenTime._Scripts.steampunk
{
    public class Maze : SmoothLerpAnimation<Vector3, Vector3Lerp>
    {
        [SerializeField] 
        private Timecore timecore;

        private Vector3 position;

        private const float Offset = 0.3738f;
        private const float LerpDuration = 1f;

        private void Start()
        {
            timecore.SetFreeze(true);
            timecore.transform.localPosition = new Vector3(0f, 3.2f, 0.3829999f);
            position = transform.position;
        }

        public void MainStateListener(MainState state)
        {
            switch (state)
            {
                case MainState.MazeActive:
                    Anim(GameController.instance.mazePositionsState.Value);
                    break;
                case MainState.MazeSolved:
                    StartCoroutine(MazeSolved());
                    break;
            }
        }

        public void MazePositionListener(MazePosition mazePosition)
        {
            Anim(mazePosition);
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private IEnumerator MazeSolved()
        {
            Anim(new MazePosition(8, 7, Direction.None));
            yield return new WaitForSeconds(LerpDuration);
            timecore.transform.localScale = Vector3.one;
            timecore.SetFreeze(false);
        }

        protected override void OnLerp(Vector3 value)
        {
            timecore.transform.position = value;
        }

        private void Anim(MazePosition mazePosition)
        {
            var currentPosition = timecore.transform.position;
            var newPosition = new Vector3(
                position.x,
                position.y - Offset * mazePosition.y,
                position.z + Offset * mazePosition.x);
            Lerp(LerpDuration, newPosition, currentPosition);
        }
    }
}