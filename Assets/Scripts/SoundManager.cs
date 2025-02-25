using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Weapon;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; set; }

    public AudioSource shootingSoundM1911;
    public AudioSource reloadingSoundM1911;

    public AudioClip AK47Shot;

    public AudioSource shootingSoundAK47;
    public AudioSource reloadingSoundAK47;
    public AudioSource emptyMagazineSoundM1911;

    public AudioClip zombieWalking;
    public AudioClip zombieChase;
    public AudioClip zombieAttack;
    public AudioClip zombieHurt;
    public AudioClip zombieDeath;
    public AudioClip explosion;

    public AudioSource zombieChannel;
    public AudioSource zombieChannel2;

    public AudioSource playerChannel;
    public AudioClip playerHurt;
    public AudioClip playerDie;
    public AudioClip gameOverMusic;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void PlayShootingSound(WeaponModel weapon)
    {
        switch (weapon)
        {
            case WeaponModel.M1911:
                shootingSoundM1911.Play();
                break;
            case WeaponModel.AK47:
                shootingSoundAK47.PlayOneShot(AK47Shot);
                break;
        }
    }

    public void PlayReloadSound(WeaponModel weapon)
    {
        switch (weapon)
        {
            case WeaponModel.M1911:
                reloadingSoundM1911.Play();
                break;
            case WeaponModel.AK47:
                reloadingSoundAK47.Play();
                break;
        }
    }


}
