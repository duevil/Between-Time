using Unity.Mathematics;
using UnityEngine;

namespace BetweenTime._Scripts
{
    /// <summary>
    ///    Behaviour that animates the warp effect
    /// </summary>
    public class WarpAnimationBehaviour : StateMachineBehaviour
    {
        private Warp _warp; // The warp component

        /// <summary>
        ///    Initializes the warp component
        /// </summary>
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _warp = animator.GetComponent<Warp>();
        }

        /// <summary>
        ///   Resets the warp value when the animation is done
        /// </summary>
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _warp.Value = 0;
        }

        /// <summary>
        ///   Updates the warp value based on the current time;
        ///     the warp value is a sine wave that goes from 0 to 1 and back to 0
        /// </summary>
        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            var time = stateInfo.normalizedTime;
            // value is a sine wave that goes from 0 to 1 and back to 0
            _warp.Value = math.clamp(math.sin(-0.5f * math.PI + time * 2 * math.PI) * 0.55f + 0.55f, 0f, 1f);
            // indicate that the warp is halfway done
            _warp.MarkHalfway(time > 0.5f);
        }
    }
}