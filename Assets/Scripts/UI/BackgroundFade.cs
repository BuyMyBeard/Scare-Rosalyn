using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundFade : MonoBehaviour
{
    Image background;
    [SerializeField] GameObject fearLevel, detectionLevel, tasklist, prompt;
    private void Awake()
    {
        background = GetComponent<Image>();
    }
    public bool Visible { get; private set; } = false;
    public bool Done { get; private set; } = true;
    public void HidePrompt()
    {
        prompt.SetActive(false);
    }
    public IEnumerator FadeIn(bool fadeOut = false)
    {
        if (!Done || Visible != fadeOut)
            yield break;
        Visible = !fadeOut;
        Done = false;
        for (float t = 0; t <= 1.1; t += Time.deltaTime)
        {
            Color c = background.color;
            c.a = fadeOut ? 1 - t : t;
            background.color = c;
            yield return null;
        }
            fearLevel.SetActive(fadeOut);
            detectionLevel.SetActive(fadeOut);
            tasklist.SetActive(fadeOut);
        
        Done = true;
    }
}
