using System;
using System.Collections.Generic;
using System.Linq;
using BetweenTime._Scripts.@base;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using Random = UnityEngine.Random;

namespace BetweenTime._Scripts.maya
{
    /// <summary>
    ///     A button of the Maya puzzle's input field;
    ///     can be pressed and released to trigger an event
    /// </summary>
    public class Button : AxisMoveSmoothLerpAnimation
    {
        /// <summary>
        ///     The possible states of a button
        /// </summary>
        public enum State
        {
            Disabled, // The button is disabled and cannot be interacted with
            Pressed, // The button is currently pressed
            Released // The button is currently released
        }

        private const float Duration = 1f; // The duration of the lerp animation
        private const float DisabledValue = 0.1f; // The offset value for the disabled state
        private const float PressedValue = 0.07f; // The offset value for the pressed state
        private const float ReleasedValue = 0.02f; // The offset value for the released state

        // Memory of assigned symbols to prevent duplicates
        private static readonly List<Symbol> AssignedSymbols = new();

        private XRSimpleInteractable _interactable; // The interactable component of the button
        private Outline _outline; // The outline component of the button
        private State _state; // The current state of the button

        public Action<Button> OnButtonPressed; // Event that is invoked when the button is pressed

        /// <summary>
        ///     The symbol assigned to this button instance
        /// </summary>
        public Symbol symbol { get; private set; }

        /// <summary>
        ///     The current state of the button;
        ///     setting the state will update the interactable and outline components accordingly
        ///     and trigger the lerp animation if necessary
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Thrown when the value to set the state to is not one of the defined values
        /// </exception>
        public State state
        {
            get => _state;
            set
            {
                if (_state == value) return;
                var offset = value switch
                {
                    State.Disabled => DisabledValue,
                    State.Pressed => PressedValue,
                    State.Released => ReleasedValue,
                    _ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
                };
                _interactable.enabled = value != State.Disabled;
                _outline.enabled = value == State.Pressed || _interactable.isHovered || _interactable.isSelected;
                if (value == State.Pressed) _outline.OutlineColor = Color.green;
                Lerp(Duration, offset, transform.right);
                _state = value;
            }
        }

        /// <summary>
        ///     Initializes the button;
        ///     assigns a symbol to the button, adds an outline and interactable component,
        ///     sets up the interactable events and adds a symbol sprite to the button
        /// </summary>
        private void Awake()
        {
            symbol = GetSymbol();
            _outline = gameObject.AddComponent<Outline>();
            _outline.OutlineMode = Outline.Mode.OutlineVisible;
            _outline.enabled = false;
            var boxCollider = gameObject.AddComponent<BoxCollider>();
            _interactable = gameObject.AddComponent<XRSimpleInteractable>();
            _interactable.colliders.Clear();
            _interactable.colliders.Add(boxCollider);
            _interactable.hoverEntered.AddListener(_ =>
            {
                _outline.enabled = true;
                _outline.OutlineColor = Color.white;
            });
            _interactable.hoverExited.AddListener(_ =>
            {
                _outline.enabled = state == State.Pressed || _interactable.isSelected;
                if (state == State.Pressed) _outline.OutlineColor = Color.green;
            });
            _interactable.activated.AddListener(_ => Triggered());
            var sprite = new GameObject("Symbol", typeof(SpriteRenderer));
            sprite.transform.SetParent(transform);
            sprite.transform.localRotation = Quaternion.Euler(180, 0, 0);
            sprite.transform.localScale = Vector3.one * 0.05f;
            sprite.transform.localPosition = transform.right * -0.038f;
            sprite.GetComponent<SpriteRenderer>().sprite = symbol.GetSprite();
        }

        /// <summary>
        ///     Initializes the button;
        ///     disables the interactable component and sets the initial state to Disabled
        /// </summary>
        protected override void Start()
        {
            base.Start();
            _interactable.enabled = false;
            Lerp(0, DisabledValue, transform.right);
        }

        /// <summary>
        ///     Removes the symbol from the assigned symbols list when the button is destroyed
        ///     to prevent duplicates (probably not necessary, but just in case)
        /// </summary>
        private void OnDestroy()
        {
            AssignedSymbols.Remove(symbol);
        }

        /// <summary>
        ///     Triggers the button pressed event and enables the outline
        /// </summary>
        private void Triggered()
        {
            OnButtonPressed?.Invoke(this);
            _outline.enabled = true;
        }

        /// <summary>
        ///     Gets a symbol that has not been assigned to any button yet
        /// </summary>
        /// <returns>
        ///     The symbol to assign to the button
        /// </returns>
        /// <exception cref="InvalidOperationException">
        ///     Thrown when there are no more symbols available to assign
        /// </exception>
        private static Symbol GetSymbol()
        {
            var availableSymbols = Enum.GetValues(typeof(Symbol)).Cast<Symbol>().Where(SymbolExtension.Assignable)
                .Except(AssignedSymbols).ToList();
            if (availableSymbols.Count == 0) throw new InvalidOperationException("No more symbols available");
            var symbol = availableSymbols[Random.Range(0, availableSymbols.Count)];
            AssignedSymbols.Add(symbol);
            return symbol;
        }

        /// <summary>
        ///     Disables the interactable component of the button
        /// </summary>
        public void DisableInteractable()
        {
            _interactable.enabled = false;
        }

        /// <summary>
        ///     Disables the outline component of the button
        /// </summary>
        public void DisableOutline()
        {
            _outline.enabled = false;
        }
    }
}