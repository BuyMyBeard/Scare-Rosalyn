using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    [SerializeField] GameObject WinScreen;
    [SerializeField] GameObject LoseScreen;
    [SerializeField] BackgroundFade backgroundFade;
    public IEnumerator Win()
    {
        StopGame();
        yield return new WaitUntil(() => backgroundFade.Done);
        WinScreen.SetActive(true);
    }
    public IEnumerator Lose()
    {
        StopGame();
        yield return new WaitUntil(() => backgroundFade.Done);
        LoseScreen.SetActive(true);
    }
    public void StopGame()
    {
        StartCoroutine(backgroundFade.FadeIn());
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0;
    }
}
