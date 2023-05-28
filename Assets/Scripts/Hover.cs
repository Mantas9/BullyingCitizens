using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hover : MonoBehaviour
{
    public float height = 0.5f;
    public float speed = 5;

    private Vector3 startPos;
    private float offset;
    public bool randomize = true;

    private void Start()
    {
        startPos = transform.localPosition;
        if (randomize) offset = Random.Range(0f, 1000f);

    }

    private void Update()
    {
        transform.localPosition = startPos + Vector3.up * Mathf.Sin(offset + Time.time * speed) * height;
    }
}
