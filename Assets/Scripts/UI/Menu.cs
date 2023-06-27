using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadScene("Map");
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public void MainMenu()
    {
        SceneManager.LoadScene("Main Menu");
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Resume()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public void Quit()
    {
        Application.Quit();
    }
    
}
