using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Weapon : MonoBehaviour
{

    public int weaponDamage;

    // Disparando
    public bool isShooting, readyToShoot;
    bool allowReset = true;
    public float initialShootingDelay = 0.2f;
    public float shootingDelay = 2f;

    // Burst
    public int bulletsPerBurst = 3;
    public int burstBulletsLeft;

    // Spread
    public float spreadIntensity;

    // Bullp
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public float bulletVelocity = 30;
    public float bulletPrefabLifeTime = 3f;

    public GameObject muzzleEffect;
    private Animator animator;

    // Loading
    public float reloadTime;
    public int magazineSize, totalBullets, bulletsInMagazine;
    public bool isReloading;

    public enum WeaponModel
    {
        M1911,
        AK47
    }

    public WeaponModel thisWeaponModel;

    public enum ShootingMode
    {
        Single,
        Burst,
        Auto
    }

    public ShootingMode currentShootingMode;

    private void Awake()
    {
        
        readyToShoot = true;
        burstBulletsLeft = bulletsPerBurst;
        animator = GetComponent<Animator>();

        bulletsInMagazine = magazineSize;
    }

    void Start()
    {
        shootingDelay = initialShootingDelay;
    }

    // Update is called once per frame
    void Update()
    {
        if (bulletsInMagazine == 0 && isShooting)
        {
            SoundManager.Instance.emptyMagazineSoundM1911.Play();
        }

        if (currentShootingMode == ShootingMode.Auto)
        {
            // Manteniendo el gatillo derecho del mando (Joystick Button 5)
            isShooting = Input.GetButton("Fire1"); // 'Fire1' por defecto está mapeado al gatillo derecho
        }
        else if (currentShootingMode == ShootingMode.Single ||
                 currentShootingMode == ShootingMode.Burst)
        {
            // Pulsando una vez el gatillo derecho del mando (Joystick Button 5)
            isShooting = Input.GetButtonDown("Fire1"); // 'Fire1' por defecto está mapeado al gatillo derecho
        }

        if ((Input.GetKeyDown(KeyCode.R) || Input.GetButtonDown("Reload")) && bulletsInMagazine < magazineSize && isReloading == false)
        {
            Reload();
        }

        // Recargar automáticamente si el cargador está vacío
        if (readyToShoot && !isShooting && isReloading == false && bulletsInMagazine <= 0)
        {
            Reload();
        }
       
        if (readyToShoot && isShooting && bulletsInMagazine > 0)
        {
            burstBulletsLeft = bulletsPerBurst;
            FireWeapon();
        }

        if (AmmoManager.Instance.ammoDisplay != null)
            {
            AmmoManager.Instance.ammoDisplay.text = $"{bulletsInMagazine}/{totalBullets}";
        }

    }

    public void SetRunningState(bool isRunning)
    {
        if (animator != null)
        {
            animator.SetBool("isRunning", isRunning);
        }
    }

    private void FireWeapon()
    {
        bulletsInMagazine--;

        muzzleEffect.GetComponent<ParticleSystem>().Play();
        animator.SetTrigger("RECOIL");
 
        SoundManager.Instance.PlayShootingSound(thisWeaponModel);

        readyToShoot = false;

        Vector3 shootingDirection = CalculateDirectionAndSpread().normalized;


        // Instanciamos la bala
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);

        Bullet bul = bullet.GetComponent<Bullet>();
        bul.bulletDamage = weaponDamage;

        // Orientamos la bala para que cuadre con shootingDirection
        bullet.transform.forward = shootingDirection;

        // Disparamos la bala
        bullet.GetComponent<Rigidbody>().AddForce(shootingDirection * bulletVelocity, ForceMode.Impulse);

        // Destruimos la bala después de un tiempo
        StartCoroutine(DestroyBulletAfterTime(bullet, bulletPrefabLifeTime));

        // Comprobar si hemos terminado de disparar
        if (allowReset)
        {
            Invoke("ResetShot", shootingDelay);
            allowReset = false;
        }

        // Modo Burst
        if (currentShootingMode == ShootingMode.Burst && burstBulletsLeft > 1)
        {
            burstBulletsLeft--;
            Invoke("FireWeapon", shootingDelay);
        }
    }

    private void Reload()
    {
        if (totalBullets <= 0 || bulletsInMagazine == magazineSize || isReloading)
        {
            return;
        }
        SoundManager.Instance.PlayReloadSound(thisWeaponModel);
        isReloading = true;
        readyToShoot = false;
        Invoke("ReloadCompleted", reloadTime);
    }

    private void ReloadCompleted()
    {
        if (totalBullets > 0)
        {
            int bulletsNeeded = magazineSize - bulletsInMagazine; 
            int bulletsToReload = Mathf.Min(bulletsNeeded, totalBullets); 

            bulletsInMagazine += bulletsToReload; 
            totalBullets -= bulletsToReload; 
        }
        isReloading = false;
        readyToShoot = true;
    }

    private void ResetShot()
    {
        readyToShoot = true;
        allowReset = true;
    }

    public Vector3 CalculateDirectionAndSpread()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit))
        {
            // Al golpear algo
            targetPoint = hit.point;
        }
        else
        {
            // Al disparar al aire
            targetPoint = ray.GetPoint(100);
        }

        Vector3 direction = targetPoint - bulletSpawn.position;

        float x = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);
        float y = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);

        // Devuelve la shootingDirection y el spread 
        return direction + new Vector3(x, y, 0);

    }
    
    private IEnumerator DestroyBulletAfterTime(GameObject bullet, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(bullet);
    }

    internal void IncreaseBulletCadence(float bulletCadenceIncreaseFactor, float effectDuration)
    {
        shootingDelay = initialShootingDelay * (1f - bulletCadenceIncreaseFactor);

        StartCoroutine(RestoreShootingDelayAfterTime(effectDuration));
    }

    private IEnumerator RestoreShootingDelayAfterTime(float duration)
    {
        yield return new WaitForSeconds(duration);
        shootingDelay = initialShootingDelay;
    }

}
