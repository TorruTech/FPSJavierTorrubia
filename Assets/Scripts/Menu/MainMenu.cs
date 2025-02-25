using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public TMP_Text highScoreMap1UI;
    public TMP_Text highScoreMap2UI;

    public GameObject canvasMenu;

    private Camera mainMenuCamera;

    public AudioClip menuMusic;
    public AudioSource menuChannel;

    private string selectedScene = "SampleScene";


    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        AudioListener.pause = false;

        menuChannel.PlayOneShot(menuMusic);

        if (SaveLoadManager.Instance != null)
        {
            canvasMenu = SaveLoadManager.Instance.canvasMenu;
        }

        mainMenuCamera = Camera.main;

        SaveLoadManager.Instance.ActivateMenuButtons();

        UpdateHighScores();
    }

    void UpdateHighScores()
    {
        int highScoreMap1 = SaveLoadManager.Instance.LoadHighScore("SampleScene");
        int highScoreMap2 = SaveLoadManager.Instance.LoadHighScore("SecondMap");

        highScoreMap1UI.text = $"Máx. Ronda: {highScoreMap1}";
        highScoreMap2UI.text = $"Máx. Ronda: {highScoreMap2}";
    }

    public void SelectMap1()
    {
        selectedScene = "SampleScene";
        StartNewGame();
    }

    public void SelectMap2()
    {
        selectedScene = "SecondMap";
        StartNewGame();
    }


    public void StartNewGame()
    {
        menuChannel.Stop();

        if (mainMenuCamera != null)
        {
            mainMenuCamera.gameObject.SetActive(false);
        }

        if (canvasMenu != null)
        {
            canvasMenu.SetActive(false);  
        }

        if (SaveLoadManager.Instance != null)
        {
            SaveLoadManager.Instance.DeactivateMenuButtons();
        }

        SceneManager.LoadScene(selectedScene);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    
}
