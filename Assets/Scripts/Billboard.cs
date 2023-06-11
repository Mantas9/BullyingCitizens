using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Transform target;
    public bool flip;

    private void Start()
    {
        target = Camera.main.transform;
    }

    private void Update()
    {
        transform.LookAt(target);
        if (flip)
            transform.Rotate(0, 180, 0);
    }
}
