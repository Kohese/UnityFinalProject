using System.Collections.Generic;
using UnityEngine;
namespace BuildDebugging.DebugConsole {
    public class DebugConsole : MonoBehaviour {
        
        private static bool showDebugConsole = false;
        private static readonly List<string> debugMessages = new List<string>();
        private static Vector2 debugConsoleScroll;
        private const int MaxDebugMessagesInConsole = 10;

        [Tooltip("Keep object alive through scene changes")]
        [SerializeField] private bool persist = true;
        [Tooltip("The key that is used to toggle showing the debug console")]
        [SerializeField] private KeyCode toggleKey = KeyCode.BackQuote;
        [Tooltip("The height of the debug console in pixels")]
        [SerializeField] private float debugConsoleHeight = 200f;
        
        private void Awake() {
            if (persist) {
                DontDestroyOnLoad(this);
            }
        }
        
        private void OnEnable() => Application.logMessageReceived += OnDebugReceived;
        private void OnDisable() => Application.logMessageReceived -= OnDebugReceived;
        
        /// <summary>
        /// Log to debug console without using Debug.Log
        /// </summary>
        /// <param name="content">The debug message</param>
        /// <param name="type">The type of log (Error, Assert, Warning, Log, Exception)</param>
        public static void Log(string content, LogType type) => OnDebugReceived(content, "", type);
        
        public static void ToggleShowDebugConsole() => showDebugConsole = !showDebugConsole;

        private static void OnDebugReceived(string content, string _, LogType type) {
            // DO NOT DEBUG LOG FROM THIS METHOD AS A INFINITE LOOP WILL OCCUR
            debugMessages.Add("[" + type + "] " + content);
            if (debugMessages.Count > MaxDebugMessagesInConsole) {
                debugMessages.Remove(debugMessages[debugMessages.Count - (MaxDebugMessagesInConsole + 1)]);
            }
        }

        private void OnGUI() {
            if (!showDebugConsole) return;
            GUI.Box(new Rect(0, 0, Screen.width, debugConsoleHeight),"");
            debugConsoleScroll = GUI.BeginScrollView(new Rect(0, 5f, Screen.width, debugConsoleHeight - 10), debugConsoleScroll, new Rect(0, 0, Screen.width - 30, 20 * debugMessages.Count));

            for (var i = 0; i < debugMessages.Count; i++) {
                GUI.Label(new Rect(5, 20 * i, Screen.width - 130, 20), debugMessages[i]);
            }
            GUI.EndScrollView();
        }

        private void Update() {
            if (Input.GetKeyDown(toggleKey)) {
                ToggleShowDebugConsole();
            }
        }
    }
}