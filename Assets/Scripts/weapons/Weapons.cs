// Base weapon class is assumed to exist: WeaponBase

// weapon class that can be used to assign the script to the player depending on which gun they are on
// currently uses 1-5 keys to swap the guns
// uses server rpc in playernetwork script to tell server to shoot the gun for the clients


using UnityEngine;
using Unity.Netcode;
using System.Collections;

namespace WeaponStrategyPattern
{
    public class WeaponShotgun : WeaponBase
    {
        public int pelletsPerShot = 8;

        public override void OnNetworkSpawn()
        {
            if (!IsOwner) return;
        }

        private void Awake()
        {
            totalAmmo = 8;
            currentAmmo = totalAmmo;
            damage = 32f;
            totalReloadTime = 2.5f;
            // bulletSpread = 0.05f;
            bulletSpread = 12f;
        }

        public void Initalize()
        {
            base.Initalize();
        }

        private void Update()
        {
            if (ownerRef == null || ownerRef != OwnerClientId) return;

            if (player.isAlive.Value)
            {
                if (Input.GetMouseButtonDown(0) && !isReloading)
                {
                    if (currentAmmo > 0)
                    {
                        for (int i = 0; i < pelletsPerShot; i++)
                            ShootWeapon();
                        currentAmmo--;
                    }
                }

                if (Input.GetKeyDown(KeyCode.R) && currentAmmo < totalAmmo)
                    isReloading = true;

                if (isReloading) Reload();
            }
            
        }

        public override void ShootWeapon()
        {
            if (ownerRef != OwnerClientId) return;

            Rigidbody rb = GetComponent<Rigidbody>();
            StartCoroutine(PushBackRoutine(rb, 0.3f, 8f));

            // Get the base shooting direction from muzzle
            Vector3 baseDirection = playerNetwork.MuzzleShotgun.forward;

            // Create a random rotation within a cone angle
            float angle = bulletSpread; // degrees

            // Generate random spread direction within a cone
            Vector3 spreadDirection = Quaternion.Euler(
                Random.Range(-angle, angle),
                Random.Range(-angle, angle),
                0
            ) * baseDirection;

            // Fire from muzzle
            // playerNetwork?.ShootGun(playerNetwork.MuzzlePoint.position, spreadDirection);
            playerNetwork?.ShootRayServerRpc(damage, playerNetwork.MuzzleShotgun.position, spreadDirection);

        // --------------------- OLD CODE --------------------------
        
        //     if (ownerRef != OwnerClientId) return;
        //     Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        //     StartCoroutine(PushBackRoutine(rb, 0.3f, 8f));

        //     // rb.AddForce(-transform.forward * 10f, ForceMode.Impulse);
        //     float xOffset = Random.Range(-bulletSpread, bulletSpread);
        //     float yOffset = Random.Range(-bulletSpread, bulletSpread);

        //     // Debug.Log(gameObject.GetComponent<Rigidbody>());

        //     Vector3 point = new Vector3(cam.pixelWidth / 2 + cam.pixelWidth * xOffset, cam.pixelHeight / 2 + cam.pixelHeight * yOffset, 0);
        //     Ray ray = cam.ScreenPointToRay(point);
        //     // Ray ray = cam.ScreenPointToRay(point);

        //     // keep this to shoot the gun
        //     // client tells server to shoot for it
        //     bool has = playerNetwork? true : false;

        //                 // get forward direction from muzzle
        //     Vector3 baseDirection = playerNetwork.MuzzlePoint.forward;

        //    // apply small random rotation in world space
        //     Quaternion randomRotation = Quaternion.Euler(
        //         xOffset,
        //         yOffset,
        //         0f
        //     );
        //     // rotate base direction to create spread
        //     Vector3 spreadDirection = randomRotation * baseDirection;

        //     // fire from muzzle
        //     playerNetwork?.ShootGun(playerNetwork.MuzzlePoint.position, spreadDirection);
        }

        IEnumerator PushBackRoutine(Rigidbody rb, float duration, float force)
        {
            float timer = 0f;
            while (timer < duration)
            {
                rb.AddForce(-transform.forward * force, ForceMode.Force);
                timer += Time.deltaTime;
                yield return null;
            }
        }
    }

    public class WeaponAssaultRifle : WeaponBase
    {
        public float fireRate = 0.1f;
        private float lastShotTime = 0f;

        private void Awake()
        {
            totalAmmo = 90;
            currentAmmo = totalAmmo;
            damage = 8f;
            totalReloadTime = 2.0f;
            bulletSpread = 0.02f;
        }

             public void Initalize()
        {
            base.Initalize();
        }

        private void Update()
        {
            if (ownerRef == null || ownerRef != OwnerClientId) return;

            if (player.isAlive.Value)
            {

                if (Input.GetMouseButton(0) && !isReloading && Time.time >= lastShotTime + fireRate)
                {
                    if (currentAmmo > 0)
                    {
                        ShootWeapon();
                        currentAmmo--;
                        lastShotTime = Time.time;
                    }
                }

                if (Input.GetKeyDown(KeyCode.R) && currentAmmo < totalAmmo)
                    isReloading = true;

                if (isReloading) Reload();
            }
        }

        public override void ShootWeapon()
        {
            Debug.Log("SHooting " + OwnerClientId);
            float xOffset = Random.Range(-bulletSpread, bulletSpread);
            float yOffset = Random.Range(-bulletSpread, bulletSpread);
            Vector3 point = new Vector3(cam.pixelWidth / 2 + cam.pixelWidth * xOffset, cam.pixelHeight / 2 + cam.pixelHeight * yOffset, 0);
            Ray ray = cam.ScreenPointToRay(point);

            // keep this to shoot the gun
            // client tells server to shoot for it
            // playerNetwork?.ShootRayServerRpc(damage, ray.origin, ray.direction);
            playerNetwork?.ShootRayServerRpc(damage, playerNetwork.MuzzleAR.position, playerNetwork.MuzzleAR.forward.normalized);
        }
    }

