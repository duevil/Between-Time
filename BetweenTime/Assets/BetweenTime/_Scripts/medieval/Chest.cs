using System;
using BetweenTime._Scripts.@base;
using UnityEngine;

namespace BetweenTime._Scripts.medieval
{
    [RequireComponent(typeof(Animator))]
    public class ChestAnim : MonoBehaviour
    {
        private static readonly int Factor = Animator.StringToHash("Factor");
        private Animator _animator;

        private void OnEnable()
        {
            _animator = GetComponent<Animator>();
        }

        private void Start()
        {
            GameController.Instance.mainState.onChange.AddListener(value =>
            {
                if (value != MainState.BookBinarySolved) return;
                OpenChest();
            });
        }

        public void OpenChest()
        {
            _animator.SetFloat(Factor, 1.0f);
            _animator.Play("ChestOpen", -1, 0.0f);
        }

        public void CloseChest()
        {
            _animator.SetFloat(Factor, -1.0f);
            _animator.Play("ChestOpen", -1, 1.0f);
        }
    }
}