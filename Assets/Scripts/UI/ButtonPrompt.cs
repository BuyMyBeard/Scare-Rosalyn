using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    ButtonPrompt buttonPrompt;
    public string promptMessage = "Interact";
    protected virtual void Awake()
    {
        buttonPrompt = FindObjectOfType<ButtonPrompt>();
    }
    protected void Prompt()
    {
        buttonPrompt.ProposePrompt(this);
    }
    protected void CancelPrompt()
    {
        buttonPrompt.CancelPrompt(this);
    }    
    public abstract void Interact();
}
public class ButtonPrompt : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textPrompt;
    [SerializeField] GameObject[] display;
    [SerializeField] float cooldown = 0.5f;
    List<Interactable> possiblePrompts = new();
    Interactable currentPrompt;
    PlayerInputs inputs;

    void Awake()
    {
        inputs = FindObjectOfType<PlayerInputs>();
    }
    void Update()
    {
        
        if (currentPrompt != null && inputs.InteractPress)
        {
            currentPrompt.Interact();
            CancelPrompt(currentPrompt);
            currentPrompt = null;
            HidePrompt();
            StartCoroutine(Cooldown());
        }
    }

    void LateUpdate()
    {
        if (possiblePrompts.Count == 0)
        {
            if (currentPrompt != null)
            {
                currentPrompt = null;
                HidePrompt();
            }    
            return;
        }
        if (possiblePrompts.Count == 1)
        {
            if (possiblePrompts.First() == currentPrompt)
                return;
            currentPrompt = possiblePrompts.First();
        }
        else
        {
            Interactable closestPrompt = possiblePrompts.First();
            float shortestDistance = (transform.position - closestPrompt.transform.position).magnitude;
            for (int i = 1; i < possiblePrompts.Count; i++)
            {
                float distance = (transform.position - possiblePrompts[i].transform.position).magnitude;
                if (distance < shortestDistance)
                {
                    shortestDistance = distance;
                    closestPrompt = possiblePrompts[i];
                }
            }
            currentPrompt = closestPrompt;
        }
        ShowPrompt(currentPrompt.promptMessage);
    }
    public void ProposePrompt(Interactable interactable)
    {
        possiblePrompts.Add(interactable);
    }
    public void CancelPrompt(Interactable interactable)
    {
        possiblePrompts.Remove(interactable);
    }
    void HidePrompt()
    {
        foreach (var c in display) c.SetActive(false);
    }
    void ShowPrompt(string message)
    {
        textPrompt.SetText(message);
        foreach (var c in display) c.SetActive(true);
    }
    
}
