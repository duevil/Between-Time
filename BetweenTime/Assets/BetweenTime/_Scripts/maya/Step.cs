using System;
using BetweenTime._Scripts.@base;
using Unity.VisualScripting;
using UnityEngine;

namespace BetweenTime._Scripts.maya
{
    /// <summary>
    ///     Class for controlling the movement of the steps in the Maya puzzle
    /// </summary>
    public class Step : MonoBehaviour
    {
        private const float LerpDuration = 1f; // The duration of the lerp animation
        private const float PositionOffset = 0.1f; // The offset value for the position lerp
        private const float RotationAngle = 43f; // The angle to rotate the step by

        private AxisMoveSmoothLerpAnimation _positionLerp; // The position lerp component of the step
        private RotationLerp _rotationLerp; // The rotation lerp component of the step


        /// <summary>
        ///     Initializes the lerp components
        /// </summary>
        private void Awake()
        {
            _positionLerp = gameObject.AddComponent<AxisMoveSmoothLerpAnimation>();
            _rotationLerp = gameObject.AddComponent<RotationLerp>();
            _rotationLerp.LerpEndAction += () => _positionLerp.Lerp(LerpDuration, PositionOffset, transform.right);
        }

        /// <summary>
        ///     Event that is invoked when the lerp animation ends
        /// </summary>
        public event Action LerpEndAction
        {
            add => _rotationLerp.LerpEndAction += value;
            remove => _rotationLerp.LerpEndAction -= value;
        }

        /// <summary>
        ///     Moves the step in
        /// </summary>
        public void MoveOut()
        {
            _rotationLerp.Lerp(LerpDuration, RotationAngle);
        }


        /// <summary>
        ///     Inner class implementing a <see cref="SmoothLerpAnimation{T,TLerp}" /> for rotating the step
        /// </summary>
        private class RotationLerp : SmoothLerpAnimation<Vector3, Vector3Lerp>
        {
            private Quaternion _initialRotation; // The initial rotation of the step
            public Action LerpEndAction; // The action to invoke when the lerp animation ends

            /// <summary>
            ///     Initializes the initial rotation
            /// </summary>
            private void Start()
            {
                _initialRotation = transform.localRotation;
            }

            /// <summary>
            ///     Sets the rotation of the step
            /// </summary>
            /// <param name="value">
            ///     The rotation to set the step to
            /// </param>
            protected override void OnLerp(Vector3 value)
            {
                transform.localRotation = Quaternion.Euler(value);
            }

            /// <summary>
            ///     Starts a lerp animation to rotate the step
            /// </summary>
            /// <param name="duration">
            ///     The duration of the lerp animation
            /// </param>
            /// <param name="angle">
            ///     The angle to rotate the step by
            /// </param>
            public void Lerp(float duration, float angle)
            {
                Lerp(duration,
                    _initialRotation.eulerAngles + transform.up * angle,
                    transform.localRotation.eulerAngles);
            }

            /// <summary>
            ///     Overrides the base method to invoke the lerp end action
            /// </summary>
            protected override void OnLerpEnd()
            {
                LerpEndAction?.Invoke();
            }
        }
    }
}