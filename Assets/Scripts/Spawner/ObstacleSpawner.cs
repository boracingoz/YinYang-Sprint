using Assets.Scripts.Managers;
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
    public GameObject[] obstaclePrefab;
    public float intialSpawnRate = 2f;
    public float minSpawnRate = 0.5f;
    public float spawnRateDecrease = 0.1f;

    [Header("Lane  Settings")]
    public float laneOffset = 2f;

    private Dictionary <int, Queue<GameObject>> _obstaclePool = new Dictionary <int, Queue<GameObject>>();
    private int _poolSizePerType = 5;

    private Transform _obstacleParent;

    private void Awake()
    {
        CreateObstacleParent();
        InitializeSpawnPoints();
        InitializePool();
    }

    private void CreateObstacleParent()
    {
        _obstacleParent = new GameObject("SpawnObstacles").transform;
        _obstacleParent.SetParent(transform);
        _obstacleParent.localPosition = Vector3.zero;
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
        if (obstaclePrefab.Length == 0)
        {
            Debug.LogError("obstacle atanmamış!");
            return;
        }


        for (int i = 0; i <obstaclePrefab.Length; i++)
        {
            Queue<GameObject> pool = new Queue<GameObject> ();
            _obstaclePool[i] = pool;
            for (int j = 0; j < _poolSizePerType; j++)
            {
                CreateNewObstacle(i);
            }
        }
    }

    private GameObject CreateNewObstacle(int obstacleTypeIndex)
    {
        if (obstacleTypeIndex <0 || obstacleTypeIndex >= obstaclePrefab.Length)
        {
            Debug.LogError("Geçersiz engel indeksi: " + obstacleTypeIndex);
            return null;
        }

        GameObject obj = Instantiate(obstaclePrefab[obstacleTypeIndex], _obstacleParent);
        obj.SetActive(false);
        _obstaclePool[obstacleTypeIndex].Enqueue(obj);
        return obj;
    }

    GameObject GetPooledObstacle(int obstacleTypeIndex)
    {
        if (!_obstaclePool.ContainsKey(obstacleTypeIndex))
        {
            Debug.LogError("Bu tipte engel yok: " + obstacleTypeIndex);
            return null;
        }

        Queue<GameObject> pool = _obstaclePool[obstacleTypeIndex];

        if (pool.Count == 0)
        {
            return CreateNewObstacle(obstacleTypeIndex);
        }

        GameObject obj = pool.Dequeue();
        obj.SetActive(true);
        return obj;
    }

    private void Update()
    {
        if (GameManager.instance != null && GameManager.instance._isGameOver) return;

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
        int obstacleTypeIndex = UnityEngine.Random.Range(0,obstaclePrefab.Length);
        GameObject obstacle = GetPooledObstacle(obstacleTypeIndex);

        if (obstacle == null) return;

        int lane = UnityEngine.Random.Range(-1, 2);
        Vector3 spawnPos = spawnPoint.transform.position + new Vector3(lane * laneOffset, 0, 0);

        obstacle.transform.position = spawnPos;
        obstacle.transform.rotation = spawnPoint.transform.rotation;

        Obstacle obstacleComponent = obstacle.GetComponent<Obstacle>();
        if (obstacleComponent != null)
        {
            int capturedTypeIndex = obstacleTypeIndex;
            obstacleComponent.OnDisableAction = () =>
            {
                if (obstacle != null && obstacle.activeSelf)
                {
                    obstacle.SetActive(false);
                    _obstaclePool[capturedTypeIndex].Enqueue(obstacle);
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
