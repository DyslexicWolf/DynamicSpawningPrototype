using System;
using System.Collections;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GunData gunData;
    [SerializeField] private Transform muzzle;
    [SerializeField] private Transform bulletOrigin;
    private float timeSinceLastShot;

    private void Awake()
    {
        PlayerShoot.shootInput += Shoot;
        PlayerShoot.reloadInput += StartReload;
    }

    public void StartReload()
    {
        if(!gunData.reloading)
        {
            StartCoroutine(ReloadCoroutine());
        }
    }

    private IEnumerator ReloadCoroutine()
    {
        gunData.reloading = true;
        yield return new WaitForSeconds(gunData.reloadTime);

        gunData.currentAmmo = gunData.magSize;
        gunData.reloading = false;
    }

    private bool CanShoot()
    {
        //this calculates the time between shots to see if you can shoot and sees if you arent reloading
        return !gunData.reloading && timeSinceLastShot > 1f / (gunData.fireRate / 60f);
    }
    public void Shoot()
    {
        if(gunData.currentAmmo > 0)
        {
            if (CanShoot())
            {
                if(Physics.Raycast(bulletOrigin.position, bulletOrigin.forward, out RaycastHit hitInfo, gunData.maxDistance))
                {
                    //implement logic to make the enemy take damage
                    //check if what we hit is the enemy
                    //call the takedamage method of the enemy
                    Debug.Log(hitInfo.transform.name);
                }

                gunData.currentAmmo--;
                timeSinceLastShot = 0;
                //can use this for effects or other logic later
                //use the muzzle transform for the origin of vfx for muzzles etc
                OnGunShot();
            }
        }
    }

    private void Update()
    {
        timeSinceLastShot += Time.deltaTime;
        Debug.DrawRay(bulletOrigin.position, bulletOrigin.forward);
    }

    private void OnGunShot()
    {
    }
}
