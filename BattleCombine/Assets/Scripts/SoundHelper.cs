using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Scripts
{
    public class SoundHelper : MonoBehaviour
    {
        public Action<AudioClip> PlaySoundEvent; //todo - move to events holder
        
        [Header("VolumeSwitchers")] //todo - rewrite (Trial billet)
        [SerializeField] private Button sfxOn;
        [SerializeField] private Button sfxOff;
        [SerializeField] private Button musicOn;
        [SerializeField] private Button musicOff;

        [Header("AudioSources")] 
        [SerializeField] private AudioSource musicSource;
        [SerializeField] private AudioSource sfxSource;
        
        [Header("MasterMixer")] 
        [SerializeField] private AudioMixer mixer;
        
        private readonly string _musicMixer = "MusicVolume";
        private readonly string _sfxMixer = "SfxVolume";
        private readonly float _soundOn = 1;
        private readonly float _soundOff = 0.0001f;
        
        private bool _isSfxOn;
        private bool _isMusicOn;
        private bool _isVibrationOn;

        private void Start()
        {
            PlaySoundEvent += PlaySound;
            sfxOn.onClick.AddListener(SwitchSfxButton);
            sfxOff.onClick.AddListener(SwitchSfxButton);
            musicOn.onClick.AddListener(SwitchMusicButton);
            musicOff.onClick.AddListener(SwitchMusicButton);
        }

        private void SwitchSfxButton()
        {
            _isSfxOn = !_isSfxOn;
            sfxOn.gameObject.SetActive(!_isSfxOn);
            sfxOff.gameObject.SetActive(_isSfxOn);
            ChangeSfxVolume(_isSfxOn ? _soundOff : _soundOn);
        }

        private void SwitchMusicButton()
        {
            _isMusicOn = !_isMusicOn;
            musicOn.gameObject.SetActive(!_isMusicOn);
            musicOff.gameObject.SetActive(_isMusicOn);
            ChangeMusicVolume(_isMusicOn ? _soundOff : _soundOn);
        }

        private void ChangeMusicVolume(float value)
        {
            var dbValue = Mathf.Log10(value) * 20;
            mixer.SetFloat(_musicMixer, dbValue);
        }

        private void ChangeSfxVolume(float value)
        {
            var dbValue = Mathf.Log10(value) * 20;
            mixer.SetFloat(_sfxMixer, dbValue);
        }

        //play sound from everywhere
        private void PlaySound(AudioClip clip)
        {
            sfxSource.PlayOneShot(clip);
        }

        private void OnDisable()
        {
            PlaySoundEvent -= PlaySound;
        }

        private void OnDestroy()
        {
            PlaySoundEvent -= PlaySound;
        }
    }
}