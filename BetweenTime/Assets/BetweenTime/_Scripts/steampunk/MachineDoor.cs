using BetweenTime._Scripts.@base;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

namespace BetweenTime._Scripts.steampunk
{
    public class MachineDoor : MonoBehaviour
    {
        private static readonly int Direction1 = Animator.StringToHash("direction");
        private static readonly int Trigger = Animator.StringToHash("trigger");

        private const ushort Timecode = 0x14ea;

        [SerializeField]
        private XRSocketInteractor socket;

        private bool _status = false;

        private Animator _animator;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private void Start()
        {
            _animator = GetComponent<Animator>();
        }

        private void MoveDoor(bool value)
        {
            var direction = 1f;

            if (!value)
            {
                direction = -1f;
            }

            _animator.SetFloat(Direction1, direction);
            _animator.SetTrigger(Trigger);
        }

        public void MainStateListener(MainState value)
        {
            Set(GameController.instance.timecodeState.Value, value);
        }

        public void TimeCodeListener(ushort value)
        {
            Set(value, GameController.instance.mainState.Value);
        }

        private void Set(ushort timecode, MainState mainState)
        {
            var movement = timecode == Timecode && mainState == MainState.BookBinarySolved;
        
            if(movement == _status) return;
            _status = movement;
        
            MoveDoor(movement);
            socket.enabled = movement;
        }
    }
}