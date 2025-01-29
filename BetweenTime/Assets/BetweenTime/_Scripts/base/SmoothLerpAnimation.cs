using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace BetweenTime._Scripts.@base
{
    /// <summary>
    ///     Abstract class for implementing a script-based smooth lerp animation
    /// </summary>
    /// <typeparam name="T">
    ///     The type of the value to execute the lerp function on
    /// </typeparam>
    /// <typeparam name="TLerp">
    ///     The type of the lerp operation to use, extending <see cref="Lerp{T}" />
    ///     (e.g. <see cref="Vector3Lerp" /> or <see cref="ScalarLerp" />)
    /// </typeparam>
    public abstract class SmoothLerpAnimation<T, TLerp> : MonoBehaviour where T : struct where TLerp : Lerp<T>, new()
    {
        private static readonly TLerp LerpOp = new(); // Instance of the lerp functor

        /// <summary>
        ///     Coroutine for executing the lerp operation;
        ///     calls <see cref="OnLerp" /> on each iteration and <see cref="OnLerpEnd" /> at the end
        /// </summary>
        /// <param name="duration">
        ///     The duration of the lerp operation
        /// </param>
        /// <param name="endValue">
        ///     The end value of the lerp operation
        /// </param>
        /// <param name="startValue">
        ///     The start value of the lerp operation
        /// </param>
        /// <returns>
        ///     The coroutine for the lerp operation
        /// </returns>
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

        /// <summary>
        ///     Executes a lerp operation on the value in a separate coroutine
        /// </summary>
        /// <param name="duration">
        ///     The duration of the lerp operation
        /// </param>
        /// <param name="endValue">
        ///     The end value of the lerp operation
        /// </param>
        /// <param name="startValue">
        ///     The start value of the lerp operation
        /// </param>
        protected void Lerp(float duration, T endValue, T startValue)
        {
            StartCoroutine(LerpCoroutine(duration, endValue, startValue));
        }

        /// <summary>
        ///     Called on each iteration of the lerp operation
        /// </summary>
        /// <param name="value">
        ///     The current value of the lerp operation
        /// </param>
        protected abstract void OnLerp(T value);

        /// <summary>
        ///     Optional method called at the end of the lerp operation;
        ///     can be overriden to implement custom behavior
        /// </summary>
        protected virtual void OnLerpEnd()
        {
            // Empty to allow for optional override
        }
    }
}