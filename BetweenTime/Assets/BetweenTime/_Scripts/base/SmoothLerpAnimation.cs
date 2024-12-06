using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace BetweenTime._Scripts.@base
{
    public abstract class SmoothLerpAnimation<T, TLerp> : MonoBehaviour where T : struct where TLerp : Lerp<T>, new()
    {
        private static readonly TLerp LerpOp = new();

        private IEnumerator LerpCoroutine(float duration, T endValue, T startValue)
        {
            for (var time = 0f; time < duration; time += Time.deltaTime)
            {
                var t = Mathf.SmoothStep(0, 1, time / duration);
                var value = LerpOp.Operation(startValue, endValue, t);
                OnLerp(value);

                yield return null;
            }

            OnLerp(endValue);
            OnLerpEnd();
        }

        protected void Lerp(float duration, T endValue, T startValue)
        {
            StartCoroutine(LerpCoroutine(duration, endValue, startValue));
        }

        protected abstract void OnLerp(T value);

        protected virtual void OnLerpEnd()
        {
            // Empty to allow for optional override
        }
    }
}