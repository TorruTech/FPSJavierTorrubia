using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject ObjetoMenuPausa;
    public bool pausa = false;
    public GameObject menuSalir;
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pausa == false)
            {
                ObjetoMenuPausa.SetActive(true);
                pausa = true;

                Time.timeScale = 0;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;

                AudioListener.pause = true;

            }
            else
            {
                Resumir();
            }
        }
    }

    public void Resumir()
    {
        ObjetoMenuPausa.SetActive(false);
        menuSalir.SetActive(false);
        pausa = false;
        Time.timeScale = 1;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        AudioListener.pause = false;

    }

    public void IrAlMenu(string NombreMenu)
    {
        SceneManager.LoadScene(NombreMenu);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

}