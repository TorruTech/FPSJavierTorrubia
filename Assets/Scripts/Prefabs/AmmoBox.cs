using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoBox : MonoBehaviour
{
    public GameObject player;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            IncreaseAmmo(other.gameObject);
            Destroy(gameObject);
        }
    }

    private void IncreaseAmmo(GameObject player)
    {
        Weapon playerWeapon = player.GetComponentInChildren<Weapon>();

        if (playerWeapon != null)
        {
            playerWeapon.bulletsInMagazine = playerWeapon.magazineSize;
            playerWeapon.totalBullets = playerWeapon.magazineSize*2;

            // Actualizar la UI de la munición
            if (AmmoManager.Instance.ammoDisplay != null)
            {
                AmmoManager.Instance.ammoDisplay.text = $"{playerWeapon.bulletsInMagazine}/{playerWeapon.totalBullets}";
            }
        }
    }
}
