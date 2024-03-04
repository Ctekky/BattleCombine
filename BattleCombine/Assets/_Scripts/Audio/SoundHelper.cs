using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace _Scripts.Audio
{
    public class SoundHelper : MonoBehaviour
    {
        private const string MusicMixer = "MusicVolume";
        private const string SfxMixer = "SfxVolume";
        private const float SoundOnValue = 1;
        private const float SoundOffValue = 0.0001f;

        [Header("VolumeSliders")] 
        [SerializeField] private Slider _musicSlider;
        [SerializeField] private Slider _sfxSlider;
        [Header("AudioSources")] 
        [SerializeField] private AudioSource _musicSource;
        [SerializeField] private AudioSource _sfxSource;
        [Header("MasterMixer")] 
        [SerializeField] private AudioMixer _mixer;

        [SerializeField] private AudioClip _clickSound;

        private bool isSfxOn;
        private bool isMusicOn;
        private bool isVibrationOn;

        public AudioSource GetSfxSource => _sfxSource;
        public AudioSource GetMusicSource => _musicSource;

        private void Start()
        {
            _musicSlider.onValueChanged.AddListener(ChangeMusicVolume);
            _sfxSlider.onValueChanged.AddListener(ChangeSfxVolume);
        }

        //todo - скрипт можно оставить, т.к. он, по сути, просто регулирует звук.
        //Но плейСаунды выпилить, т.к. они тут уже будут не нужны в будущем ибо проигрываться будет через сервис.
        public void PlayClickSound()
        {
            PlaySound(_clickSound);
        }
        
        //play sound
        private void PlaySound(AudioClip clip)
        {
            _sfxSource.PlayOneShot(clip);
        }

        private void ChangeMusicVolume(float value)
        {
            var dbValue = Mathf.Log10(value) * 20;
            _mixer.SetFloat(MusicMixer, dbValue);
        }

        private void ChangeSfxVolume(float value)
        {
            var dbValue = Mathf.Log10(value) * 20;
            _mixer.SetFloat(SfxMixer, dbValue);
        }
    }
}