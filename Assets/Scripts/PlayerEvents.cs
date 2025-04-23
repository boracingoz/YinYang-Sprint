using Assets.Scripts.Managers;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts
{
    public class PlayerEvents : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private CustomCharacterController _characterController;

        [Header("Events")]
        public UnityEvent onPlayerDeath;

        // Use this for initialization
        void Start()
        {
            if (_characterController != null)
            {
                _characterController = GetComponent<CustomCharacterController>();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Assets.Scripts.Spawner.Obstacle>() != null)
            {
                Die();
            }
        }

        public void Die()
        {
            onPlayerDeath?.Invoke();
            GameManager gameManager = FindObjectOfType<GameManager>();

            if (gameManager != null)
            {
                gameManager.GameOver();
            }
        }
    }
}