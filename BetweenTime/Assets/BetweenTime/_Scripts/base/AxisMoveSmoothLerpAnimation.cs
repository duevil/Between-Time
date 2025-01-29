using Unity.VisualScripting;
using UnityEngine;

namespace BetweenTime._Scripts.@base
{
    /// <summary>
    ///     Implements a smooth lerp animation for moving an object along an axis
    ///     using a <see cref="SmoothLerpAnimation{T,TLerp}" /> with a <see cref="Vector3Lerp" />
    /// </summary>
    public class AxisMoveSmoothLerpAnimation : SmoothLerpAnimation<Vector3, Vector3Lerp>
    {
        // Remember the initial position of the object for calculating the offset
        private Vector3 _initialPosition;

        /// <summary>
        ///     Initializes the initial position of the object
        /// </summary>
        protected virtual void Start()
        {
            _initialPosition = transform.localPosition;
        }

        /// <summary>
        ///     Sets the object's position to the lerp value
        /// </summary>
        /// <param name="value">
        ///     The value to set the object's position to
        /// </param>
        protected override void OnLerp(Vector3 value)
        {
            transform.localPosition = value;
        }

        /// <summary>
        ///     Lerp the object along the <see cref="Transform.up" /> axis
        /// </summary>
        /// <param name="duration">
        ///     The duration of the lerp operation
        /// </param>
        /// <param name="offset">
        ///     The offset to move the object by
        /// </param>
        protected void Lerp(float duration, float offset)
        {
            Lerp(duration, offset, transform.up);
        }

        /// <summary>
        ///     Lerp the object along a specified axis
        /// </summary>
        /// <param name="duration">
        ///     The duration of the lerp operation
        /// </param>
        /// <param name="offset">
        ///     The offset to move the object by
        /// </param>
        /// <param name="axis">
        ///     The axis to move the object along
        /// </param>
        public void Lerp(float duration, float offset, Vector3 axis)
        {
            Lerp(duration,
                _initialPosition + axis * offset,
                transform.localPosition);
        }
    }
}