    public class WeaponSMG : WeaponBase
    {
        public float fireRate = 0.07f;
        private float lastShotTime = 0f;

        private void Awake()
        {
            totalAmmo = 120;
            currentAmmo = totalAmmo;
            damage = 5f;
            totalReloadTime = 1.5f;
            bulletSpread = 0.035f;
        }
        public void Initalize()
        {
            base.Initalize();
        }

        private void Update()
        {
            if (ownerRef == null || ownerRef != OwnerClientId) return;

            if (player.isAlive.Value)
            {
                if (Input.GetMouseButton(0) && !isReloading && Time.time >= lastShotTime + fireRate)
                {
                    if (currentAmmo > 0)
                    {
                        ShootWeapon();
                        currentAmmo--;
                        lastShotTime = Time.time;
                    }
                }

                if (Input.GetKeyDown(KeyCode.R) && currentAmmo < totalAmmo)
                    isReloading = true;

                if (isReloading) Reload();
            }
        }

        public override void ShootWeapon()
        {
            float xOffset = Random.Range(-bulletSpread, bulletSpread);
            float yOffset = Random.Range(-bulletSpread, bulletSpread);
            Vector3 point = new Vector3(cam.pixelWidth / 2 + cam.pixelWidth * xOffset, cam.pixelHeight / 2 + cam.pixelHeight * yOffset, 0);
            Ray ray = cam.ScreenPointToRay(point);

            Vector3 baseDirection = playerNetwork.MuzzleSMG.forward;

           // apply small random rotation in world space
            Quaternion randomRotation = Quaternion.Euler(
                xOffset,
                yOffset,
                0f
            );
            // rotate base direction to create spread
            Vector3 spreadDirection = randomRotation * baseDirection;

            // keep this to shoot the gun
            // client tells server to shoot for it
            playerNetwork?.ShootRayServerRpc(damage, playerNetwork.MuzzleSMG.position, spreadDirection);
        }
    }

    public class WeaponSniper : WeaponBase
    {
        private void Awake()
        {
            totalAmmo = 1;
            currentAmmo = totalAmmo;
            damage = 100;
            totalReloadTime = 6.0f;
            bulletSpread = 0f;
        }
        public void Initalize()
        {
            base.Initalize();
        }

        private void Update()
        {
            if (ownerRef == null || ownerRef != OwnerClientId) return;

            if (player.isAlive.Value)
            {
                if (Input.GetMouseButtonDown(0) && !isReloading)
                {
                    if (currentAmmo > 0)
                    {
                        ShootWeapon();
                        currentAmmo--;
                    }
                }

                if (Input.GetKeyDown(KeyCode.R) && currentAmmo < totalAmmo)
                    isReloading = true;

                if (isReloading) Reload();
            }
        }

        public override void ShootWeapon()
        {
            Vector3 point = new Vector3(cam.pixelWidth / 2, cam.pixelHeight / 2, 0);
            Ray ray = cam.ScreenPointToRay(point);

            // keep this to shoot the gun
            // client tells server to shoot for it
             playerNetwork?.ShootRayServerRpc(damage, playerNetwork.MuzzleSniper.position ,playerNetwork.MuzzleSniper.forward.normalized);
        }
    }

    public class WeaponPistol : WeaponBase
    {
        private void Awake()
        {
            totalAmmo = 50;
            currentAmmo = totalAmmo;
            damage = 6f;
            totalReloadTime = 2.2f;
            bulletSpread = 0.01f;
        }
        public void Initalize()
        {
            base.Initalize();
        }

        private void Update()
        {
            if (player.isAlive.Value)
            {
                if (ownerRef == null || ownerRef != OwnerClientId) return;

                if (Input.GetMouseButtonDown(0) && !isReloading)
                {
                    if (currentAmmo > 0)
                    {
                        ShootWeapon();
                        currentAmmo--;
                    }
                }

                if (Input.GetKeyDown(KeyCode.R) && currentAmmo < totalAmmo)
                    isReloading = true;

                if (isReloading) Reload();
            }
        }

        public override void ShootWeapon()
        {
            float xOffset = Random.Range(-bulletSpread, bulletSpread);
            float yOffset = Random.Range(-bulletSpread, bulletSpread);
            Vector3 point = new Vector3(cam.pixelWidth / 2 + cam.pixelWidth * xOffset, cam.pixelHeight / 2 + cam.pixelHeight * yOffset, 0);
            Ray ray = cam.ScreenPointToRay(point);
            Debug.Log(playerNetwork?.MuzzlePoint.position);

            // keep this to shoot the gun
            // client tells server to shoot for it
            //  playerNetwork?.ShootRayServerRpc(ray.origin, ray.direction);
            playerNetwork?.ShootRayServerRpc(damage, playerNetwork.MuzzlePoint.position, playerNetwork.MuzzlePoint.forward.normalized);
                // DEBUG: visualize the ray
    // Debug.DrawRay(playerNetwork.MuzzlePoint.position, playerNetwork.MuzzlePoint.forward.normalized * 5f, Color.red, 2f);
            //  playerNetwork?.ShootRayServerRpc(playerNetwork.MuzzlePoint.position, playerNetwork.MuzzlePoint.forward);
        }
    }
}
