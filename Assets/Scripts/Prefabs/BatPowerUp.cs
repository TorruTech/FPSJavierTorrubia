using System.Collections;
using UnityEngine;

public class BatPowerUp : MonoBehaviour
{
    public float bulletCadenceIncreaseFactor = 0.5f;  
    public float effectDuration = 60f;  

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            Weapon playerWeapon = other.gameObject.GetComponentInChildren<Player>().weapon;

            playerWeapon.IncreaseBulletCadence(bulletCadenceIncreaseFactor, effectDuration);

            Destroy(gameObject);
        }
    }
}
