using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Checkbox : MonoBehaviour
{

    [SerializeField] GameObject checkmark;
    public void Check()
    {
       checkmark.SetActive(true);
    }
}
