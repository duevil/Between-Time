using System;
using BetweenTime._Scripts.@base;
using Unity.VisualScripting;
using UnityEngine;

namespace BetweenTime._Scripts.maya
{
    public class Step : MonoBehaviour
    {
        private const float LerpDuration = 1f;
        private const float PositionOffset = 0.1f;
        private const float RotationAngle = 43f;

        private AxisMoveSmoothLerpAnimation _positionLerp;
        private RotationLerp _rotationLerp;


        private void Awake()
        {
            _positionLerp = gameObject.AddComponent<AxisMoveSmoothLerpAnimation>();
            _rotationLerp = gameObject.AddComponent<RotationLerp>();
            _rotationLerp.LerpEndAction += () => _positionLerp.Lerp(LerpDuration, PositionOffset, transform.right);
        }

        public event Action LerpEndAction
        {
            add => _rotationLerp.LerpEndAction += value;
            remove => _rotationLerp.LerpEndAction -= value;
        }

        public void MoveOut()
        {
            Debug.Log($"{gameObject.name} moving out");
            _rotationLerp.Lerp(LerpDuration, RotationAngle);
        }


        private class RotationLerp : SmoothLerpAnimation<Vector3, Vector3Lerp>
        {
            private Quaternion _initialRotation;
            public Action LerpEndAction;

            private void Start()
            {
                _initialRotation = transform.localRotation;
            }

            protected override void OnLerp(Vector3 value)
            {
                transform.localRotation = Quaternion.Euler(value);
            }

            public void Lerp(float duration, float angle)
            {
                Lerp(duration,
                    _initialRotation.eulerAngles + transform.up * angle,
                    transform.localRotation.eulerAngles);
            }

            protected override void OnLerpEnd()
            {
                LerpEndAction?.Invoke();
            }
        }
    }
}