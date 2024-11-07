using System;
using System.Collections.Generic;
using System.Text;
using BetweenTime._Scripts.@base;
using UnityEngine;
using UnityEngine.Events;
using uPLibrary.Networking.M2Mqtt;

namespace BetweenTime._Scripts
{
    /// <summary>
    ///     Class for representing a readonly state to be transmitted over MQTT
    /// </summary>
    /// <p>
    ///     For MQTT communication details, see <see cref="SetupMqtt" />
    /// </p>
    /// <typeparam name="T">The type of value to represent</typeparam>
    /// <typeparam name="TParser">
    ///     The parser to use for converting values to and from strings;
    ///     must implement <see cref="IParser{T}" /> and have a parameterless constructor
    /// </typeparam>
    /// <seealso cref="MutableState{T,TParser}" />
    [Serializable]
    public class State<T, TParser> where TParser : IParser<T>, new()
    {
        [Tooltip("The topic to use for MQTT communication")]
        public string topic;

        [Tooltip("The current value of the state")] [ReadOnly] [SerializeField]
        private string value = "NAN"; // primarily used for visualizing the value in the inspector

        [Tooltip("Event that is invoked when the value changes")]
        public UnityEvent<T> onChange = new();

        // The comparer to use for checking value equality
        private IEqualityComparer<T> _comparer = EqualityComparer<T>.Default;
        private TParser _parser = new(); // The parser to use for converting values to and from strings

        private T _value; // Underlying field for the Value property

        /// <summary>
        ///     The current value of the state; read-only
        /// </summary>
        public T Value
        {
            get => _value;
            private protected set
            {
                // Only update the value and invoke the onChange event if the value has changed
                if (value is null || _comparer.Equals(_value, value)) return;
                this.value = _parser.To(value);
                onChange.Invoke(value);
                _value = value;
            }
        }

        /// <summary>
        ///     Sets up this state's topic for MQTT communication;
        ///     subscribes to the topic for updating its value and publishes changes to the topic
        /// </summary>
        /// <param name="client">The MQTT client to use for communication</param>
        public void SetupMqtt(MqttClient client)
        {
            // Subscribe to the topic to receive updates
            client.Subscribe(new[] { topic }, new byte[] { 0 });
            // Add a listener to the client's message received event to update the value
            client.MqttMsgPublishReceived += (_, args) =>
            {
                if (args.Topic != topic) return;
                var v = Encoding.UTF8.GetString(args.Message);
                Debug.Log($"Received message on topic {args.Topic}: {v}");
                Value = _parser.From(v);
            };
            // Add a listener to the onChange event to publish the new value to the topic
            onChange.AddListener(_ =>
            {
                var message = Encoding.UTF8.GetBytes(value);
                Debug.Log($"Publishing message on topic {topic}: {value}");
                client.Publish(topic, message, 0, true);
            });
        }
    }


    /// <summary>
    ///     Class for representing a mutable state to be transmitted over MQTT
    /// </summary>
    /// <typeparam name="T">The type of value to represent</typeparam>
    /// <typeparam name="TParser">
    ///     The parser to use for converting values to and from strings;
    ///     must implement <see cref="IParser{T}" /> and have a parameterless constructor
    /// </typeparam>
    /// <seealso cref="State{T,TParser}" />
    [Serializable]
    public class MutableState<T, TParser> : State<T, TParser> where TParser : IParser<T>, new()
    {
        /// <summary>
        ///     The current value of the state; can be set
        /// </summary>
        public new T Value
        {
            get => base.Value;
            set => base.Value = value;
        }
    }
}