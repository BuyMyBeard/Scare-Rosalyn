using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : Objective
{
    public override void InteractionOver()
    {
        Check();
    }
    private void OnTriggerEnter(Collider other)
    {
        other.GetComponent<VictimAI>().InteractWithObjective(this);
    }
}
