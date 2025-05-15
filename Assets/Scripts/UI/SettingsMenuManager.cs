using UnityEngine;
using UnityEngine.UI;

public class SettingsMenuManager : MonoBehaviour
{
    public GameObject settingsPanel;
    public Slider sensitivitySlider;
    public ThirdPersonCam playerCam;

    private bool isMenuOpen = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        settingsPanel.SetActive(false);

        sensitivitySlider.onValueChanged.AddListener(OnSensitivityChanged);

        if (playerCam != null)
            sensitivitySlider.value = playerCam.rotationSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleSettingsMenu();
        }
    }

    private void ToggleSettingsMenu()
    {
        isMenuOpen = !isMenuOpen;
        settingsPanel.SetActive(isMenuOpen);

        Cursor.lockState = isMenuOpen ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = isMenuOpen;
    }

    private void OnSensitivityChanged(float value)
    {
        if (playerCam != null)
        {
            playerCam.SetSensitivity(value);
        }
    }
}
