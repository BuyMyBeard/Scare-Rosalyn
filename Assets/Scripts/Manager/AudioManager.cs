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
    public void PlaySFX(int id)
    {
        StartCoroutine(DelaySound(id));
    }
    IEnumerator DelaySound(int id)
    {
        yield return new WaitForSeconds(0.1f);
        sfxSource.PlayOneShot(sfx[id], 1);
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
