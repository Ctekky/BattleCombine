using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using Zenject;

namespace _Scripts.Audio
{
	public class SoundHelper : MonoBehaviour
	{
		private const string MusicMixer = "MusicVolume";
		private const string SfxMixer = "SfxVolume";
		private const float SoundOnValue = 1;
		private const float SoundOffValue = 0.0001f;

		[SerializeField] private bool _startNewBackgroundMusic = false;
		[SerializeField] private string _musicName = "m_menu";
		[Header("VolumeSliders")]
		[SerializeField] private Slider _musicSlider;
		[SerializeField] private Slider _sfxSlider;
		[Header("AudioSources")]
		[SerializeField] private AudioSource _musicSource;
		[SerializeField] private AudioSource _sfxSource;
		[Header("MasterMixer")]
		[SerializeField] private AudioMixer _mixer;

		private bool isSfxOn;
		private bool isMusicOn;
		private bool isVibrationOn;

		[Inject]
		private AudioService audioSource;

		public AudioSource GetSfxSource => _sfxSource;
		public AudioSource GetMusicSource => _musicSource;

		private void Start()
		{
			VolumeSliderSubscribe();

			(_sfxSource, _musicSource) = audioSource.UpdateAudioSources();

			if(_startNewBackgroundMusic && CheckSceneAndMusic()==false)
				StartBGM();
		}

		private void VolumeSliderSubscribe()
		{
			if(_musicSlider != null)
				_musicSlider.onValueChanged.AddListener(ChangeMusicVolume);

			if(_sfxSlider != null)
				_sfxSlider.onValueChanged.AddListener(ChangeSfxVolume);
		}

		private void StartBGM()
		{
			audioSource.PlayMusic(_musicName);
		}

		private bool CheckSceneAndMusic() 
			=> audioSource.GetCurrentMusicThemeName == _musicName;
	
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