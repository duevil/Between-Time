using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit.Inputs;

namespace BetweenTime._Scripts
{
    /// <summary>
    ///     Class that allows for debugging and testing of the game using a very rudimentary custom console UI
    /// </summary>
    public class DebugController : MonoBehaviour
    {
        [Tooltip("The commands that can be entered in the console")] [SerializeField]
        private Command[] commands;

        [Tooltip("The input action manager to be blocked when the debug console is active")] [SerializeField]
        private InputActionManager xrInputActionManager;

        private int _command; // The index of the current command
        private string _consoleInput = ""; // The current input in the console
        private bool _showConsole; // Whether the console should be displayed


        /// <see cref="MonoBehaviour" />
        /// OnQUI method
        /// <summary>
        ///     Draws the console UI
        /// </summary>
        private void OnGUI()
        {
            if (!_showConsole) return;
            GUI.Box(new Rect(0, 0, Screen.width, 30), "");
            GUI.backgroundColor = new Color(0, 0, 0, 0);
            var cmd = commands[_command];
            // Visualize whether the command accepts parameters
            var lbl = new GUIContent($"> {cmd.prompt}{(cmd.noParams ? "" : ":")} ");
            var lblW = GUI.skin.label.CalcSize(lbl).x;
            GUI.Label(new Rect(10, 5, lblW, 20), lbl);
            // Don't draw the input field if the command doesn't take parameters
            if (cmd.noParams) return;
            GUI.SetNextControlName("ConsoleInput");
            _consoleInput = GUI.TextField(new Rect(10 + lblW, 5, Screen.width - lblW - 10, 20), _consoleInput);
            GUI.FocusControl("ConsoleInput");
        }


        /// <summary>
        ///     Method that is called when the ToggleConsole input action is received
        /// </summary>
        private void OnToggleConsole()
        {
            _showConsole = !_showConsole;
            xrInputActionManager.enabled = !_showConsole;
        }

        /// <summary>
        ///     Method that is called when the Return input action is received
        /// </summary>
        private void OnReturn()
        {
            if (!_showConsole) return;
            commands[_command].action?.Invoke(_consoleInput);
            _consoleInput = "";
        }

        /// <summary>
        ///     Method that is called when the Tab input action is received
        /// </summary>
        private void OnTab()
        {
            if (!_showConsole) return;
            _command = (_command + 1) % commands.Length;
            _consoleInput = "";
        }


        /// <summary>
        ///     Struct that represents a console command
        /// </summary>
        [Serializable]
        public struct Command
        {
            [Tooltip("The prompt that will be displayed in the console")]
            public string prompt;

            [Tooltip("Whether the command uses parameters")]
            public bool noParams;

            [Tooltip("The action(s) to be executed when the command is entered")]
            public UnityEvent<string> action;

            // Constructor for defaulting to the command having parameters
            public Command(string prompt, UnityEvent<string> action)
            {
                this.prompt = prompt;
                this.action = action;
                noParams = false;
            }
        }
    }
}