using System.Diagnostics;
using UnityEngine;
namespace BuildDebugging.FileLogging {
    public class LogUnityEngineDebugs : MonoBehaviour {
        [Tooltip("Keep object alive through scene changes")]
        [SerializeField] private bool persist = true;
        [Tooltip("Record the steps leading up to the log")]
        [SerializeField] private bool recordStacktrace = true;
        private void Awake() {
            if (persist) {
                DontDestroyOnLoad(this);
            }
        }

        private void OnEnable() => Application.logMessageReceived += OnDebugReceived;
        private void OnDisable() => Application.logMessageReceived -= OnDebugReceived;

        private void OnDebugReceived(string content, string _, LogType type) {
            // DO NOT DEBUG LOG FROM THIS METHOD AS A INFINITE LOOP WILL OCCUR
            var trace = recordStacktrace? "\n" + new StackTrace().ToString() + "\n" : null;
            LogToFile.LogDebugToFile(content, trace, type);
        }
    }
}