using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameSettings
{
    static private float brightness, mouseSensitivity, musicVolume, sfxVolume;
    static private int toggleRun, nightVisionShader;
    static public float Brightness
    {
        get => brightness;
        set
        {

            brightness = value;
            PlayerPrefs.SetFloat("Brightness", value);
        }
    }
   static public float MouseSensitivity
    {
        get => mouseSensitivity;
        set
        {
            mouseSensitivity = value;
            PlayerPrefs.SetFloat("MouseSensitivity", value);
        }
    }
    static public float MusicVolume
    {
        get => musicVolume;
        set
        {
            musicVolume = value;
            PlayerPrefs.SetFloat("MusicVolume", value);
        }
    }
    static public float SFXVolume
    {
        get => sfxVolume;
        set
        {
            sfxVolume = value;
            PlayerPrefs.SetFloat("SFXVolume", value);
        }
    }
    
    static private int _ToggleRun
    {
        get => toggleRun;
        set
        {
            toggleRun = value;
            PlayerPrefs.SetInt("ToggleRun", value);
        }
    }
    static public bool ToggleRun
    {
        get => _ToggleRun == 1;
        set => _ToggleRun = value ? 1 : 0;
    }
    static private int _NightVisionShader
    {
        get => nightVisionShader;
        set
        {
            nightVisionShader = value;
            PlayerPrefs.SetInt("NightVisionShader", value);
        }
    }
    static public bool NightVisionShader
    {
        get => _NightVisionShader == 1;
        set => _NightVisionShader = value ? 1 : 0;
    }
    static GameSettings()
    {
        LoadPlayerPrefs();
    }
    static void LoadPlayerPrefs()
    {
        Brightness = PlayerPrefs.GetFloat("Brightness", 1);
        MouseSensitivity = PlayerPrefs.GetFloat("MouseSensitivity", 0.5f);
        MusicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        SFXVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);
        _ToggleRun = PlayerPrefs.GetInt("ToggleRun", 0);
        _NightVisionShader = PlayerPrefs.GetInt("NightVisionShader", 1);

    }
    static void ResetPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
        LoadPlayerPrefs();
    }
}
