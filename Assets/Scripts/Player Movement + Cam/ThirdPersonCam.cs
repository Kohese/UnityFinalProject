using UnityEngine;
using Unity.Netcode;

public class ThirdPersonCam : NetworkBehaviour
{
    [Range(0.1f, 20f)]

    public Transform target;
    public Vector3 offset = new Vector3(0, 3, -6);
    public float rotationSpeed = 5f;
    public float followSpeed = 10f;
    public bool isActive;


    public Transform orientation;
    public Transform playerBody; 

    private float yaw = 0f;
    private float pitch = 10f;
    public float pitchMin = -20f;
    public float pitchMax = 80f;

    private NetworkVariable<float> syncedYRotation = new NetworkVariable<float>(
    0f, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    private NetworkVariable<float> syncedXRotation = new NetworkVariable<float>(
    0f, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);


    public void SetSensitivity(float value)
    {
        rotationSpeed = value;
    }
        void Start()
    {
                // if (!isActive) return;
        if (!IsOwner)
        {
            // Disable this camera + audio listener if not owned by this client
            Transform child = GetComponentInChildren<Camera>().transform;
            child.GetComponent<Camera>().enabled = false;

            var listener = child.GetComponent<AudioListener>();
            if (listener != null)
                listener.enabled = false;

            // Disable this script if not owner
            // enabled = false;
            return;
        }
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        // if (!isActive) return;
        // if (!IsOwner)
        // {
        //     // Disable this camera + audio listener if not owned by this client
        //     Debug.Log(transform.GetChild(0));
        //     // Transform cam = transform.Find("ThirdPersonCam");
        //     // cam.GetComponent<Camera>().enabled = false;
        //     // cam.GetComponent<Camera>.enabled = false;
        //     // Debug.Log(cam);
        //     // transform.Find("ThirdPersonCam").GetComponent<Camera>.enabled = false;
        //     // GameObject.GetChild(0).enabled = false;
        //     // GetComponent<Camera>().enabled = false;

        //     var listener = cam.GetComponent<AudioListener>();
        //     if (listener != null)
        //         listener.enabled = false;

        //     // Disable this script if not owner
        //     // enabled = false;
        //     return;
        // }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    private void Update()
    {
        // if (!isActive) return;
        //To rotate the camera with the mouse
        yaw += Input.GetAxis("Mouse X") * rotationSpeed;
        pitch -= Input.GetAxis("Mouse Y") * rotationSpeed;
        pitch = Mathf.Clamp(pitch, pitchMin, pitchMax);
    }

    private void LateUpdate()
    {

        if (!target) return;

        if (IsOwner)
        {

        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);

          syncedYRotation.Value = yaw;
        syncedXRotation.Value = pitch;


        //Old Rotation line
        //Vector3 desiredPosition = target.position + rotation * offset;

        Vector3 rotatedOffset = rotation * new Vector3(0, offset.y, offset.z);
        Vector3 sideOffset = transform.right * offset.x;

        Vector3 desiredPosition = target.position + rotatedOffset + sideOffset;

        transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);

        //Old clamping (doesn't work)
        //transform.LookAt(target.position + Vector3.up * 1.5f);

        transform.localRotation = Quaternion.Euler(pitch, 0, 0);
        orientation.rotation = Quaternion.Euler(0, yaw, 0);
        playerBody.rotation = Quaternion.Euler(0, yaw, 0);

        if (orientation != null)
        {
            Vector3 flatForward = transform.forward;
            flatForward.y = 0f;
            orientation.forward = flatForward.normalized;
        }

        }
        else
        {
                        // do the same for non owners of the script so they can see it
            transform.localRotation = Quaternion.Euler(syncedXRotation.Value, 0, 0);
            playerBody.rotation = Quaternion.Euler(0, syncedYRotation.Value, 0);
            orientation.rotation = Quaternion.Euler(0, syncedYRotation.Value, 0);
        }
    }
}
