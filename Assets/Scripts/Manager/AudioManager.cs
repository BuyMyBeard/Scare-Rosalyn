using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioClip[] sfx;
    AudioSource sfxSource;
    private void Awake()
    {
        sfxSource = GetComponent<AudioSource>();
    }
    public void PlaySFX(int id, float volumeScale = 1)
    {
        sfxSource.PlayOneShot(sfx[id], volumeScale);
    }

    public void PlayLoop()
    {
        sfxSource.Play();
    }
    public void StopLoop()
    {
        sfxSource.Stop();
    }
}
