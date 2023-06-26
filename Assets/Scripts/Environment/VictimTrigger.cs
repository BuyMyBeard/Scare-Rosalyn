using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictimTrigger : MonoBehaviour
{
    public bool Triggered { get; private set; } = false;

    private void OnTriggerEnter(Collider other)
    {
        Triggered = true;
    }
    private void OnTriggerExit(Collider other)
    {
        Triggered = false;
    }
}
