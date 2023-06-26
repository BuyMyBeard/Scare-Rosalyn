using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TV : Interactable
{
    [SerializeField] Animator animator;
    [SerializeField] float cooldown = 0.5f;
    [SerializeField] LayerMask victimLayer;
    [SerializeField] VictimAI victim;
    [SerializeField] float terrorValue = 0.15f;
    bool turnedOn = false;
    SphereCollider trigger;
    VictimTrigger scareArea;
    bool hasScaredVictim = false;

    protected override void Awake()
    {
        base.Awake();
        trigger = GetComponent<SphereCollider>();
        scareArea = GetComponentInChildren<VictimTrigger>();
    }
    public void CollectRemote()
    {
        StartCoroutine(InitializationCooldown());
        
    }
    public override void Interact()
    {
        StartCoroutine(BlockInteraction());
        turnedOn = !turnedOn;
        if (turnedOn)
        {
            animator.Play("Static");
            if (!hasScaredVictim && scareArea.Triggered)
            {
                victim.StartCoroutine(victim.RaiseProgressBarOverTime(terrorValue));
                hasScaredVictim = true;
            }
        }
        else
        {
            animator.Play("Off");

        }
    }
    private void OnTriggerEnter(Collider other)
    {
        Prompt();
    }

    private void OnTriggerExit(Collider other)
    {
        CancelPrompt();
    }

    IEnumerator InitializationCooldown()
    {
        yield return new WaitForSeconds(cooldown);
        trigger.enabled = true;
        Prompt();
    }
    IEnumerator BlockInteraction()
    {
        trigger.enabled = false;
        yield return new WaitForSeconds(cooldown);
        trigger.enabled = true;
    }
}
