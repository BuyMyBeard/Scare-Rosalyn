using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarDoors : Interactable
{
    [SerializeField] PlayerMovement movement;
    [SerializeField] BackgroundFade backgroundFade;
    BoxCollider trigger;
    public bool MonsterInCar { get; private set; } = false;

    protected override void Awake()
    {
        base.Awake();
        trigger = GetComponent<BoxCollider>();
    }
    public override void Interact()
    {
        if (MonsterInCar)
        {
            movement.ExitCar();
            StartCoroutine(backgroundFade.FadeIn(true));
            promptMessage = "Enter car";
        }
        else
        {
            movement.EnterCar();
            StartCoroutine(backgroundFade.FadeIn());
            promptMessage = "Exit car";
        }
        MonsterInCar = !MonsterInCar;
        StartCoroutine(BlockInteraction());
    }
    IEnumerator BlockInteraction()
    {
        trigger.enabled = false;
        yield return new WaitUntil(() => backgroundFade.Done);
        trigger.enabled = true;
    }


}
