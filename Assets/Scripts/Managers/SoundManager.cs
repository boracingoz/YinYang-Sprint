using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [Header("Audio Mixer")]
    [SerializeField] private AudioMixer _audioMixer;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource _musicSource;
    [SerializeField] private AudioSource _sfxSource;

    [Header("Music Clips")]
    [SerializeField] private AudioClip _menuMusic; 
    [SerializeField] private AudioClip _gameMusic;

    [Header("Settings")]
    [SerializeField] private bool _enableSFXInMenu = false;

    [Header("Debug")]
    [SerializeField] private string _currentSceneName;
    [SerializeField] private bool _debugMode = true;


    public const string MASTER_VOL_KEY = "MasterVolume";
    public const string MUSIC_VOL_KEY = "MusicVolume";
    public const string SFX_VOL_KEY = "SFXVolume";

    private const string MASTER_PARAM = "MasterVolume";
    private const string MUSIC_PARAM = "MusicVolume";
    private const string SFX_PARAM = "SFXVolume";

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;

            _currentSceneName= SceneManager.GetActiveScene().name;


            if (_debugMode)
            {
                Debug.Log($"SoundManager created. Current scene: {_currentSceneName}");
            }
        }
        else
        {
            if (_debugMode)
            {
                Debug.Log("SoundManager instance already exists. Destroying duplicate.");
            }
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        LoadVolumeSettings();
        if (_musicSource != null)
            _musicSource.enabled = true;

        if (_sfxSource != null)
            _sfxSource.enabled = true;

        HandleSceneAudio(_currentSceneName);
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        _currentSceneName = scene.name;
        if (_debugMode)
        {
            Debug.Log($"Scene loaded: {_currentSceneName}");
        }

        HandleSceneAudio(_currentSceneName);

        _enableSFXInMenu = (scene.name == "Game");
    }

    private void HandleSceneAudio(string sceneName)
    {
        if (sceneName == "MainMenu" || SceneManager.GetActiveScene().buildIndex == 0)
        {
            if (_debugMode)
                Debug.Log("Main Menu scene detected. Playing menu music.");

            _enableSFXInMenu = false;
            PlayMenuMusic();
        }
        else 
        {
            if (_debugMode)
                Debug.Log("Game scene detected. Playing game music.");

            _enableSFXInMenu = true;
            PlayGameMusic();
        }
    }

    public void PlayMenuMusic()
    {
        if (_musicSource == null)
        {
            Debug.LogError("Music source null!");
            return;
        }

        if (_menuMusic == null)
        {
            Debug.LogWarning("Menu music clip is not assigned");
            return;
        }

        _musicSource.enabled = true;

        _musicSource.Stop();
        _musicSource.clip = _menuMusic;
        _musicSource.Play();

        if (_debugMode)
            Debug.Log("Menu music started playing.");
    }

    public void PlayGameMusic()
    {
        if (_musicSource == null)
        {
            Debug.LogError("Music source is null in SoundManager!");
            return;
        }

        if (_gameMusic == null)
        {
            Debug.LogWarning("Game music clip is not assigned in SoundManager!");
            return;
        }

        _musicSource.enabled = true;

        _musicSource.Stop();
        _musicSource.clip = _gameMusic;
        _musicSource.Play();

        if (_debugMode)
            Debug.Log("Game music started playing.");
    }

    public void LoadVolumeSettings()
    {
        SetMasterVolume(PlayerPrefs.GetFloat(MASTER_VOL_KEY, 1f));
        SetMusicVolume(PlayerPrefs.GetFloat(MUSIC_VOL_KEY, 1f));
        SetSFXVolume(PlayerPrefs.GetFloat(SFX_VOL_KEY, 1f));
    }

    public void SetMasterVolume(float v)
    {
        try
        {
            float safeVolume = Mathf.Max(0.0001f, v);
            _audioMixer.SetFloat(MASTER_PARAM, Mathf.Log10(safeVolume) * 20);
            PlayerPrefs.SetFloat(MASTER_VOL_KEY, v);

            if (_debugMode)
                Debug.Log($"Set master volume: {v}");
        }
        catch (Exception e)
        {
            Debug.LogError($"Error setting master volume: {e.Message}");
        }
    }

    public void SetMusicVolume(float v)
    {
        try
        {
            float safeVolume = Mathf.Max(0.0001f, v);
            _audioMixer.SetFloat(MUSIC_PARAM, Mathf.Log10(safeVolume) * 20);
            PlayerPrefs.SetFloat(MUSIC_VOL_KEY, v);

            if (_debugMode)
                Debug.Log($"Set music volume: {v}");
        }
        catch (Exception e)
        {
            Debug.LogError($"Error setting music volume: {e.Message}");
        }
    }

    public void SetSFXVolume(float v)
    {
        try
        {
            float safeVolume = Mathf.Max(0.0001f, v);
            _audioMixer.SetFloat(SFX_PARAM, Mathf.Log10(safeVolume) * 20);
            PlayerPrefs.SetFloat(SFX_VOL_KEY, v);

            if (_debugMode)
                Debug.Log($"Set SFX volume: {v}");
        }
        catch (Exception e)
        {
            Debug.LogError($"Error setting SFX volume: {e.Message}");
        }
    }

    public void PlaySFX(AudioClip clip)
    {
        if (_sfxSource == null)
        {
            Debug.LogError("SFX source is null in SoundManager!");
            return;
        }

        if (clip == null)
        {
            Debug.LogWarning("Attempted to play null audio clip!");
            return;
        }

        _sfxSource.enabled = true;

        if (_enableSFXInMenu || _currentSceneName != "MainMenu")
        {
            _sfxSource.PlayOneShot(clip);

            if (_debugMode)
                Debug.Log($"Playing SFX: {clip.name}");
        }
        else
        {
            if (_debugMode)
                Debug.Log("SFX ignored: disabled in menu scene.");
        }
    }
}
