using DitzeGames.Effects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchPointsSpawner : MonoBehaviour
{
    public PunchSpeedGame parent;

    public Transform canvasToSpawnOn;
    public GameObject punchPointPrefab;
    public GameObject avoidPointPrefab;
    public LayerMask mask;

    public List<GameObject> punchPoints;

    private void Start()
    {
        if (parent == null)
            parent = GetComponentInParent<PunchSpeedGame>();
        if (canvasToSpawnOn == null)
            canvasToSpawnOn = transform.parent;
    }

    public void SpeedGameSpawn(int amount, bool avoidPoints = false, float precentage = 15)
    {
        for (int i = 0; i < amount; i++)
        {
            var colliderPoint = GetRandomPointInsideCollider(canvasToSpawnOn.GetComponent<BoxCollider>());
            colliderPoint.z = canvasToSpawnOn.transform.position.z;

            var prefab = punchPointPrefab;
            if (avoidPoints && (int)Random.Range(0, 100) <= precentage)
                prefab = avoidPointPrefab;

            var punchPoint = Instantiate(prefab, colliderPoint, prefab.transform.rotation, transform.parent.parent);
            punchPoints.Add(punchPoint);
        }
    }

    public void PunchPointHit(GameObject punchPoint)
    {
        punchPoints.Remove(punchPoint);
        Destroy(punchPoint);

        foreach (var point in punchPoints)
        {
            if (!point.CompareTag("AvoidPunchPoint"))
                return;
        }

        parent.EndMiniGame(true);
    }

    public void ClearPoints()
    {
        foreach (var item in punchPoints)
        {
            Destroy(item);
        }

        punchPoints = new List<GameObject>();
    }

    public Vector3 GetRandomPointInsideCollider(BoxCollider boxCollider)
    {
        Vector3 extents = boxCollider.size / 2f;
        Vector3 point = new Vector3(
            Random.Range(-extents.x, extents.x),
            Random.Range(-extents.y, extents.y),
            Random.Range(-extents.z, extents.z)
        ) + boxCollider.center;
        return boxCollider.transform.TransformPoint(point);
    }

}
