using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableRat : MonoBehaviour
{
    public static CollectableRat Instance;

    private void Awake() // Destroy if more than one rat spawned
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Update()
    {
        if (RatHolder.instance.hasRat)
            Destroy(gameObject);
    }
}
