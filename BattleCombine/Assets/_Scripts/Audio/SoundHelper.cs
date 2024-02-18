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
        
        //[Header("VolumeSwitchers")] //todo - rewrite (Trial billet)
        //[SerializeField] private Button sfxOn;
        //[SerializeField] private Button sfxOff;
        //[SerializeField] private Button musicOn;
        //[SerializeField] private Button musicOff;
        
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

        private void Start()
        {
            //sfxOn.onClick.AddListener(SwitchSfxButton);
            //sfxOff.onClick.AddListener(SwitchSfxButton);
            //musicOn.onClick.AddListener(SwitchMusicButton);
            //musicOff.onClick.AddListener(SwitchMusicButton);
            _musicSlider.onValueChanged.AddListener(ChangeMusicVolume);
            _sfxSlider.onValueChanged.AddListener(ChangeSfxVolume);
        }

        public void PlayClickSound()
        {
            PlaySound(_clickSound);
        }
        
        //play sound
        private void PlaySound(AudioClip clip)
        {
            _sfxSource.PlayOneShot(clip);
        }

        //private void SwitchSfxButton()
        //{
        //    _isSfxOn = !_isSfxOn;
        //    sfxOn.gameObject.SetActive(!_isSfxOn);
        //    sfxOff.gameObject.SetActive(_isSfxOn);
        //    ChangeSfxVolume(_isSfxOn ? _soundOff : _soundOn);
        //}

        //private void SwitchMusicButton()
        //{
        //    _isMusicOn = !_isMusicOn;
        //    musicOn.gameObject.SetActive(!_isMusicOn);
        //    musicOff.gameObject.SetActive(_isMusicOn);
        //    ChangeMusicVolume(_isMusicOn ? _soundOff : _soundOn);
        //}

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