using UnityEngine;
namespace BuildDebugging.FileLogging {
    public class OpenSessionLogs : MonoBehaviour {
        
        /// <summary>
        /// Opens the current session logs in the default program for .txt files
        /// </summary>
        /// <param name="session">Opens the logs of another session instead of the current one</param>
        public void OpenLogs(int session = -1) {
            if (session == -1) {
                System.Diagnostics.Process.Start(LogToFile.FilePath);
            } else {
                System.Diagnostics.Process.Start(Application.persistentDataPath + "/SessionLogs_" + session + ".txt");
            }
        }
    }
}