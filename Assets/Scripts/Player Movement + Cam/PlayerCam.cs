using UnityEngine;
using Unity.Netcode;

public class PlayerCam : NetworkBehaviour
{
    public float sensX;
    public float sensY;
    public bool isActive = true;

    public Transform orientation;
    public Transform playerBody; 
    public Transform gunHolder; 
    public Transform headPivot; // 🔧 This will rotate up/down

    public float mouseX;
    public float mouseY;

    float xRotation;
    float yRotation;

    // private NetworkVariable<float> syncedXRotation = new NetworkVariable<float>(
    // 0f, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    private NetworkVariable<float> syncedYRotation = new NetworkVariable<float>(
    0f, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    private NetworkVariable<float> syncedXRotation = new NetworkVariable<float>(
    0f, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public PlayerMovement player;

    void Awake()
    {
        player = playerBody.GetComponent<PlayerMovement>();       
    }
    void Start()
    {
        // if (!isActive) return;
        if (!IsOwner)
        {
            // Disable this camera + audio listener if not owned by this client
            GetComponent<Camera>().enabled = false;

            var listener = GetComponent<AudioListener>();
            if (listener != null)
                listener.enabled = false;

            // Disable this script if not owner
            // enabled = false;
            return;
        }
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnEnable()
    {
        transform.parent.localPosition = Vector3.zero;
    }

    // private void Update()
    // {
    //         // mouseX = Input.GetAxisRaw("Mouse X") * sensX * Time.deltaTime;
    //         // mouseY = Input.GetAxisRaw("Mouse Y") * sensY * Time.deltaTime;

    //         // // Update rotation values
    //         // yRotation += mouseX;
    //         // xRotation -= mouseY;
    //         // xRotation = Mathf.Clamp(xRotation, -60f, 60f);
    // }

    private void Update()
    {
        // transform.parent.localPosition = Vector3.zero;
        // if (isActive == false) return;
        // if (!IsOwner) return;
        if (IsOwner)
        {
            float mouseX = Input.GetAxisRaw("Mouse X") * sensX * Time.deltaTime;
            float mouseY = Input.GetAxisRaw("Mouse Y") * sensY * Time.deltaTime;

            // Update rotation values
            yRotation += mouseX;
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -60f, 60f);

            // Sync values
            syncedYRotation.Value = yRotation;
            syncedXRotation.Value = xRotation;

            if (player.isAlive.Value)   // only rotate when alive
            {
                // Rotate camera up/down - needs to be localrotation
                transform.localRotation = Quaternion.Euler(xRotation, 0, 0);

                // rotate camera left/right - needs to be rotation i think
                orientation.rotation = Quaternion.Euler(0, yRotation, 0);
                // rotate player
                playerBody.rotation = Quaternion.Euler(0, yRotation, 0);  
            }

            
            
        }
        else
        {
            // do the same for non owners of the script so they can see it
            
            if (player.isAlive.Value)
            {
                transform.localRotation = Quaternion.Euler(syncedXRotation.Value, 0, 0);
                playerBody.rotation = Quaternion.Euler(0, syncedYRotation.Value, 0);
                orientation.rotation = Quaternion.Euler(0, syncedYRotation.Value, 0);
            }
            
            
        }
    }
}

// old player cam script if needed
// ---------------------------------------------------------------
// using Unity.Netcode;

// public class PlayerCam : NetworkBehaviour
// {
// public float sensX;
// public float sensY;

// ```
// public Transform orientation;
// public Transform playerBody; 

// float xRotation;
// float yRotation;

// void Start()
// {
//     if (!IsOwner)
//     {
//         // Disable this camera + audio listener if not owned by this client
//         GetComponent<Camera>().enabled = false;

//         var listener = GetComponent<AudioListener>();
//         if (listener != null)
//             listener.enabled = false;

//         // Disable this script if not owner
//         enabled = false;
//         return;
//     }

//     Cursor.lockState = CursorLockMode.Locked;
//     Cursor.visible = false;
// }

// void Update()
// {
//     if (!IsOwner) return;
//     float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
//     float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

//     yRotation += mouseX;
//     xRotation -= mouseY;
//     xRotation = Mathf.Clamp(xRotation, -90f, 90f);

//     // Rotate camera up/down
//     transform.localRotation = Quaternion.Euler(xRotation, 0, 0);

//     // Rotate player
//     playerBody.rotation = Quaternion.Euler(0, yRotation, 0);

//     orientation.rotation = Quaternion.Euler(0, yRotation, 0);
// }
// }
