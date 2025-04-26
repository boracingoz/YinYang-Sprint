using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Managers;

namespace Assets.Scripts.UI
{
    public class SettingsPanel : MonoBehaviour
    {
        [SerializeField] private Slider _masterSlider;
        [SerializeField] private Slider _musicSlider;
        [SerializeField] private Slider _sfxSlider;

        private void Start()
        {
            _masterSlider.value = PlayerPrefs.GetFloat(SoundManager.MASTER_VOL_KEY, 1f);
            _musicSlider.value = PlayerPrefs.GetFloat(SoundManager.MUSIC_VOL_KEY, 1F);
            _sfxSlider.value = PlayerPrefs.GetFloat(SoundManager.SFX_VOL_KEY, 1F);

            _masterSlider.onValueChanged.AddListener(SoundManager.instance.SetMasterVolume);
            _musicSlider.onValueChanged.AddListener(SoundManager.instance.SetMusicVolume);
            _sfxSlider.onValueChanged.AddListener(SoundManager.instance.SetSFXVolume);

        }

        private void OnEnable()
        {
            _masterSlider.value = PlayerPrefs.GetFloat(SoundManager.MASTER_VOL_KEY, 1f);
            _musicSlider.value = PlayerPrefs.GetFloat(SoundManager.MUSIC_VOL_KEY, 1f);
            _sfxSlider.value = PlayerPrefs.GetFloat(SoundManager.SFX_VOL_KEY, 1f);

            if (SoundManager.instance != null)
            {
                _masterSlider.onValueChanged.AddListener(SoundManager.instance.SetMasterVolume);
                _musicSlider.onValueChanged.AddListener(SoundManager.instance.SetMusicVolume);
                _sfxSlider.onValueChanged.AddListener(SoundManager.instance.SetSFXVolume);
            }
        }

        public void CloseSettings()
        {
            gameObject.SetActive(false);
        }
    }
}
