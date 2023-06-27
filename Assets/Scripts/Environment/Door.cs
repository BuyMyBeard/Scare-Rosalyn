using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class Door : Interactable
{
    [SerializeField] BoxCollider doorCollider;
    [SerializeField] LayerMask playerLayer;
    [SerializeField] LayerMask victimLayer;
    [SerializeField] float cooldown = 1.5f;
    [SerializeField] float animationLength = 0.4f;
    [SerializeField] NavMeshObstacle obstacle;
    Animator animator;
    BoxCollider doorTrigger;
    bool opened = false;
    AudioManager audioManager;

    protected override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
        doorTrigger = GetComponent<BoxCollider>();
        audioManager = GetComponent<AudioManager>();
    }
    protected override void OnTriggerEnter(Collider other)
    {
        if (playerLayer.IncludesLayer(other.gameObject.layer))
        {
            Prompt();
        }
        else if (victimLayer.IncludesLayer(other.gameObject.layer))
        {
            if (!opened)
            {
                StartCoroutine(Open());
                other.gameObject.GetComponent<VictimAI>().OpenDoor();
            }
        }
    }
    protected override void OnTriggerExit(Collider other)
    {
        if (playerLayer.IncludesLayer(other.gameObject.layer))
        {
            CancelPrompt();
        }
        if (victimLayer.IncludesLayer(other.gameObject.layer))
        {
            if (opened)
                StartCoroutine(Close());
        }
    }

    public override void Interact()
    {
        if (opened)
            StartCoroutine(Close());
        else
            StartCoroutine(Open());
    }
    IEnumerator Open()
    {
        audioManager.PlaySFX(0);
        animator.Play("Open");
        StartCoroutine(BlockInteraction());
        //doorCollider.enabled = false;
        yield return new WaitForSeconds(animationLength);
        //doorCollider.enabled = true;
        opened = true;
        obstacle.enabled = true;
        promptMessage = "Close door";
    }
    IEnumerator Close()
    {
        audioManager.PlaySFX(1);
        animator.Play("Close");
        StartCoroutine(BlockInteraction());
        //doorCollider.enabled = false;
        yield return new WaitForSeconds(animationLength);
        //doorCollider.enabled = true;
        opened = false;
        obstacle.enabled = false;
        promptMessage = "Open door";
    }

    IEnumerator BlockInteraction()
    {
        doorTrigger.enabled = false;
        yield return new WaitForSeconds(cooldown);
        doorTrigger.enabled = true;
    }
}
public static class ExtensionMethods
{
    public static bool IncludesLayer(this LayerMask mask, int layer) => mask == (mask | (1 << layer));
}
