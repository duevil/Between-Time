using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using BetweenTime._Scripts.@base;
using UnityEngine;
using UnityEngine.Events;
using uPLibrary.Networking.M2Mqtt;

namespace BetweenTime._Scripts
{
    /// <summary>
    ///     Interface for a MQTT state
    /// </summary>
    internal interface IState
    {
        void SetupMqtt(MqttClient client);
        void PublishValue(MqttClient client);
        void Reset();
    }

    /// <summary>
    ///     Class for representing a state to be transmitted over MQTT
    /// </summary>
    /// <p>
    ///     For MQTT communication details, see <see cref="SetupMqtt" />
    /// </p>
    /// <typeparam name="T">The type of value to represent; must have a parameterless/default constructor</typeparam>
    /// <typeparam name="TParser">
    ///     The parser to use for converting values to and from strings;
    ///     must implement <see cref="IParser{T}" /> and have a parameterless constructor
    /// </typeparam>
    [Serializable]
    public class State<T, TParser> : IState where T : new() where TParser : IParser<T>, new()
    {
        [Tooltip("The topic to use for MQTT communication")] [SerializeField]
        private string topic;

        [Tooltip("The current value of the state")] [ReadOnly] [SerializeField]
        private string value; // string representation of the value

        [Tooltip("Event that is invoked when the value changes")]
        public UnityEvent<T> onChange = new();

        // The comparer to use for checking value equality
        private IEqualityComparer<T> _comparer = EqualityComparer<T>.Default;
        private TParser _parser = new(); // The parser to use for converting values to and from strings

        [NotNull] private T _value = new(); // Underlying field for the Value property; never null

        /// <summary>
        ///     The current value of the state
        /// </summary>
        public T Value
        {
            get => _value;
            set
            {
                // Only update the value and invoke the onChange event if the value has changed
                if (value is null || _comparer.Equals(_value, value)) return;
                // Enqueue the onChange event to run on the main thread
                MainThreadInvoker.Enqueue(() => onChange?.Invoke(value));
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
            onChange.AddListener(_ => PublishValue(client));
        }

        /// <summary>
        ///     Publishes the current value to the topic
        /// </summary>
        /// <param name="client">The MQTT client to use for communication</param>
        public void PublishValue(MqttClient client)
        {
            value = _parser.To(_value);
            var message = Encoding.UTF8.GetBytes(value);
            Debug.Log($"Publishing message on topic {topic}: {value}");
            client.Publish(topic, message, 0, true);
        }

        /// <summary>
        ///     Resets the state to its default value
        /// </summary>
        public void Reset()
        {
            Value = new T();
        }
    }
}