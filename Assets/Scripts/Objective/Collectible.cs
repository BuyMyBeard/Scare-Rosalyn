using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : Objective
{
    public override void InteractionOver()
    {
        gameObject.SetActive(false);
        Check();
    }

    private void OnTriggerEnter(Collider other)
    {
        VictimAI ai = other.GetComponent<VictimAI>();
        if (ai.CurrentObjective == this)
            ai.StartInteractWithObjectiveAnimation(this);
    }
}
