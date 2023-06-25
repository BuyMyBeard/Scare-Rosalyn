using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckboxList : MonoBehaviour
{
    [SerializeField] Checkbox[] checkboxes;

    public void Check(int i)
    {
        checkboxes[i].Check();
    }
}
