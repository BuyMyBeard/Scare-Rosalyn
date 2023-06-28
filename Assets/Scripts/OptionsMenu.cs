using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{

    [SerializeField] Slider sfxSlider, musicSlider, mouseSensSlider, brightnessSlider;
    [SerializeField] Checkbox nightVisionCheckbox, toggleRunCheckbox;
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] UnityEvent settingsChanged = new();

    void Awake()
    {
        sfxSlider.value = GameSettings.SFXVolume;
        musicSlider.value = GameSettings.MusicVolume;
        mouseSensSlider.value = GameSettings.MouseSensitivity;
        brightnessSlider.value = GameSettings.Brightness;
        nightVisionCheckbox.Value = GameSettings.NightVisionShader;
        toggleRunCheckbox.Value = GameSettings.ToggleRun;
        
        brightnessSlider.interactable = nightVisionCheckbox.Value;
        UpdateMusic();
        UpdateSFX();
    }
    void UpdateSFX() => audioMixer.SetFloat("SFXVolume", Mathf.Log10(GameSettings.SFXVolume) * 20);
    void UpdateMusic() => audioMixer.SetFloat("MusicVolume", Mathf.Log10(GameSettings.MusicVolume) * 20);

    public void SFXChanged()
    {
        GameSettings.SFXVolume = sfxSlider.value;
        UpdateSFX();
    }
    public void MusicChanged()
    {
        GameSettings.MusicVolume = musicSlider.value;
        UpdateMusic();
    }
    public void MouseSensChanged()
    {
        GameSettings.MouseSensitivity = mouseSensSlider.value;
        settingsChanged.Invoke();
    }
    public void BrightnessChanged()
    {
        GameSettings.Brightness = brightnessSlider.value;
        settingsChanged.Invoke();
    }
    public void ToggleRunChanged()
    {
        GameSettings.ToggleRun = toggleRunCheckbox.Value;
        settingsChanged.Invoke();
    }
    public void NightVisionChanged()
    {
        GameSettings.NightVisionShader = nightVisionCheckbox.Value;
        brightnessSlider.interactable = nightVisionCheckbox.Value;
        settingsChanged.Invoke();
    }
}
