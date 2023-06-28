using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckboxList : MonoBehaviour
{
    [SerializeField] Checkbox[] checkboxes;
    AudioManager audioManager;
    private void Awake()
    {
        audioManager = GetComponent<AudioManager>();
    }
    public void Check(int i)
    {
        checkboxes[i].Check();
        audioManager.PlaySFX(0);
    }
}
