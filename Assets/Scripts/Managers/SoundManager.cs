using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [Header("Audio Mixer")]
    [SerializeField] private AudioMixer _audioMixer;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource _musicSource;
    [SerializeField] private AudioSource _sfxSource;

    public const string MASTER_VOL_KEY = "MasterVolume";
    public const string MUSIC_VOL_KEY = "MusicVolume";
    public const string SFX_VOL_KEY = "SFXVolume";

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
        }
    }


    public void LoadVolumeSettings()
    {
        SetMasterVolume(PlayerPrefs.GetFloat(MASTER_VOL_KEY, 1f));
        SetMusicVolume(PlayerPrefs.GetFloat(MUSIC_VOL_KEY, 1f));
        SetSFXVolume(PlayerPrefs.GetFloat(SFX_VOL_KEY, 1f));
    }

    public void SetMasterVolume(float v)
    {
        _audioMixer.SetFloat("MasterVol", Mathf.Log10(v) * 20);
        PlayerPrefs.SetFloat(MASTER_VOL_KEY, v);
    }

    public void SetMusicVolume(float v)
    {
       _audioMixer.SetFloat("MusciVol", Mathf.Log10(v) * 20);
        PlayerPrefs.SetFloat(MUSIC_VOL_KEY, v);
    }

    public void SetSFXVolume(float v)
    {
        _audioMixer.SetFloat("SFXVol", Mathf.Log10(v) * 20);
        PlayerPrefs.SetFloat(SFX_VOL_KEY, v);
    }

    public void PlaySFX(AudioClip clip)
    {
        _sfxSource.PlayOneShot(clip);
    }
}
