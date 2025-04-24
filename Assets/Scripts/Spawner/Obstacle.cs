using Assets.Scripts.Managers;
using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Spawner
{
    public class Obstacle : MonoBehaviour
    {
        public ObstacleSettings obstacleSettings;
        public Action OnDisableAction;

        [Header("Character Target")]
        public bool isForArrowKeyUser = false;

        private float _moveSpeed;
        private Collider _collider;

        private void Awake()
        {
            _collider = GetComponent<Collider>();

            if (_collider != null && !_collider.isTrigger)
            {
                _collider.isTrigger = true;
            }
        }


        private void OnEnable()
        {
            if (SyncManager.Instance == null)
            {
                Debug.LogError("Sync Manager not initialized!");
                return;
            }


            UpdateSpeed();
        }

        private void UpdateSpeed()
        {
            if (DifficultyManager.instance != null)
            {
                float characterSpeed = DifficultyManager.instance.GetObstacleSpeedForCharacter(isForArrowKeyUser);

                if (obstacleSettings == null)
                {
                    _moveSpeed = characterSpeed;
                }
                else
                {
                    _moveSpeed = characterSpeed * obstacleSettings.speedMultiplier;
                }
            }
            else
            {
                if (obstacleSettings == null)
                {
                    _moveSpeed = Assets.Scripts.SyncManager.Instance.forwardSpeed * 1.2f;
                }
                else
                {
                    _moveSpeed = Assets.Scripts.SyncManager.Instance.forwardSpeed * obstacleSettings.speedMultiplier;
                }
            }
        }

        private void OnDisable()
        {
            OnDisableAction?.Invoke();
        }

        private void Update()
        {
            if (GameManager.instance != null && GameManager.instance._isGameOver) return;

            transform.Translate(Vector3.back * _moveSpeed * Time.deltaTime);

            float despawnDistance = obstacleSettings != null ? obstacleSettings.despawnDistance : -10f;
            if (transform.position.z < despawnDistance)
            {
                gameObject.SetActive(false);
            }
        }

       
    }
}