using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FenceGate : MonoBehaviour
{
    float totalDistance = 4.78f;
    public void Open()
    {
        GetComponent<AudioManager>().PlaySFX(0);
        StartCoroutine(OpenAnimation());
    }

    IEnumerator OpenAnimation()
    {
        for (float t = 0; t < 5; t += Time.deltaTime)
        {
            transform.Translate(Time.deltaTime / 5 * totalDistance, 0, 0);
            Debug.Log(transform.position);
            yield return null;
        }
    }
}
