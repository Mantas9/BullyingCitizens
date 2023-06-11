using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableDisableAfter : MonoBehaviour
{
    private void Start()
    {
        Invoke("Disable", Time.deltaTime);
    }

    private void Disable()
    {
        gameObject.SetActive(false);
    }
}
