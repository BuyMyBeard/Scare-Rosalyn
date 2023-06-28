using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Lever : Objective
{
    Animator animator;
    BoxCollider trigger;
    [SerializeField] Animator fenceAnimator;
    [SerializeField]AudioSource audioSource;
    public override void InteractionOver()
    {
        animator.Play("On");
        fenceAnimator.Play("Open");
        StartCoroutine(PlayGateSound());
        trigger.enabled = false;
        Check();
    }
    IEnumerator PlayGateSound()
    {
        audioSource.Play();
        yield return new WaitForSeconds(8);
        audioSource.Stop();
    }

    void Awake()
    {
        animator = GetComponent<Animator>();
        trigger = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        VictimAI ai = other.GetComponent<VictimAI>();
        if (ai.CurrentObjective == this)
            ai.StartInteractWithObjectiveAnimation(this);
    }
}
