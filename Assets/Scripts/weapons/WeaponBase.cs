using UnityEngine;
using System.Collections;
using Unity.Netcode;

namespace WeaponStrategyPattern     // Namespace for code related to the guns
{
    public abstract class WeaponBase : NetworkBehaviour   // Interface that all weapon shooting code will use
        {

        [SerializeField] 
        public Camera cam;
        public int totalAmmo;   // initalize variables
        public int currentAmmo;
        public float damage;
        public float  bulletSpread;
        public float totalReloadTime;
        public float currentReloadTime = 0; 
        public bool isReloading;
        private WeaponTiers weaponTiers;
        public PlayerMovement player;

        // probably need idk yet
        protected ulong ownerRef;

        // needs to be initialized for weapon to be able to shoot
        protected PlayerNetwork playerNetwork;
    
        public void Initalize()
        {
            cam = GetComponentInChildren<Camera>();
            player = GetComponent<PlayerMovement>();
            weaponTiers = GetComponent<WeaponTiers>();
            currentAmmo = totalAmmo;
            isReloading = false;
            Debug.Log($"Ammo: {currentAmmo}/{totalAmmo}");
            playerNetwork = GetComponent<PlayerNetwork>();
        }
        public virtual void ShootWeapon ()
        {
        
        }
        
        public void Reload()
        {
            currentReloadTime += Time.deltaTime;
            if(currentReloadTime > totalReloadTime)
            {
                currentAmmo = totalAmmo;
                currentReloadTime = 0;
                isReloading = false;
                Debug.Log("Reload complete!");
            }
        }

        public void unequip()
        {
            Destroy(this);
        }



        public IEnumerator TestHit(Vector3 pos)
        {
            // GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            // sphere.transform.position = pos;
            // sphere.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
            // yield return new WaitForSeconds(1);
            // Destroy(sphere);

                GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
    sphere.transform.position = transform.position + transform.forward; // start near player
    sphere.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

    float speed = 30f;
    float distance = Vector3.Distance(sphere.transform.position, pos);
    float travelTime = distance / speed;

    float elapsed = 0f;
    Vector3 start = sphere.transform.position;

    while (elapsed < travelTime)
    {
        sphere.transform.position = Vector3.Lerp(start, pos, elapsed / travelTime);
        elapsed += Time.deltaTime;
        yield return null;
    }

    sphere.transform.position = pos;
    yield return new WaitForSeconds(1);
    Destroy(sphere);
        }
    }

}


// namespace WeaponStrategyPattern     // Namespace for code related to the guns
// {
//     public interface IWeapon   // Interface that all weapon shooting code will use
//         {
//             void Active();
//             void ShootWeapon();
//             void Reload();
//             void Initalize(PlayerShoot player);
//         }

// }
