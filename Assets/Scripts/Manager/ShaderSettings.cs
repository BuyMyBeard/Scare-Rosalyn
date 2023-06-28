using Nightvision;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderSettings : MonoBehaviour
{
    Nightvision.Nightvision nightvision;
    private void Awake()
    {
        nightvision = GetComponent<Nightvision.Nightvision>();
        UpdateShader();
    }
    public void UpdateShader()
    {
        nightvision.Settings.Power = GameSettings.Brightness;
        nightvision.enabled = GameSettings.NightVisionShader;
    }
}
