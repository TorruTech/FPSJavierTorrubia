using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreepNPC : MonoBehaviour
{
    public GameObject player; // Referencia al jugador
    public GameObject ak47Prefab; // Prefab del AK47 que será instanciado
    public Transform weaponSpawnPoint; // Donde aparecerá el arma, como las manos del jugador
    public float interactDistance = 3f; // Distancia para la interacción
    public KeyCode interactKey = KeyCode.E; // Tecla para interactuar

    private bool isInRange = false; // Flag para saber si el jugador está cerca

    public Animator npcAnimator;
    public AudioSource npcAudioSource; // Referencia al AudioSource del NPC
    public AudioClip interactionSound;

    private Boolean hasInteracted = false;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        npcAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (isInRange && Input.GetKeyDown(interactKey))
        {
            InteractWithNPC();  // Llamamos al método para hacer la acción
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))  // Verificamos si es el jugador
        {
            isInRange = true;
            Debug.Log("Jugador cerca del NPC. Presiona 'E' para interactuar.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInRange = false;
        }
    }

    private void InteractWithNPC()
    {
        if (hasInteracted) return;  // Si ya interactuamos, no hacer nada más
        hasInteracted = true;
        Debug.Log("Interacción con el NPC iniciada!");

        // Cambiar el arma del jugador por el AK47
        WeaponChanger weaponChanger = player.GetComponentInChildren<WeaponChanger>();
        
        if (weaponChanger != null)
        {
            weaponChanger.ChangeWeaponToAK47(ak47Prefab, weaponSpawnPoint);
        }

        // Animación y sonido de la interacción
        if (npcAnimator != null)
        {
            Debug.Log("Activando Trigger DIE");
            npcAnimator.SetTrigger("DIE");
            Debug.Log("Activadooooo");

        }

        if (npcAudioSource != null && interactionSound != null)
        {
            npcAudioSource.PlayOneShot(interactionSound);
        }

        StartCoroutine(DestroyNPC());
    }

    private IEnumerator DestroyNPC()
    {
        yield return new WaitForSeconds(7f);

        Destroy(gameObject);
    }
}
