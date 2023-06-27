using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : Objective
{
    [SerializeField] GameStateManager gameStateManager;
    public override void InteractionOver()
    {
        Check();
        if (GetComponentInChildren<CarDoors>().MonsterInCar)
            gameStateManager.Lose();
        else
            gameStateManager.Win();
    }
    private void OnTriggerEnter(Collider other)
    {
        VictimAI ai = other.GetComponent<VictimAI>();
        if (ai.CurrentObjective == this)
            ai.StartInteractWithObjectiveAnimation(this);
    }
}
