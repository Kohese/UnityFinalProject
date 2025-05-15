using UnityEngine;
using Unity.Netcode;

public class CameraSwap : NetworkBehaviour
{
    [SerializeField] private GameObject firstPersonCam;
    [SerializeField] private GameObject thirdPersonCam;

    [SerializeField] private NetworkBehaviour firstPersonMovementScript;
    [SerializeField] private NetworkBehaviour thirdPersonMovementScript;

    [SerializeField] private GameObject GunHolder;

    private NetworkVariable<bool> isThirdPersonNetworkVariable = new NetworkVariable<bool>(
        false,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Owner
    );

    private void Start()
    {
        // Ensure the local state matches network on start
        ApplyCameraView(isThirdPersonNetworkVariable.Value);
    }

    public override void OnNetworkSpawn()
    {
        isThirdPersonNetworkVariable.OnValueChanged += OnThirdPersonChanged;

        // Apply initial state
        ApplyCameraView(isThirdPersonNetworkVariable.Value);
    }

    private void OnDestroy()
    {
        isThirdPersonNetworkVariable.OnValueChanged -= OnThirdPersonChanged;
    }

    private void OnThirdPersonChanged(bool oldValue, bool newValue)
    {
        ApplyCameraView(newValue);
    }

    private void Update()
    {
        if (!IsOwner) return;

        if (Input.GetKeyDown(KeyCode.C)) // press C to swap views
        {
            SetView(!isThirdPersonNetworkVariable.Value);
        }
    }

    public void SetView(bool thirdPerson)
    {
        if (!IsOwner) return;

        isThirdPersonNetworkVariable.Value = thirdPerson;
    }

    private void ApplyCameraView(bool thirdPerson)
    {

        // Enable/disable camera control scripts
        PlayerCam playerCamScript = firstPersonCam.GetComponent<PlayerCam>();
        ThirdPersonCam thirdPersonCamScript = firstPersonCam.GetComponentInParent<ThirdPersonCam>();

        if (playerCamScript != null) playerCamScript.enabled = !thirdPerson;
        if (thirdPersonCamScript != null) thirdPersonCamScript.enabled = thirdPerson;

        if (thirdPerson) GunHolder.transform.SetParent(this.gameObject.transform);
        else GunHolder.transform.SetParent(firstPersonCam.transform);

        // Sync gun holder visuals or position
        // if (GunHolder != null)
        // {
        //     GunHolder.SetActive(!thirdPerson); // hide gun in third person
        // }
    }
}
