using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TVRemote : Interactable
{
    [SerializeField] TV tv;
    public override void Interact()
    {
        tv.CollectRemote();
        gameObject.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        Prompt();
    }
    private void OnTriggerExit(Collider other)
    {
        CancelPrompt();
    }
}
