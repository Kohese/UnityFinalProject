using System.IO;
using UnityEngine;
namespace BuildDebugging.FileLogging {
    public class LogToFile : MonoBehaviour {

        internal static string FilePath = null;
        
        /// <summary>
        /// Log to file without using Debug.Log, good for logging things like in game chat
        /// </summary>
        /// <param name="content">The logs message</param>
        /// <param name="type">The type of log (Error, Assert, Warning, Log, Exception)</param>
        public static void Log(string content, LogType type) {
            LogDebugToFile(content, null, type);
        }

        /// <summary>
        /// Log a message to the sessions logs
        /// </summary>
        /// <param name="content">The main content of the log</param>
        /// <param name="stacktrace">The list of methods the program ran when the log occured</param>
        /// <param name="type">The type of debug (Error, Assert, Warning, Log, Exception)</param>
        internal static void LogDebugToFile(string content, string stacktrace, LogType type) {
            // DO NOT DEBUG LOG FROM THIS METHOD AS A INFINITE LOOP WILL OCCUR
            FilePath ??= GetFilePath();
            var writer = new StreamWriter(FilePath, true);
            
            writer.WriteLine("[" + type +"] at [" + System.DateTime.Now + "] " + content);
            if (stacktrace != null) {
                writer.WriteLine("[Stacktrace] " + stacktrace);
            }
            
            writer.Close();
        }
        
        /// <returns>The file path of the sessions logs</returns>
        private static string GetFilePath() {
            var sessionCount = PlayerPrefs.GetInt("BetterDebug.SessionCount", -1) + 1;
            PlayerPrefs.SetInt("BetterDebug.SessionCount", sessionCount);
            return Application.persistentDataPath + "/SessionLogs_" + sessionCount + ".txt";
        }
        
    }
}