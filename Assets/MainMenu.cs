using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadScene("Map");
    }

    public void HowToPlay()
    {

    }

    public void Quit()
    {
        Application.Quit();
    }
    
}
