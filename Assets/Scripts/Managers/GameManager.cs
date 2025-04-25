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
        public bool _isGameOver = false;
        public bool _isGameWon = false;

        [Header("UI Reference")]
        [SerializeField] private GameObject _gameOverPanel;
        [SerializeField] private GameObject _winPanel;


        private bool _isMusicOn = true;
        private bool _isSFXOn = true;

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

        void Start()
        {
            if (_gameOverPanel != null)
            {
                _gameOverPanel.SetActive(false);
            }

            if (_winPanel != null)
            {
                _winPanel.SetActive(false);
            }

            _isGameWon = false;
            _isGameOver = false;
            Time.timeScale = 1f;
        }

        public void ToggleMusic()
        {
            SoundManager.instance.SetMusicVolume(_isMusicOn ? 0f : 1f);
        }

        public void ToggleSFX()
        {
            SoundManager.instance.SetSFXVolume(_isSFXOn ? 0f : 1f); 
        }

        public void GameOver()
        {
            if (_isGameOver || _isGameWon) return;

            _isGameOver = true;
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

        public void Win()
        {
            if (_isGameWon || _isGameOver) return;

            _isGameWon = true;
            Debug.Log("WİN!");

            if (_winPanel != null)
            {
                _winPanel.SetActive(true);
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

        public void LoadMainMenu() 
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(0);
        }
    }
}