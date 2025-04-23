using Assets.Scripts.Spawner;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Managers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;

        [Header("Game State")]
        public bool isGameOver = false;

        [Header("UI Reference")]
        [SerializeField] private GameObject _gameOverPanel;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
                return;
            }
        }

        // Use this for initialization
        void Start()
        {
            if (_gameOverPanel != null)
            {
                _gameOverPanel.SetActive(false);
            }

            isGameOver = false;
            Time.timeScale = 1f;
        }

        public void GameOver()
        {
            if (isGameOver) return;

            isGameOver = true;
            Debug.Log("GameOver");

            if (_gameOverPanel != null)
            {
                _gameOverPanel.SetActive(true);
            }

            foreach (var obstacle in FindObjectsOfType<Obstacle>())
            {
                Rigidbody rb = obstacle.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.velocity = Vector3.zero;
                    rb.isKinematic = true;
                }
            }

            Time.timeScale = 0f;
        }
        
        public void RestartGame()
        {
            Time.timeScale = 1f;

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}