using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weaver : MonoBehaviour
{
    public KeyCode keybindLeft;
    public KeyCode keybindRight;

    [Range(0, 1)] public float weaveSpeed = 0.1f;
    public float weaveDegrees = 45;

    [Range(0, 1)] public float retractionSpeed = 0.8f;

    private Vector3 startPos;
    private Vector3 startRot;

    private void Start()
    {
        startPos = transform.localPosition;
        startRot = transform.localEulerAngles;

        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(keybindRight)) // Weave right
        {
            Weave(-weaveDegrees);
        }
        else if (Input.GetKey(keybindLeft)) // Weave right
        {
            Weave(weaveDegrees);
        }
        else // Return to starting position if no binds pressed
        {
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(startRot), retractionSpeed);
            transform.localPosition = Vector3.Lerp(transform.localPosition, startPos, retractionSpeed);
        }
    }

    private void Weave(float degrees)
    {
        var weaveDeg = new Vector3(0, 0, degrees);
        transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(startRot + weaveDeg), weaveSpeed);
    }
}
