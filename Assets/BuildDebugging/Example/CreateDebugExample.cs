using UnityEngine;
namespace BuildDebugging.Example {
    public class CreateDebugExample : MonoBehaviour {
        private void Update() {
            if (Input.GetKeyDown(KeyCode.Space)) {
                Debug.Log("Test debug by pressing Space");
            }
            if (Input.GetKeyDown(KeyCode.LeftShift)) {
                Debug.LogError("Test error by pressing Left Shift");
            }
            if (Input.GetKeyDown(KeyCode.LeftAlt)) {
                Debug.LogWarning("Test warning by pressing Left Alt");
            }
        }
    }
}