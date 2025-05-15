// using System.Collections;
// using Unity.VisualScripting;
// using UnityEngine;

// namespace WeaponStrategyPattern
// {
//     public class WeaponPistol : WeaponBase    // extention of weapon interface
//     {
//         private void Awake()    // initalize variables
//         {
//             totalAmmo = 100;
//             currentAmmo = totalAmmo;
//             damage = 0;
//             totalReloadTime = 3;
//             bulletSpread = 0.01f;
//             Initalize();
//         }

//         private void Update()   // code for shooting weapon
//         {
            
//             if (Input.GetMouseButtonDown(0) && (isReloading == false))          // can attempt shooting when not reloading
//             {
//                 if(currentAmmo > 0){    // fires weapon if there are bullets
//                     ShootWeapon();
//                     currentAmmo--;
//                     Debug.Log($"Pistol PEW PEW");
//                 } else {
//                     Debug.Log("Empty clip");
//                 }
//             }
            
//             if (Input.GetKeyDown("r") && (currentAmmo < totalAmmo)) // enter reloading state
//             {isReloading = true;    Debug.Log("RELOADING");}

//             if (isReloading == true)    // reload function active when in reloading state
//             {Reload();}
            
//         }

//         public override void ShootWeapon ()
//         {
//             float xOffset = Random.Range(-bulletSpread, bulletSpread);  // calculate bullet spread
//             float yOffset = Random.Range(-bulletSpread, bulletSpread);

//             Vector3 point = new Vector3(cam.pixelWidth/2 + cam.pixelWidth*xOffset, cam.pixelHeight/2 + cam.pixelHeight*yOffset, 0); // set raycast point destination to center of screen + offset
//             Ray ray = cam.ScreenPointToRay(point);  // set target position that raycast will hit
//             RaycastHit hit;                         // create raycast
//             if (Physics.Raycast(ray, out hit)) {    // logic when raycast hits something
//                 StartCoroutine(TestHit(hit.point));
//             }
//         }

//         private void OnGUI()
//         {
//             int size = 24;
//             float posX = cam.pixelWidth/2 - size/4;
//             float posY = cam.pixelHeight/2 - size/4;
//             GUI.Label(new Rect(posX,posY, size, size), "+");
//         }


//     }














//     // public class WeaponPistol : MonoBehaviour, IWeapon    // extention of weapon interface
//     // {
//     //     public Camera cam;
//     //     public int totalAmmo = 10;   // initalize variables
//     //     public int currentAmmo;
//     //     public int damage = 0;
//     //     public float totalReloadTime = 3;
//     //     public float currentReloadTime = 0; 
//     //     public bool isReloading;
    
//     //     public void Initalize(PlayerShoot player)
//     //     {
//     //         cam = player.GetComponent<Camera>();
//     //         currentAmmo = totalAmmo;
//     //         isReloading = false;
//     //         Debug.Log($"Ammo: {currentAmmo}/{totalAmmo}");
//     //     }
//     //     public void Active()   // code for shooting weapon
//     //     {
            
//     //         if (Input.GetMouseButtonDown(0) && (isReloading == false))
//     //         {
//     //             if(currentAmmo > 0){
//     //                 ShootWeapon();
//     //                 currentAmmo--;
//     //                 Debug.Log($"Pistol PEW PEW {currentAmmo}/{totalAmmo}");
//     //             } else {
//     //                 Debug.Log("Empty clip");
//     //             }
//     //         }
            
//     //         if (Input.GetKeyDown("r") && (currentAmmo < totalAmmo))
//     //         {isReloading = true;    Debug.Log("RELOADING");}

//     //         if (isReloading == true)
//     //         {Reload();}
            
            
//     //     }

//     //     public void ShootWeapon ()
//     //     {
//     //         Vector3 point = new Vector3(cam.pixelWidth/2, cam.pixelHeight/2, 0);
//     //         Ray ray = cam.ScreenPointToRay(point);
//     //         RaycastHit hit;
//     //         if (Physics.Raycast(ray, out hit)) {
//     //             StartCoroutine(sphereIndicator(hit.point));
//     //         }
//     //     }


//     //     private IEnumerator sphereIndicator(Vector3 pos)
//     //     {
//     //         GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
//     //         sphere.transform.position = pos;
//     //         sphere.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
//     //         yield return new WaitForSeconds(1);
//     //         Destroy(sphere);
//     //     }

//     //     public void Reload()
//     //     {
//     //         currentReloadTime += Time.deltaTime;
//     //         if(currentReloadTime > totalReloadTime)
//     //         {
//     //             currentAmmo = totalAmmo;
//     //             currentReloadTime = 0;
//     //             isReloading = false;
//     //             Debug.Log("Reload complete!");
//     //         }
//     //     }
        
//     // }
// }

