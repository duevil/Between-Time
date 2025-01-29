using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using BetweenTime._Scripts.@base;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace BetweenTime._Scripts.maya
{
    /// <summary>
    ///     Class for controlling the main logic of the Maya puzzle
    /// </summary>
    public class Maya : MonoBehaviour
    {
        private const ushort Timecode = 0xdc3e; // The timecode assigned to the Maya puzzle
        private const float LerpDuration = 2f; // The duration of the lerp animation of the temple elements
        private const float LerpPositionOffset = 1f; // The offset value for the lerp animation of the temple elements


        // The solution to the Maya puzzle
        private static readonly ReadOnlyCollection<Symbol> Solution = new(new List<Symbol>
        {
            Symbol._4, Symbol._8, Symbol._5, Symbol._3, Symbol._6, Symbol._1
        }); // 485361

        // The hint solution for the Candle puzzle
        private static readonly ReadOnlyCollection<Symbol> HintSolution = new(new List<Symbol>
        {
            Symbol._7, Symbol._8, Symbol._0, Symbol._6
        }); // 7806


        [Tooltip("The audio clips to play when receiving input")] [SerializeField]
        private List<AudioClip> audioClips;

        [FormerlySerializedAs("timeCube")] [SerializeField]
        private GameObject timecore;

        [SerializeField] private GameObject templeTop;
        private readonly List<Symbol> _input = new(Solution.Count);

        private Button[] _buttons; // The buttons of the NumPad
        private bool _isActivated; // Whether the NumPad is activated
        private bool _isSolved; // Whether the puzzle is solved
        private int _stepCount; // The number of steps that are currently moving
        private Step[] _steps; // The steps of the temple
        private TimecoreAnimation _timecoreAnimation; // The timecore animation component
        private TopAnimation _topAnimation; // The temple top animation component


        /// <summary>
        ///     Initializes the NumPad and the temple elements and assigns the button event handlers
        /// </summary>
        private void Awake()
        {
            _buttons = GetComponentsInChildren<Button>();
            _steps = GetComponentsInChildren<Step>();

            foreach (var button in _buttons) button.OnButtonPressed = HandleButtonPressed;
        }

        /// <summary>
        ///     Initializes the temple elements and assigns the animation components
        /// </summary>
        private void Start()
        {
            // for some reason the animation script is added twice, therefore this check
            _timecoreAnimation = timecore.GetComponent<TimecoreAnimation>();
            if (_timecoreAnimation) Destroy(_timecoreAnimation);
            _timecoreAnimation = timecore.AddComponent<TimecoreAnimation>();
            _topAnimation = templeTop.GetComponent<TopAnimation>();
            if (_topAnimation) Destroy(_topAnimation);
            _topAnimation = templeTop.AddComponent<TopAnimation>();
            // animate the core only after the top has finished animating
            _topAnimation.LerpEndAction += () => _timecoreAnimation.Animate();
            _topAnimation.LerpEndAction += () => PlayMoveSound(_topAnimation.transform);

            foreach (var step in _steps) step.LerpEndAction += () => _stepCount--;
        }

        /// <summary>
        ///     Handler for updates of the timecode state value;
        ///     resets the NumPad when the timecode is updated and the puzzle is not yet solved
        /// </summary>
        /// <param name="value">
        ///     The new timecode value
        /// </param>
        public void HandleTimeCode(ushort value)
        {
            if (_isSolved) return;
            ResetNumPad();
        }

        /// <summary>
        ///     Handler for updates of the main state value;
        ///     resets the NumPad when it is opened and finishes the puzzle when the input field was solved
        /// </summary>
        /// <param name="value">
        ///     The new main state value
        /// </param>
        public void HandleMainState(MainState value)
        {
            switch (value)
            {
                case MainState.InputFieldOpened when !_isActivated:
                    _isActivated = true;
                    // movement to the right position
                    foreach (var step in _steps)
                    {
                        step.MoveOut();
                        PlayMoveSound(step.transform);
                        _stepCount++;
                    }

                    // wait for all steps to finish and animate buttons afterward
                    StartCoroutine(Routine());
                    break;

                    // coroutine to wait for all steps to finish and animate buttons afterwards
                    IEnumerator Routine()
                    {
                        yield return new WaitUntil(() => _stepCount == 0);
                        foreach (var button in _buttons)
                        {
                            button.state = Button.State.Released;
                            PlayMoveSound(button.transform);
                        }
                    }
                case MainState.InputFieldSolved when !_isSolved:
                    Finish();
                    break;
            }
        }

        /// <summary>
        ///     Handler for the button pressed event;
        ///     adds the button's symbol to the input and checks if the input matches the solution.
        ///     If the input matches the solution, the main state is set to InputFieldSolved;
        ///     otherwise, the NumPad is reset.
        ///     If the currently set timecode does not match the puzzle's timecode, the button's state is toggled,
        ///     but no input check is performed, rendering the puzzle mechanic inactive.
        /// </summary>
        /// <param name="button">
        ///     The button that was pressed
        /// </param>
        private void HandleButtonPressed(Button button)
        {
            PlayMoveSound(button.transform);

            var gameController = GameController.instance;
            if (gameController.timecodeState.value == Timecode)
            {
                button.state = Button.State.Pressed;
                _input.Add(button.symbol);

                if (_input.Count != Solution.Count) return;
                if (Solution.SequenceEqual(_input)) gameController.mainState.value = MainState.InputFieldSolved;
                else ResetNumPad();
            }
            else
            {
                button.state = button.state switch
                {
                    Button.State.Pressed => Button.State.Released,
                    Button.State.Released => Button.State.Pressed,
                    _ => button.state
                };
            }
        }

        /// <summary>
        ///     Finishes the Maya puzzle;
        ///     animates the temple top and shows the hint solution
        ///     only called when the input matches the solution
        /// </summary>
        private void Finish()
        {
            _isSolved = true;
            _topAnimation.Animate();
            ShowHint();
        }


        /// resets the NumPad;
        /// only called when input is not matching solution
        private void ResetNumPad()
        {
            _input.Clear();

            foreach (var button in _buttons)
            {
                var resetState = _isActivated ? Button.State.Released : Button.State.Disabled;
                if (resetState != button.state) PlayMoveSound(button.transform);
                button.state = resetState;
                button.DisableOutline();
            }
        }

        /// <summary>
        ///     Shows the hint solution on the NumPad
        /// </summary>
        private void ShowHint()
        {
            foreach (var button in _buttons)
            {
                button.state = HintSolution.Contains(button.symbol)
                    ? Button.State.Pressed
                    : Button.State.Released;
                button.DisableInteractable();
                if (button.state == Button.State.Pressed) continue;
                PlayMoveSound(button.transform);
                button.DisableOutline();
            }
        }

        /// <summary>
        ///     Plays one of the stone movement sounds at the destination transform
        /// </summary>
        /// <param name="dest">
        ///     Where the sound should be played at
        /// </param>
        private void PlayMoveSound(Transform dest)
        {
            // Play a random audio clip
            AudioSource.PlayClipAtPoint(audioClips[Random.Range(0, audioClips.Count)], dest.position, 0.5f);
        }


        /// <summary>
        ///     Inner class implementing a <see cref="SmoothLerpAnimation{T,TLerp}" /> for moving the timecore
        ///     when the puzzle is solved
        /// </summary>
        [RequireComponent(typeof(Timecore))]
        private class TimecoreAnimation : AxisMoveSmoothLerpAnimation
        {
            private bool _active; // Whether the timecore is active

            /// <summary>
            ///     Initializes the timecore;
            ///     freezes the timecore and moves it to the initial position
            /// </summary>
            protected override void Start()
            {
                base.Start();
                var timecore = GetComponent<Timecore>();
                timecore.SetFreeze(true);
                Lerp(0, -LerpPositionOffset);
            }

            /// <summary>
            ///     Enables the timecore and animates it
            /// </summary>
            public void Animate()
            {
                _active = true;
                gameObject.SetActive(true);
                Lerp(LerpDuration, 0);
            }

            /// <summary>
            ///     After the lerp animation ends, the timecore is unfrozen if it was active
            /// </summary>
            protected override void OnLerpEnd()
            {
                if (_active) GetComponent<Timecore>().SetFreeze(false);
                else gameObject.SetActive(false);
            }
        }

        /// <summary>
        ///     Inner class implementing a <see cref="SmoothLerpAnimation{T,TLerp}" /> for moving the temple top
        ///     when the puzzle is solved
        /// </summary>
        private class TopAnimation : AxisMoveSmoothLerpAnimation
        {
            public Action LerpEndAction; // The action to invoke when the lerp animation ends

            /// <summary>
            ///     Animates the temple top
            /// </summary>
            public void Animate()
            {
                Lerp(LerpDuration, LerpPositionOffset, transform.right);
            }

            /// <summary>
            ///     After the lerp animation ends, the lerp end action is invoked,
            ///     i.e. the timecore is animated
            /// </summary>
            protected override void OnLerpEnd()
            {
                gameObject.SetActive(false);
                LerpEndAction?.Invoke();
            }
        }
    }
}