using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEngine.UI.Button;

public class Checkbox : MonoBehaviour, IPointerClickHandler
{

    [SerializeField] GameObject checkmark;
    [SerializeField] ButtonClickedEvent m_OnClick = new ButtonClickedEvent();

    public bool Value
    {
        get => checkmark.activeSelf;
        set => checkmark.SetActive(value);
            
    }
    public void Check()
    {
       checkmark.SetActive(true);
    }
    public void UnCheck()
    {
        checkmark.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Value = !Value;
        m_OnClick.Invoke();
    }
}
