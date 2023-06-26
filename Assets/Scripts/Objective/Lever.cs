using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Lever : Objective
{
    Animator animator;
    BoxCollider trigger;
    [SerializeField] Animator fenceAnimator;
    [SerializeField] FenceGate fenceGate;

    public override void InteractionOver()
    {
        animator.Play("On");
        //fenceGate.Open();
        fenceAnimator.Play("Open");
        trigger.enabled = false;
        Check();
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
