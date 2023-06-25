using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Expressions { Neutral, Scared, Dead, Blinking }
public class Expression : MonoBehaviour
{
    [SerializeField] Expressions currentExpression = Expressions.Neutral;
    [SerializeField] float blinkFrequency = 6;
    [SerializeField] Animator animator;
    void Awake()
    {
        animator.Play(currentExpression.ToString());
    }

    private void OnEnable()
    {
        StartCoroutine(Blink());
    }
    public void ChangeExpression(Expressions expression)
    {
        currentExpression = expression;
        animator.Play(expression.ToString());
    }

    IEnumerator Blink()
    {
        while (true)
        {
            yield return new WaitForSeconds(blinkFrequency);
            if (currentExpression == Expressions.Neutral)
            {
                currentExpression = Expressions.Blinking;
                animator.Play(currentExpression.ToString());
                yield return new WaitForSeconds(0.5f);
                if (currentExpression != Expressions.Blinking)
                    continue;
                currentExpression = Expressions.Neutral;
                animator.Play(currentExpression.ToString());
            }
        }
    }
}
