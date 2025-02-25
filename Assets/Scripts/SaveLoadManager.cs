using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveLoadManager : MonoBehaviour
{
    public static SaveLoadManager Instance { get; set; }

    public GameObject canvasMenu;  

    public Button map1Button;
    public Button map2Button;
    public Button exitButton; 

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

    public void SaveHighScore(string mapName, int score)
    {
        string key = "HighScore_" + mapName;

        if (score > PlayerPrefs.GetInt(key, 0))
        {
            PlayerPrefs.SetInt(key, score);
            PlayerPrefs.Save();
        }
    }

    // Método para cargar la puntuación según el nombre del mapa
    public int LoadHighScore(string mapName)
    {
        string key = "HighScore_" + mapName;
        return PlayerPrefs.GetInt(key, 0);
    }

    public void ActivateMenuButtons()
    {
        map1Button.gameObject.SetActive(true);
        map2Button.gameObject.SetActive(true);
        exitButton.gameObject.SetActive(true);
    }

    public void DeactivateMenuButtons()
    {
        map1Button.gameObject.SetActive(false);
        map2Button.gameObject.SetActive(false);
        exitButton.gameObject.SetActive(false);
    }
}
