using System.Collections;
using System.Collections.Generic;
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
    }
    public override void Interact()
    {
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
}
