using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionCone : MonoBehaviour
{
    [SerializeField] VictimAI victimAI;
    private void Awake()
    {
        if (victimAI == null)
            throw new ArgumentNullException(nameof(victimAI));
    }
    private void OnTriggerStay(Collider other)
    {
        victimAI.DetectMonster(other);
    }
    private void OnTriggerExit(Collider other)
    {
        victimAI.StopDetectingMonster();
    }
}
