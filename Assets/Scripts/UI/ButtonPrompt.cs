using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public abstract class Interactable : MonoBehaviour, IComparer<Interactable>
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

    public int Compare(Interactable x, Interactable y)
    {
        float distanceX = (x.transform.position - transform.position).magnitude;
        float distanceY = (y.transform.position - transform.position).magnitude;
        if (distanceX == distanceY)
            return 0;
        else if (distanceX > distanceY)
            return 1;
        return -1;
    }
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
        possiblePrompts.Sort();
        if (possiblePrompts.First() == currentPrompt)
            return;
        currentPrompt = possiblePrompts.First();
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
