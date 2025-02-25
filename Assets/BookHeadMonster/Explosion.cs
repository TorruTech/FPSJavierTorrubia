using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public float explosionRadius = 15f;
    public float explosionDamage = 30f;
    public GameObject explosionEffectPrefab;

    private bool hasExploded = false;

    public AudioSource explosionSound;
    public AudioClip explosionClip;

    // M�todo que maneja la explosi�n
    public void Explode(Vector3 explosionPosition)
    {

        if (explosionEffectPrefab == null)
        {
            Debug.Log("NO HAY EXPLOSION");
        }

        Debug.Log("Explotando");
        if (hasExploded) return;  // Si ya explot�, no hacer nada m�s.
        hasExploded = true;

        // Instanciar el efecto de explosi�n
        if (explosionEffectPrefab != null)
        {
            Debug.Log("Explosion at " + explosionPosition);
            Instantiate(explosionEffectPrefab, explosionPosition, Quaternion.identity);
        }

        HashSet<GameObject> damagedObjects = new HashSet<GameObject>();

        foreach (Collider hitCollider in Physics.OverlapSphere(explosionPosition, explosionRadius))
        {
            GameObject obj = hitCollider.gameObject;

            if (damagedObjects.Contains(obj)) continue;  // Evita duplicados
            damagedObjects.Add(obj);  // Registra que ya recibi� da�o

            if (obj.CompareTag("Enemy"))
            {
                Enemy targetEnemy = obj.GetComponent<Enemy>();
                if (targetEnemy != null)
                {
                    targetEnemy.TakeDamage((int)explosionDamage);
                }
            }
            else if (obj.CompareTag("Player"))
            {
                Player targetPlayer = obj.GetComponent<Player>();
                if (targetPlayer != null)
                {
                    targetPlayer.TakeDamage((int)explosionDamage);
                }
            }
        }

        // Reproducir el sonido de la explosi�n
        explosionSound.PlayOneShot(explosionClip);

        // Destruir el objeto con la explosi�n despu�s de un corto tiempo (puedes ajustar este tiempo)
        Destroy(gameObject, 3f);
    }

    // Gizmos para mostrar el �rea de explosi�n en el editor
    public void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, explosionRadius); // �rea de la explosi�n
    }
}
