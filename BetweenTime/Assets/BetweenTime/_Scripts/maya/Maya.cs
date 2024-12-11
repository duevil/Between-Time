using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using BetweenTime._Scripts.@base;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

// manages the atztec puzzle
namespace BetweenTime._Scripts.maya
{
    public class Maya : MonoBehaviour
    {
        private const ushort Timecode = 0xdc3e;
        private const float LerpDuration = 2f;
        private const float LerpPositionOffset = 1f;


        private static readonly ReadOnlyCollection<Symbol> Solution = new(new List<Symbol>
        {
            Symbol._4, Symbol._8, Symbol._5, Symbol._3, Symbol._6, Symbol._1
        }); // 485361

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

        private Button[] _buttons;
        private bool _isActivated;
        private bool _isSolved;
        private int _stepCount;
        private Step[] _steps;
        private TimecoreAnimation _timecoreAnimation;
        private TopAnimation _topAnimation;


        private void Awake()
        {
            _buttons = GetComponentsInChildren<Button>();
            _steps = GetComponentsInChildren<Step>();

            foreach (var button in _buttons) button.OnButtonPressed = HandleButtonPressed;
        }

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

        public void HandleTimeCode(ushort value)
        {
            if (_isSolved) return;
            ResetNumPad();
        }

        public void HandleMainState(MainState value)
        {
            switch (value)
            {
                case MainState.InputFieldSolved when !_isSolved:
                    Finish();
                    break;
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

                    IEnumerator Routine()
                    {
                        yield return new WaitUntil(() => _stepCount == 0);
                        foreach (var button in _buttons)
                        {
                            button.state = Button.State.Released;
                            PlayMoveSound(button.transform);
                        }
                    }
            }
        }

        private void HandleButtonPressed(Button button)
        {
            PlayMoveSound(button.transform);

            var gameController = GameController.Instance;
            if (gameController.timecodeState.Value == Timecode)
            {
                button.state = Button.State.Pressed;
                _input.Add(button.symbol);

                if (_input.Count != Solution.Count) return;
                if (Solution.SequenceEqual(_input)) gameController.mainState.Value = MainState.InputFieldSolved;
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

        private void PlayMoveSound(Transform dest)
        {
            // Play a random audio clip
            AudioSource.PlayClipAtPoint(audioClips[Random.Range(0, audioClips.Count)], dest.position, 0.5f);
        }


        [RequireComponent(typeof(Timecore))]
        private class TimecoreAnimation : AxisMoveSmoothLerpAnimation
        {
            private bool _active;

            protected override void Start()
            {
                base.Start();
                var timecore = GetComponent<Timecore>();
                timecore.SetFreeze(true);
                Lerp(0, -LerpPositionOffset);
            }

            public void Animate()
            {
                _active = true;
                gameObject.SetActive(true);
                Lerp(LerpDuration, 0);
            }

            protected override void OnLerpEnd()
            {
                if (_active) GetComponent<Timecore>().SetFreeze(false);
                else gameObject.SetActive(false);
            }
        }

        private class TopAnimation : AxisMoveSmoothLerpAnimation
        {
            public Action LerpEndAction;

            public void Animate()
            {
                Lerp(LerpDuration, LerpPositionOffset, transform.right);
            }

            protected override void OnLerpEnd()
            {
                gameObject.SetActive(false);
                LerpEndAction?.Invoke();
            }
        }
    }
}