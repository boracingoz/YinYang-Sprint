using Assets.Scripts.Spawner;
using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class DifficultyManager : MonoBehaviour
    {
        public static DifficultyManager instance;

        [Header("Diffuculty Settings")]
        public float baseObstacleSpeed = 2.4f;
        public float increadeSpeedMultiplier = 1.2f;

        [Header("References")]
        public ObstacleSpawner[] obstacleSpawners;

        private float player1ObstacleSpeed;
        private float player2ObstacleSpeed;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

            player1ObstacleSpeed = baseObstacleSpeed;
            player2ObstacleSpeed = baseObstacleSpeed;
        }

        void Start()
        {
            if (obstacleSpawners == null || obstacleSpawners.Length == 0)
            {
                obstacleSpawners = FindObjectsOfType<ObstacleSpawner>();
            }
        }

        public void UpdateDifficulty()
        {
            int leadingPlayer = SoulCollector.instance.GetLeadinPLayer();

            if (SoulCollector.instance.IsBalanced())
            {
                ResetObstacleSpeeds();
                Debug.Log("Game Balanced! Normal seped for both players");
            }
            else if (leadingPlayer == 1)
            {
                player1ObstacleSpeed = baseObstacleSpeed;
                player2ObstacleSpeed = baseObstacleSpeed * increadeSpeedMultiplier;
                Debug.Log("Player 1 leading!  Increased obstacle Speed for player 2.");
            }
            else if (leadingPlayer == 2)
            {
                player1ObstacleSpeed = baseObstacleSpeed * increadeSpeedMultiplier;
                player2ObstacleSpeed = baseObstacleSpeed;
                Debug.Log("Player 2 leading!  Increased obstacle Speed for player 1.");
            }

            UpdateObstacleSpawners();
        }

        private void ResetObstacleSpeeds()
        {
            player1ObstacleSpeed = baseObstacleSpeed;
            player2ObstacleSpeed = baseObstacleSpeed;
        }

        private void UpdateObstacleSpawners()
        {
            Obstacle[] activeObstacles = FindObjectsOfType<Obstacle>();
            foreach (var obstacle in activeObstacles)
            {
                obstacle.UpdateSpeed();
            }
        }

        public float GetObstacleSpeedForCharacter(bool isArrowKeyUsers)
        {
            return isArrowKeyUsers ? player2ObstacleSpeed : player1ObstacleSpeed;

        }
    }
}