using System;
using System.Collections.Generic;
using System.Linq;
using BetweenTime._Scripts.@base;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using Random = UnityEngine.Random;

namespace BetweenTime._Scripts.maya
{
    public class Button : AxisMoveSmoothLerpAnimation
    {
        public enum State
        {
            Disabled,
            Pressed,
            Released
        }

        private const float Duration = 1f;
        private const float DisabledValue = 0.1f;
        private const float PressedValue = 0.07f;
        private const float ReleasedValue = 0.02f;

        private static readonly List<Symbol> AssignedSymbols = new();

        private XRSimpleInteractable _interactable;
        private Outline _outline;
        private State _state;

        public Action<Button> OnButtonPressed;

        public Symbol symbol { get; private set; }

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

        protected override void Start()
        {
            base.Start();
            _interactable.enabled = false;
            Lerp(0, DisabledValue, transform.right);
        }

        private void OnDestroy()
        {
            AssignedSymbols.Remove(symbol);
        }

        private void Triggered()
        {
            OnButtonPressed?.Invoke(this);
            _outline.enabled = true;
        }

        private static Symbol GetSymbol()
        {
            var availableSymbols = Enum.GetValues(typeof(Symbol)).Cast<Symbol>().Where(SymbolExtension.Assignable)
                .Except(AssignedSymbols).ToList();
            if (availableSymbols.Count == 0) throw new InvalidOperationException("No more symbols available");
            var symbol = availableSymbols[Random.Range(0, availableSymbols.Count)];
            AssignedSymbols.Add(symbol);
            return symbol;
        }

        public void DisableInteractable()
        {
            _interactable.enabled = false;
        }

        public void DisableOutline()
        {
            _outline.enabled = false;
        }
    }
}