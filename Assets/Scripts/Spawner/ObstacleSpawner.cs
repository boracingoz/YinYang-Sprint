using Assets.Scripts.Spawner;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ObstacleSpawner : MonoBehaviour
{
    [System.Serializable]
    public class SpawnPoint
    {
        public Transform transform;
        public float nextSpawnTime;
        public float currentSpawnRate;
    }

    [Header("Spawn Settings")]
    public SpawnPoint[] spawnPoints;
    public GameObject obstaclePrefab;
    public float intialSpawnRate = 2f;
    public float minSpawnRate = 0.5f;
    public float spawnRateDecrease = 0.1f;

    [Header("Lane  Settings")]
    public float laneOffset = 2f;

    private Queue<GameObject> _obstaclePool = new Queue<GameObject>();
    private int _poolSize = 10;

    private void Awake()
    {
        InitializeSpawnPoints();
        InitializePool();
    }

    private void InitializeSpawnPoints()
    {
        foreach (var point in spawnPoints)
        {
            if (point.transform == null)
            {
                Debug.LogWarning("Spawn point boş!");
            }
            point.currentSpawnRate = intialSpawnRate;
        }
    }

    private void InitializePool()
    {
        for (int i = 0; i < _poolSize; i++)
        {
            GameObject obj = Instantiate(obstaclePrefab);
            obj.SetActive(false);
            _obstaclePool.Enqueue(obj);
        }
    }

    GameObject GetPooledObstacle()
    {
        if (_obstaclePool.Count == 0)
        {
            ExpandPool();
        }
        GameObject obj = _obstaclePool.Dequeue();
        obj.SetActive(true);
        return obj;
    }

    private void ExpandPool()
    {
        for (int i = 0; i < 5; i++)
        {
            GameObject obj = Instantiate(obstaclePrefab);
            obj.SetActive(false);
            _obstaclePool.Enqueue(obj);
        }
    }

    private void Update()
    {
        foreach (SpawnPoint point in spawnPoints)
        {
            if (Time.time >= point.nextSpawnTime)
            {
                SpawnObstacle(point);
                UpdateSpawnRate(point);
            }
        }
    }


    private void SpawnObstacle(SpawnPoint spawnPoint)
    {
        GameObject obstacle = GetPooledObstacle();

        int lane = UnityEngine.Random.Range(-1, 2);
        Vector3 spawnPos = spawnPoint.transform.position + new Vector3(lane * laneOffset, 0, 0);

        obstacle.transform.position = spawnPos;
        obstacle.transform.rotation = spawnPoint.transform.rotation;

        Obstacle obstacleComponent = obstacle.GetComponent<Obstacle>();
        if (obstacleComponent != null)
        {
            obstacleComponent.OnDisableAction = () =>
            {
                if (obstacle.activeSelf)
                {
                    obstacle.SetActive(false);
                    _obstaclePool.Enqueue(obstacle);
                }
            };
        }

        spawnPoint.nextSpawnTime = Time.time + spawnPoint.currentSpawnRate;
    }


    private void UpdateSpawnRate(SpawnPoint point)
    {
        point.currentSpawnRate = Mathf.Max(minSpawnRate, intialSpawnRate - (spawnRateDecrease * Time.deltaTime));
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        foreach (SpawnPoint point in spawnPoints)
        {
            if (point.transform != null)
            {
                Gizmos.DrawWireCube(point.transform.position, Vector3.one);
                for (int i = -1; i <= 1; i++)
                {
                    Vector3 lanePos = point.transform.position + new Vector3(i * laneOffset, 0, 0);
                    Gizmos.DrawSphere(lanePos, 0.2f);
                }
            }
        }
    }
}
