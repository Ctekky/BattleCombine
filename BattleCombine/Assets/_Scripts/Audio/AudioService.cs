using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.Audio
{
	public class AudioService : MonoBehaviour
	{
		[Header("Папка со звуками")]
		[SerializeField] private string _soundPath = "Audio/Effects";
		[Header("Папка с Музыкой")]
		[SerializeField] private string _musicPath = "Audio/Music";
		
		[Header("Источники Звука")]
		[SerializeField] private AudioSource sfxSource;
		[SerializeField] private AudioSource musicSource;

		private readonly Dictionary<string, AudioClip> sounds = new Dictionary<string, AudioClip>();
		private readonly Dictionary<string, AudioClip> musicThemes = new Dictionary<string, AudioClip>();

		private void OnEnable()
		{
			//todo - link to global scene changeEvent
			//event += LoadAudioSources();
		}

		private void Awake()
		{
			LoadSounds();
			LoadMusic();
		}

		public string GetCurrentMusicThemeName {get; private set;}

		public void PlaySound(string key)
		{
			if(sounds.TryGetValue(key, out var sound))
			{
				sfxSource.PlayOneShot(sound);
				Debug.Log(sound + "played");
			}
			else
			{
				Debug.LogWarning("Sound with key " + key + " not found!");
			}
		}

		public void PlayMusic(string key)
		{
			StopMusic();
			
			if(musicThemes.TryGetValue(key, out var music))
			{
				musicSource.PlayOneShot(music);
				GetCurrentMusicThemeName = key;
			}
			else
			{
				Debug.LogWarning("Music with key " + key + " not found!");
			}
		}

		public (AudioSource, AudioSource) UpdateAudioSources()=>
			(sfxSource, musicSource);

		public void StopSound()
			=> sfxSource.Stop();
		//todo - useful or not?
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

		private void LoadSounds()
		{
			var clips = Resources.LoadAll<AudioClip>(_soundPath);

			foreach (var clip in clips)
			{
				sounds.Add(clip.name, clip);
				Debug.Log(clip + " / " + clip.name );
			}
		}

		private void LoadMusic()
		{
			var clips = Resources.LoadAll<AudioClip>(_musicPath);

			foreach (var clip in clips)
			{
				musicThemes.Add(clip.name, clip);
			}
		}

		private void OnDisable()
		{
			//todo - unlink
			//event -= LoadAudioSources();
		}
	}
}