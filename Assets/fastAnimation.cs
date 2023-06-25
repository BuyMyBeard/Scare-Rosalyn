using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fastAnimation : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Animator>().speed = 10;
    }

    
}
