using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBox : MonoBehaviour
{
    public GameObject player;

    public int healthIncrease = 30;

    private void OnTriggerEnter(Collider player)
    {
        if (player.CompareTag("Player"))
        {
            Debug.Log("Player entered health box");
            IncreaseHealth(player.gameObject);
            Destroy(gameObject);
        }
    }

    private void IncreaseHealth(GameObject player)
    {
        Player playerScript = player.GetComponent<Player>();

        playerScript.HP += healthIncrease;
        if (playerScript.HP > 100)
        {
            playerScript.HP = 100;
        }

        playerScript.playerHealthUI.text = $"HP: {playerScript.HP}";
    }
}
