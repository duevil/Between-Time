using BetweenTime._Scripts.@base;
using Unity.VisualScripting;
using UnityEngine;

// TODO: Comments

namespace BetweenTime._Scripts.steampunk
{
    public class Maze : SmoothLerpAnimation<Vector3, Vector3Lerp>
    {
        private const float Offset = 0.3738f;
        private const float LerpDuration = 0.75f;
        [SerializeField] private Timecore timecore;
        private bool _finished;

        private Vector3 _position;

        private void Start()
        {
            timecore.AddComponent<ScaleLerp>();
            timecore.SetFreeze(true);
            timecore.transform.localPosition = new Vector3(0f, 3.2f, 0.3829999f);
            _position = transform.position;
        }

        public void MainStateListener(MainState state)
        {
            switch (state)
            {
                case MainState.MazeActive:
                    timecore.SetFreeze(true, false);
                    Anim(GameController.instance.mazePositionsState.value);
                    break;
            }
        }

        public void MazePositionListener(MazePosition mazePosition)
        {
            Anim(mazePosition);
        }

        protected override void OnLerp(Vector3 value)
        {
            timecore.transform.position = value;
        }

        protected override void OnLerpEnd()
        {
            if (GameController.instance.mainState.value != MainState.MazeSolved || _finished) return;
            var pos = GameController.instance.mazePositionsState.value;
            Anim(new MazePosition(pos.x + 1, pos.y));
            timecore.GetComponent<ScaleLerp>().ScaleUp();
            _finished = true;
        }

        private void Anim(MazePosition mazePosition)
        {
            var currentPosition = timecore.transform.position;
            var newPosition = new Vector3(
                _position.x,
                _position.y - Offset * mazePosition.y,
                _position.z + Offset * mazePosition.x);
            Lerp(LerpDuration, newPosition, currentPosition);
        }

        private class ScaleLerp : SmoothLerpAnimation<Vector3, Vector3Lerp>
        {
            protected override void OnLerp(Vector3 value)
            {
                transform.localScale = value;
            }

            protected override void OnLerpEnd()
            {
                GetComponent<Timecore>().SetFreeze(false);
            }

            public void ScaleUp()
            {
                Lerp(LerpDuration, Vector3.one, transform.localScale);
            }
        }
    }
}