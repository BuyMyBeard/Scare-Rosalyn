using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonnalBubble : MonoBehaviour
{
    VictimAI victim;
    private void Awake()
    {
        victim = transform.parent.GetComponent<VictimAI>();
    }
    private void OnTriggerStay(Collider other)
    {
        victim.MonsterVeryClose();
    }
}
