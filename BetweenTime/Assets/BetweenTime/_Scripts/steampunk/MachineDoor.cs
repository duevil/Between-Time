using BetweenTime._Scripts.@base;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

// TODO: Comments

namespace BetweenTime._Scripts.steampunk
{
    public class MachineDoor : MonoBehaviour
    {
        private const ushort Timecode = 0x14ea;
        private static readonly int TriggerOpen = Animator.StringToHash("open");
        private static readonly int TriggerClose = Animator.StringToHash("close");

        [SerializeField] private XRSocketInteractor socket;

        private Animator _animator;

        private bool _status;

        private void Start()
        {
            _animator = GetComponent<Animator>();
        }

        public void MainStateListener(MainState value)
        {
            Set(GameController.instance.timecodeState.value, value);
        }

        public void TimeCodeListener(ushort value)
        {
            Set(value, GameController.instance.mainState.value);
        }

        private void Set(ushort timecode, MainState mainState)
        {
            var movement = timecode == Timecode && mainState == MainState.BookBinarySolved;
            if (movement == _status) return;
            _status = movement;
            _animator.SetTrigger(movement ? TriggerOpen : TriggerClose);
            socket.enabled = movement;
        }
    }
}