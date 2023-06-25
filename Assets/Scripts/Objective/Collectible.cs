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
        other.GetComponent<VictimAI>().InteractWithObjective(this);
    }
}
