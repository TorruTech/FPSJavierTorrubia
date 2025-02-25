using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public int HP = 100;

    public GameObject bloodyScreen;

    public Weapon weapon;
    public WeaponChanger weaponChanger; 

    public TextMeshProUGUI playerHealthUI;
    public GameObject gameOverUI;

    public bool isDead;

    private void Start()
    {
        playerHealthUI.text = $"HP: {HP}";
        weapon = GetComponentInChildren<Weapon>();
    }

    private void Update()
    {
        if (transform.position.y < -5 && !isDead) 
        {
            DieFromFall();
        }
    }

    private void DieFromFall()
    {
        HP = 0;
        PlayerDead();
    }

    public void TakeDamage(int damageAmount)
    {
        HP -= damageAmount;

        Debug.Log("holaaaa");

        if (HP <= 0)
        {
            print("Player Dead");
            weapon.readyToShoot = false;
            PlayerDead();
            isDead = true;
            SoundManager.Instance.playerChannel.PlayOneShot(SoundManager.Instance.playerDie);
        }
        else
        {
            print("Player Hit");
            StartCoroutine(BloodyScreenEffect());
            playerHealthUI.text = $"HP: {HP}";
            SoundManager.Instance.playerChannel.PlayOneShot(SoundManager.Instance.playerHurt);
        }
    }

    private void PlayerDead()
    {
        GetComponent<MouseMovement>().enabled = false;
        GetComponent<PlayerMovement>().enabled = false;

        GetComponentInChildren<Animator>().enabled = true;
  
        playerHealthUI.gameObject.SetActive(false);
        AmmoManager.Instance.ammoDisplay.gameObject.SetActive(false);

        if (weapon != null)
        {
            weapon.enabled = false;
        }

        GameObject roundNumberObject = GameObject.Find("roundNumberUI");
        if (roundNumberObject != null)
        {
            roundNumberObject.SetActive(false);
        }

        GetComponent<ScreenFader>().StartFade();
        StartCoroutine(ShowGameOverUI());
    }

    private IEnumerator ShowGameOverUI()
    {
        yield return new WaitForSeconds(1f);
        gameOverUI.gameObject.SetActive(true);
        SoundManager.Instance.playerChannel.PlayOneShot(SoundManager.Instance.gameOverMusic);

        int waveSurvived = GlobalReferences.Instance.waveNumber;

        string currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

        if (waveSurvived > SaveLoadManager.Instance.LoadHighScore(currentScene))
        {
            SaveLoadManager.Instance.SaveHighScore(currentScene, waveSurvived);
        }

        StartCoroutine(ReturnToMainMenu());
    }

    private IEnumerator ReturnToMainMenu()
    {
        yield return new WaitForSeconds(5f);

        SceneManager.LoadScene("MainMenu");

        if (SaveLoadManager.Instance != null && SaveLoadManager.Instance.canvasMenu != null)
        {
            SaveLoadManager.Instance.canvasMenu.SetActive(true);
        }
    }

    private IEnumerator BloodyScreenEffect()
    {
        if (!bloodyScreen.activeInHierarchy)
        {
            bloodyScreen.SetActive(true);
        }

        var image = bloodyScreen.GetComponentInChildren<Image>();

        Color startColor = image.color;
        startColor.a = 1f;
        image.color = startColor;

        float duration = 2f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / duration);

            Color newColor = image.color;
            newColor.a = alpha;
            image.color = newColor;

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        // Si no se comenta no aparece la bloodyScreen
        if (bloodyScreen.activeInHierarchy)
        {
            //bloodyScreen.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ZombieHand"))
        {
            if (isDead == false)
            {
                TakeDamage(other.gameObject.GetComponent<ZombieHand>().damage);
            }
           
        }
    }
}
