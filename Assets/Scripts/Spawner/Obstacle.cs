using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Spawner
{
    public class Obstacle : MonoBehaviour
    {
        [SerializeField] private float _speedMultiplier = 1.2f;
        public Action OnDisableAction;

        private float _moveSpeed;


        private void OnEnable()
        {
            _moveSpeed = Assets.Scripts.SyncManager.Instance.forwardSpeed * _speedMultiplier;
        }

        private void OnDisable()
        {
            OnDisableAction?.Invoke();
        }

        private void Update()
        {
            transform.Translate(Vector3.back * _moveSpeed * Time.deltaTime);

            if (transform.position.z < -10f)
            {
                gameObject.SetActive(false);
            }
        }
    }
}