using System;
using UnityEngine;
using UnityEngine.Events;

namespace BetweenTime._Scripts
{
    public interface IState{}
    
    [Serializable]
    public class State<T> : IState
    {
        private T _value;
        public UnityEvent<T> onChanged = new();

        public T value
        {
            get => _value;
            set
            {
                if (!Equals(value, _value)) onChanged?.Invoke(_value);
                _value = value;
            }
        }
    }
}