using BetweenTime._Scripts.@base;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

namespace BetweenTime._Scripts.steampunk
{
    public class MachineDoor : MonoBehaviour
    {
        private static readonly int TriggerOpen = Animator.StringToHash("open");
        private static readonly int TriggerClose = Animator.StringToHash("close");

        private const ushort Timecode = 0x14ea;

        [SerializeField]
        private XRSocketInteractor socket;

        private bool _status;

        private Animator _animator;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private void Start()
        {
            _animator = GetComponent<Animator>();
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
            _animator.SetTrigger(movement ? TriggerOpen : TriggerClose);
            socket.enabled = movement;
        }
    }
}