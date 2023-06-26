using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TV : Interactable
{
    [SerializeField] Animator animator;

    bool turnedOn = false;
    SphereCollider trigger;

    protected override void Awake()
    {
        base.Awake();
        trigger = GetComponent<SphereCollider>();
    }
    public void CollectRemote()
    {
        trigger.enabled = true;
        Prompt();
    }
    public override void Interact()
    {
        Debug.Log("Toggled");
        turnedOn = !turnedOn;
        if (turnedOn)
            animator.Play("Static");
        else
            animator.Play("Off");
    }
    private void OnTriggerEnter(Collider other)
    {
        Prompt();
    }

    private void OnTriggerExit(Collider other)
    {
        CancelPrompt();
    }
    IEnumerator BlockInteraction()
    {
        trigger.enabled = false;
        yield return new WaitForSeconds(cooldown);
        trigger.enabled = true;
    }
}
