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
}
