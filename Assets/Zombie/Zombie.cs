using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    public ZombieHand zombieHand;

    public int zombieDamage;

    private void Start()
    {
        zombieHand.damage = zombieDamage;
    }

    public void Die() 
    {
        if (zombieHand != null)
        {
            Collider handCollider = zombieHand.GetComponent<Collider>();
            if (handCollider != null)
            {
                handCollider.enabled = false; 
            }
        }
    }
}
