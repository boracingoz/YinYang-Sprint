using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager instance;

        public const string MASTER_VOL_KEY = "MasterVolume";
        public const string MUSIC_VOL_KEY = "MusicVolume";
        public const string SFX_VOL_KEY = "SFXVolume";

        [Header("Audio Sources")]
        [SerializeField] private AudioSource _musicSource;
        [SerializeField] private AudioSource _sfxSource;

        [Header("Background Music")]
        [SerializeField] private AudioClip _mainMenuMusic;
        [SerializeField] private AudioClip _gameMusic;

        [Header("Game Result Sounds")]
        [SerializeField] private AudioClip _winSound;
        [SerializeField] private AudioClip _gameOverSound;

        [Header("SFX Sounds")]
        [SerializeField] private AudioClip _runningSound;
        [SerializeField] private AudioClip _jumpSound;
        [SerializeField] private AudioClip _collectCoinSound;

        // Volume levels
        private float _masterVolume = 1f;
        private float _musicVolume = 1f;
        private float _sfxVolume = 1f;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
                return;
            }

            if (_musicSource == null)
            {
                _musicSource = gameObject.AddComponent<AudioSource>();
                _musicSource.loop = true;
            }

            if (_sfxSource == null)
            {
                _sfxSource = gameObject.AddComponent<AudioSource>();
                _sfxSource.loop = false;
            }
        }

        private void Start()
        {
            LoadVolumeSettings();
            PlayMainMenuMusic();
        }

        private void OnEnable()
        {
            UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
        }


        #region Music Methods

        public void PlayMainMenuMusic()
        {
            if (_musicSource == null || _mainMenuMusic == null) return;

            _musicSource.clip = _mainMenuMusic;
            _musicSource.Play();
            Debug.Log("Playing main menu music");
        }

        public void PlayGameMusic()
        {
            if (_musicSource == null || _gameMusic == null) return;

            // Stop previous music if playing
            if (_musicSource.isPlaying)
            {
                _musicSource.Stop();
            }

            _musicSource.clip = _gameMusic;
            _musicSource.Play();
            Debug.Log("Playing game music");
        }

        #endregion

        #region SFX Methods

        public void PlayRunningSound()
        {
            PlaySFX(_runningSound);
        }

        public void PlayJumpSound()
        {
            PlaySFX(_jumpSound);
        }

        public void PlayCoinCollectSound()
        {
            PlaySFX(_collectCoinSound);
        }

        public void PlayWinSound()
        {
            PlaySFX(_winSound);
        }

        public void PlayGameOverSound()
        {
            PlaySFX(_gameOverSound);
        }

        public void PlaySFX(AudioClip clip)
        {
            if (_sfxSource == null || clip == null) return;

            _sfxSource.PlayOneShot(clip, _sfxVolume * _masterVolume);
        }

        #endregion

        #region Volume Settings

        public void LoadVolumeSettings()
        {
            _masterVolume = PlayerPrefs.GetFloat(MASTER_VOL_KEY, 1f);
            _musicVolume = PlayerPrefs.GetFloat(MUSIC_VOL_KEY, 1f);
            _sfxVolume = PlayerPrefs.GetFloat(SFX_VOL_KEY, 1f);

            ApplyVolumeSettings();

            Debug.Log($"Loaded volume settings - Master: {_masterVolume}, Music: {_musicVolume}, SFX: {_sfxVolume}");
        }

        public void SaveVolumeSettings()
        {
            PlayerPrefs.SetFloat(MASTER_VOL_KEY, _masterVolume);
            PlayerPrefs.SetFloat(MUSIC_VOL_KEY, _musicVolume);
            PlayerPrefs.SetFloat(SFX_VOL_KEY, _sfxVolume);
            PlayerPrefs.Save();

            Debug.Log($"Saved volume settings - Master: {_masterVolume}, Music: {_musicVolume}, SFX: {_sfxVolume}");
        }

        private void ApplyVolumeSettings()
        {
            if (_musicSource != null)
            {
                _musicSource.volume = _musicVolume * _masterVolume;
            }
        }

        public void SetMasterVolume(float volume)
        {
            _masterVolume = volume;
            ApplyVolumeSettings();
            SaveVolumeSettings();
        }

        public void SetMusicVolume(float volume)
        {
            _musicVolume = volume;
            if (_musicSource != null)
            {
                _musicSource.volume = _musicVolume * _masterVolume;
            }
            SaveVolumeSettings();
        }

        private void OnDisable()
        {
            UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
        {
            Debug.Log($"SoundManager: Scene loaded - {scene.name}");

            if (scene.name == "MainMenu")
            {
                PlayMainMenuMusic();
            }
            else if (scene.name == "Game")
            {
                PlayGameMusic();
            }
        }

        public void SetSFXVolume(float volume)
        {
            _sfxVolume = volume;
            SaveVolumeSettings();
        }

        #endregion
    }
}