using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Objective : MonoBehaviour
{
    public Transform objectivePosition;
    public bool done = false;
    public int angleToLookAt = 0;
    public int waitTime = 0;
    [SerializeField] int collectibleId;
    [SerializeField] CheckboxList checkboxList;

    public abstract void InteractionOver();
    protected void Check()
    {
        checkboxList.Check(collectibleId);
    }
}