using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{

    [SerializeField] Slider sfxSlider, musicSlider, mouseSensSlider, brightnessSlider;
    [SerializeField] Checkbox nightVisionCheckbox, toggleRunCheckbox;
    [SerializeField] ISettingsListener[] settingsListeners;
    void Awake()
    {
        sfxSlider.value = GameSettings.SFXVolume;
        musicSlider.value = GameSettings.MusicVolume;
        mouseSensSlider.value = GameSettings.MouseSensitivity;
        brightnessSlider.value = GameSettings.Brightness;
        nightVisionCheckbox.Value = GameSettings.NightVisionShader;
        toggleRunCheckbox.Value = GameSettings.ToggleRun;
        
        brightnessSlider.interactable = nightVisionCheckbox.Value;

    }
    public void SFXChanged()
    {
        GameSettings.SFXVolume = sfxSlider.value;
        WarnListerers();
    }
    public void MusicChanged()
    {
        GameSettings.MusicVolume = musicSlider.value;
        WarnListerers();
    }
    public void MouseSensChanged()
    {
        GameSettings.MouseSensitivity = mouseSensSlider.value;
        WarnListerers();
    }
    public void BrightnessChanged()
    {
        GameSettings.Brightness = brightnessSlider.value;
        WarnListerers();
    }
    public void ToggleRunChanged()
    {
        GameSettings.ToggleRun = toggleRunCheckbox.Value;
        WarnListerers();
    }
    public void NightVisionChanged()
    {
        GameSettings.NightVisionShader = nightVisionCheckbox.Value;
        brightnessSlider.interactable = nightVisionCheckbox.Value;
        WarnListerers();
    }
    void WarnListerers()
    {
        if (settingsListeners == null)
            return;
        foreach (var s in settingsListeners)
            s.SettingsUpdated();
    }
}


public interface ISettingsListener
{
    public void SettingsUpdated();
}