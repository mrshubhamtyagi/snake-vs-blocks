using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointSpawner : MonoBehaviour
{
    public Transform pointPrefab;

    private Vector3 newPosition;

    void Start()
    {
        SpawnPoint();
        SpawnPoint();
        SpawnPoint();
        SpawnPoint();
        SpawnPoint();
        SpawnPoint();
        SpawnPoint();
        SpawnPoint();
        SpawnPoint();
        SpawnPoint();
    }

    private void SpawnPoint()
    {
        Transform point = Instantiate(pointPrefab);
        newPosition.x = Random.Range(-2, 2);
        newPosition.y += Random.Range(5, 10);
        point.position = newPosition;
        point.SetParent(transform);
    }
}
