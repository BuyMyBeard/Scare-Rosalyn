using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitSpot : Objective
{
    public override void InteractionOver()
    {
    }
    private void OnTriggerEnter(Collider other)
    {
        VictimAI ai = other.GetComponent<VictimAI>();
        if (ai.CurrentObjective == this)
            ai.StartWaitAround(this);
    }
}
