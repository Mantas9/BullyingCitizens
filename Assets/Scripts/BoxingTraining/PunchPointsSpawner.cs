using DitzeGames.Effects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchPointsSpawner : MonoBehaviour
{
    public PunchingGames parent;

    public float powerGameInterval = 2f;

    private AudioSource source;
    public AudioClip hitClip;

    public Transform canvasToSpawnOn;
    public GameObject punchPointPrefab;
    public GameObject avoidPointPrefab;
    public GameObject watchOutPrefab;

    public bool speedGame = false;
    public bool gameInProgress;

    public List<GameObject> punchPoints;

    private void Start()
    {
        if (parent == null)
            parent = GetComponentInParent<PunchingGames>();
        if (canvasToSpawnOn == null)
            canvasToSpawnOn = transform.parent;

        source = GetComponent<AudioSource>();
    }

    public void SpeedGameSpawn(int amount, bool avoidPoints = false, float percentage = 35)
    {
        for (int i = 0; i < amount; i++)
        {
            var colliderPoint = GetRandomPointInsideCollider(canvasToSpawnOn.GetComponent<BoxCollider>());
            colliderPoint.z = canvasToSpawnOn.transform.position.z;

            var prefab = punchPointPrefab;
            if (avoidPoints && (int)Random.Range(0, 100) <= percentage)
            {
                prefab = avoidPointPrefab;
                i--;
            }

            var punchPoint = Instantiate(prefab, colliderPoint, prefab.transform.rotation, transform.parent.parent);
            punchPoints.Add(punchPoint);
        }
    }

    public IEnumerator PowerGameSpawn(int levelUpEverySection, int sectionsUntilAvoidPoints = 3, int sectionsUntilDodging = 5, float avoidPointPrecentage = 30, float dodgePrecentage = 15, float doublePrecentage = 30)
    {
        int cycles = 0;

        while (gameInProgress)
        {
            cycles++;
            if (cycles % levelUpEverySection == 0)
                parent.UpgradePower();

            float moreTime = 0;

            // PunchPointPrefab
            var colliderPoint = GetRandomPointInsideCollider(canvasToSpawnOn.GetComponent<BoxCollider>());
            var punchPoint = Instantiate(punchPointPrefab, colliderPoint, punchPointPrefab.transform.rotation, transform.parent.parent);
            punchPoints.Add(punchPoint);

            if (Random.Range(0, 100) <= doublePrecentage)
            {
                colliderPoint = GetRandomPointInsideCollider(canvasToSpawnOn.GetComponent<BoxCollider>());
                var secondPunchPoint = Instantiate(punchPointPrefab, colliderPoint, punchPointPrefab.transform.rotation, transform.parent.parent);
                punchPoints.Add(secondPunchPoint);
                doublePrecentage += 5;

                moreTime += 0.4f;
            }

            if (cycles >= sectionsUntilAvoidPoints && Random.Range(0, 100) <= avoidPointPrecentage) // Avoid punching prefab
            {
                colliderPoint = GetRandomPointInsideCollider(canvasToSpawnOn.GetComponent<BoxCollider>());

                var avoidPoint = Instantiate(avoidPointPrefab, colliderPoint, avoidPointPrefab.transform.rotation, transform.parent.parent);
                punchPoints.Add(avoidPoint);
                avoidPointPrecentage += 5;
            }

            if (cycles >= sectionsUntilDodging && Random.Range(0, 100) <= dodgePrecentage) // Dodge Mechanic
            {
                watchOutPrefab.GetComponent<Animator>().SetBool("Activate", true);
                yield return null;
                watchOutPrefab.GetComponent<Animator>().SetBool("Activate", false);
                dodgePrecentage += 5;

                moreTime = +1f;
            }

            yield return parent.CountdownRoutine(powerGameInterval + moreTime, true);

            foreach (var point in punchPoints)
            {
                if (!point.CompareTag("AvoidPunchPoint"))
                {
                    parent.EndPowerMiniGame();
                    yield break;
                }
            }

            ClearPoints();
        }
    }

    public void PunchPointHit(GameObject punchPoint)
    {
        source.PlayOneShot(hitClip, 0.9f);

        if (punchPoint.CompareTag("AvoidPunchPoint"))
        {
            parent.EndAllGames();
            ClearPoints();
            return;
        }

        punchPoints.Remove(punchPoint);
        Destroy(punchPoint);

        CameraEffects.ShakeOnce(0.1f, 10);

        if (speedGame)
        {
            foreach (var point in punchPoints)
            {
                if (!point.CompareTag("AvoidPunchPoint"))
                    return;
            }

            parent.EndSpeedMiniGame(true);
        }
    }

    public void ClearPoints()
    {
        foreach (var item in punchPoints)
        {
            Destroy(item);
        }

        punchPoints = new List<GameObject>();
    }

    private Vector3 GetRandomPointInsideCollider(BoxCollider boxCollider)
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
