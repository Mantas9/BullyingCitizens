using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointHitter : MonoBehaviour
{
    public LayerMask mask;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            var ray = new Ray(transform.position, transform.forward);
            if (!Physics.Raycast(ray, out var hit, 100f, mask))
                return;

            if(!hit.transform.gameObject.TryGetComponent<PunchPoint>(out var punchPoint))
            {
                Debug.LogError("No punchpoint script attached");
                return;
            }

            punchPoint.GotHit();
        }
    }
}
