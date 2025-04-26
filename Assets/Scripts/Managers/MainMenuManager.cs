using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts.Managers
{
    public class MainMenuManager : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private Button _playButton;
        [SerializeField] private Button _settingsButton;
        [SerializeField] private GameObject _settingsPanel;

        private void Awake()
        {
            _playButton.onClick.AddListener(PlayGame);
            _settingsButton.onClick.AddListener(OpenSettings);
            SoundManager.instance.LoadVolumeSettings();
        }

        public void PlayGame()
        {
            SceneManager.LoadScene("Game");
        }

        public void OpenSettings() 
        {
            _settingsPanel.SetActive(true);
        }
    }

}
