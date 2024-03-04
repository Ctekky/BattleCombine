using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.Audio
{
	public class AudioService : MonoBehaviour
	{
		[Header("Папка со звуками")]
		[SerializeField] private string _soundPath = "SFX";
		[Header("Папка с Музыкой")]
		[SerializeField] private string _musicPath = "Music";

		private readonly Dictionary<string, AudioClip> sounds = new Dictionary<string, AudioClip>();
		private readonly Dictionary<string, AudioClip> musicThemes = new Dictionary<string, AudioClip>();

		private AudioSource sfxSource;
		private AudioSource musicSource;

		private void OnEnable()
		{
			//todo - подвязаться к глобальному ивенту смены сцены
			//ивент += LoadAudioSources();
		}

		private void Awake()
		{
			UpdateAudioSources();
			LoadSounds();
			LoadMusic();
		}

		public void PlaySound(string key)
		{
			if(sounds.TryGetValue(key, out var sound))
			{
				sfxSource.PlayOneShot(sound);
			}
			else
			{
				Debug.LogWarning("Sound with key " + key + " not found!");
			}
		}

		public void PlayMusic(string key)
		{
			if(musicThemes.TryGetValue(key, out var music))
			{
				musicSource.PlayOneShot(music);
			}
			else
			{
				Debug.LogWarning("Music with key " + key + " not found!");
			}
		}

		private void UpdateAudioSources()
		{
			var soundHelper = FindObjectOfType<SoundHelper>();
			
			sfxSource = soundHelper.GetSfxSource;
			musicSource = soundHelper.GetMusicSource;
		}

		private void LoadSounds()
		{
			var clips = Resources.LoadAll<AudioClip>(_soundPath);

			foreach (var clip in clips)
			{
				sounds.Add(clip.name, clip);
			}
		}

		private void LoadMusic()
		{
			var clips = Resources.LoadAll<AudioClip>(_soundPath);

			foreach (var clip in clips)
			{
				musicThemes.Add(clip.name, clip);
			}
		}

		public void StopSound()
			=> sfxSource.Stop();
		public void PauseSound()
			=> sfxSource.Pause();
		public void UnpauseSound()
			=> sfxSource.Play();		
		
		public void StopMusic()
			=> musicSource.Stop();
		public void PauseMusic()
			=> musicSource.Pause();
		public void UnpauseMusic()
			=> musicSource.Play();

		private void OnDisable()
		{
			//todo - отвязаться
			//ивент -= LoadAudioSources();
		}
	}
}