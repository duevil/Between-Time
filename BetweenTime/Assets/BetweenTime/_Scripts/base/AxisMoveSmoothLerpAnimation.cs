using Unity.VisualScripting;
using UnityEngine;

namespace BetweenTime._Scripts.@base
{
    public class AxisMoveSmoothLerpAnimation : SmoothLerpAnimation<Vector3, Vector3Lerp>
    {
        private Vector3 _initialPosition;

        protected virtual void Start()
        {
            _initialPosition = transform.localPosition;
        }

        protected override void OnLerp(Vector3 value)
        {
            transform.localPosition = value;
        }

        protected void Lerp(float duration, float offset)
        {
            Lerp(duration, offset, transform.up);
        }

        public void Lerp(float duration, float offset, Vector3 axis)
        {
            Lerp(duration,
                _initialPosition + axis * offset,
                transform.localPosition);
        }
    }
}