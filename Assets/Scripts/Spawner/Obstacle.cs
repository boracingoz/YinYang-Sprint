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


            if (obstacleSettings == null)
            {
                _moveSpeed = Assets.Scripts.SyncManager.Instance.forwardSpeed * 1.2f;
            }
            else
            {
                _moveSpeed = Assets.Scripts.SyncManager.Instance.forwardSpeed * obstacleSettings.speedMultiplier;
            }
        }

        private void OnDisable()
        {
            OnDisableAction?.Invoke();
        }

        private void Update()
        {
            if (GameManager.instance != null && GameManager.instance.isGameOver) return;

            //float speed = Assets.Scripts.SyncManager.Instance.obstacleSpeed;
            transform.Translate(Vector3.back * _moveSpeed * Time.deltaTime);

            float despawnDistance = obstacleSettings != null ? obstacleSettings.despawnDistance : -10f;
            if (transform.position.z < despawnDistance)
            {
                gameObject.SetActive(false);
            }
        }

       
    }
}