using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int baseHP = 40;
    private int HP;
    private int MaxHP = 100;
    private Animator animator;

    private NavMeshAgent navAgent;

    public bool isDead;

    public Zombie zombieScript;
    public GameObject bookHeadMonsterPrefab; 

    public GameObject ammoBoxPrefab;
    public GameObject healthBoxPrefab;
    public GameObject batPrefab;
    public float dropChance = 0.25f;

    private Explosion explosionScript;

    private void Start()
    {
        animator = GetComponent<Animator>();
        navAgent = GetComponent<NavMeshAgent>();
        zombieScript = GetComponent<Zombie>();

        HP = baseHP;

        if (gameObject.layer == LayerMask.NameToLayer("Boss"))
        {
            explosionScript = GetComponent<Explosion>();
        }
    }

    public void TakeDamage(int damageAmount)
    {

        if (isDead) return;

        HP -= damageAmount;

        if (HP <= 0)
        {

            DropItem();

            zombieScript.Die();

            int randomValue = Random.Range(0, 2);

            if (randomValue == 0)
            {
                animator.SetTrigger("DIE1");
            }
            else
            {
                animator.SetTrigger("DIE2");
            }

            isDead = true;

            // Si el enemigo está en la capa BookheadMonster, explota
            if (gameObject.layer == LayerMask.NameToLayer("Boss") && explosionScript != null)
            {
                explosionScript.Explode(transform.position); // Llama a la explosión
            }

            SoundManager.Instance.zombieChannel.PlayOneShot(SoundManager.Instance.zombieDeath);

        }
        else
        {
            animator.SetTrigger("DAMAGE");

            SoundManager.Instance.zombieChannel2.PlayOneShot(SoundManager.Instance.zombieHurt);
        }
    }

    public void UpdateHealth(int extraHealth)
    {
        HP = Mathf.Min(HP + extraHealth, MaxHP);
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 2.5f); //Attacking

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, 18f); //Detection

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 21f); //Stop Chasing
    }

    private void DropItem()
    {
        if (Random.value < dropChance)
        {
            int randomValue = Random.Range(0, 4);
            GameObject itemToDrop;

            if (randomValue == 0)
            {
                itemToDrop = healthBoxPrefab;
            }
            else if (randomValue == 1)
            {
                itemToDrop = batPrefab;
            }
            else
            {
                itemToDrop = ammoBoxPrefab;
            }

            Vector3 spawnPosition = transform.position + new Vector3(Random.Range(-0.5f, 0.5f), 0.5f, Random.Range(-0.5f, 0.5f));
            Instantiate(itemToDrop, spawnPosition, Quaternion.identity);
        }
    }

}
