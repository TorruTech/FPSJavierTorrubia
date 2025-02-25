using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class ZombieSpawnController : MonoBehaviour
{
    public int initialZombiesPerWave = 5;
    public int currentZombiesPerWave;

    public float spawnDelay = 0.5f;

    public int currentWave = 0;
    public float waveCooldown = 10.0f;

    public bool inCooldown;
    public float cooldownCounter = 0;

    public List<Enemy> currentZombiesAlive;

    public GameObject zombiePrefab;
    public GameObject bookHeadMonsterPrefab; 

    public TextMeshProUGUI waveOverUI;
    public TextMeshProUGUI cooldownCounterUI;
    public TextMeshProUGUI roundNumberUI;

    // Lista de puntos de spawn
    public List<Transform> spawnPoints;

    private void Start()
    {
        currentZombiesPerWave = initialZombiesPerWave;

        GlobalReferences.Instance.waveNumber = currentWave;

        StartNextWave();
    }

    private void Update()
    {
        // Obtener todos los zombies muertos
        List<Enemy> zombiesToRemove = new List<Enemy>();
        foreach (Enemy zombie in currentZombiesAlive)
        {
            if (zombie.isDead)
            {
                zombiesToRemove.Add(zombie);
            }
        }

        // Eliminar los zombies muertos
        foreach (Enemy zombie in zombiesToRemove)
        {
            currentZombiesAlive.Remove(zombie);
        }

        zombiesToRemove.Clear();

        // Empezar el Cooldown entre rondas si todos los zombies están muertos
        if (currentZombiesAlive.Count == 0 && inCooldown == false)
        {
            StartCoroutine(WaveCooldown());
        }

        // Iniciar el contador
        if (inCooldown)
        {
            cooldownCounter -= Time.deltaTime;
        }
        else
        {
            cooldownCounter = waveCooldown;
        }

        cooldownCounterUI.text = ((int)cooldownCounter).ToString();
    }

    private IEnumerator WaveCooldown()
    {
        inCooldown = true;

        waveOverUI.gameObject.SetActive(true);
        roundNumberUI.gameObject.SetActive(false);

        yield return new WaitForSeconds(waveCooldown);

        inCooldown = false;
        waveOverUI.gameObject.SetActive(false);
        roundNumberUI.gameObject.SetActive(true);

        currentZombiesPerWave += 2;  // Aumentamos el número de zombies por ola

        StartNextWave();
    }

    private void StartNextWave()
    {
        currentZombiesAlive.Clear();

        currentWave++;

        GlobalReferences.Instance.waveNumber = currentWave;

        roundNumberUI.text = $"Ronda {currentWave}";

        UpdateZombiesHealth();  // Actualizamos la salud de los zombies

        StartCoroutine(SpawnWave());  // Iniciamos la aparición de los zombies
    }

    private void UpdateZombiesHealth()
    {
        // Aumentamos la salud de los zombies según la ronda actual, con un máximo de 100
        int healthIncreasePerWave = Mathf.Min(10 * currentWave, 60); // Aumenta 10 por ronda hasta un máximo de 60

        // Actualizamos la salud de todos los zombies vivos
        foreach (Enemy zombie in currentZombiesAlive)
        {
            zombie.UpdateHealth(healthIncreasePerWave);
        }
    }

    private IEnumerator SpawnWave()
    {
        int bookHeadCount = 0;

        // Determinar cuántos BookHeadMonsters deben aparecer en esta ronda
        if (currentWave >= 10)
        {
            bookHeadCount = 3;
        }
        else if (currentWave >= 7)
        {
            bookHeadCount = 2;
        }
        else if (currentWave >= 5)
        {
            bookHeadCount = 1;
        }

        // Ahora instanciamos los zombies (incluyendo los BookHeadMonsters si corresponde)
        for (int i = 0; i < currentZombiesPerWave; i++)
        {
            // Selecciona un punto de spawn aleatorio
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
            Vector3 spawnPosition = spawnPoint.position;

            // Instanciar el zombie
            GameObject zombie = Instantiate(zombiePrefab, spawnPosition, Quaternion.identity);
            Enemy enemyScript = zombie.GetComponent<Enemy>();

            // Asegúrate de que los zombies recién creados tengan la salud correcta
            int healthIncreasePerWave = Mathf.Min(10 * currentWave, 60);
            enemyScript.UpdateHealth(healthIncreasePerWave);

            // Trackear el zombie
            currentZombiesAlive.Add(enemyScript);

            // Si corresponde, instanciar BookHeadMonsters en esta ronda
            if (bookHeadCount > 0)
            {
                // Instanciamos BookHeadMonster en los lugares correspondientes
                GameObject bookHead = Instantiate(bookHeadMonsterPrefab, spawnPosition, Quaternion.identity);
                Enemy bookHeadScript = bookHead.GetComponent<Enemy>();
                bookHeadScript.UpdateHealth(healthIncreasePerWave);  // Asegura que tenga la salud correcta
                currentZombiesAlive.Add(bookHeadScript);
                bookHeadCount--;  // Reducimos el contador de BookHeadMonsters
            }

            yield return new WaitForSeconds(spawnDelay);
        }
    }
}
