using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Spawner
{
    public class Obstacle : MonoBehaviour
    {
        public Action OnDisableAction;

        private void OnDisable()
        {
            OnDisableAction?.Invoke();
        }

        private void Update()
        {
            transform.Translate(Vector3.back * SyncManager.Instance.forwardSpeed * Time.deltaTime);
        }
    }
}