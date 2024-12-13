using System;
using BetweenTime._Scripts.@base;
using Unity.VisualScripting;
using UnityEngine;

namespace BetweenTime._Scripts.steampunk
{
    public class Maze2 : SmoothLerpAnimation<Vector3, Vector3Lerp>
    {
        private const ushort Timecode = 0x14ea;

        [SerializeField] private Timecore timecore;


        private void Start()
        {
            timecore.SetFreeze(true);
        }

        public void MainStateListener(MainState state)
        {
            switch (state)
            {
                case MainState.MazeActive:
                    Anim(GameController.instance.mazePositionsState.Value);
                    break;
                case MainState.MazeSolved:
                    timecore.SetFreeze(false);
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

        private void Anim(MazePosition mazePosition)
        {
            //TODO: calc pos
        }
    }
}