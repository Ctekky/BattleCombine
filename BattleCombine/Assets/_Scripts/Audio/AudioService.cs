using System.Collections.Generic;
using BattleCombine.ScriptableObjects;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Scripts.Audio
{
	public class AudioService : MonoBehaviour
	{
		[Header("Папка со звуками")]
		[SerializeField] private string _soundPath = "Audio/Effects";
		[Header("Папка с Музыкой")]
		[SerializeField] private string _musicPath = "Audio/Music";
		
		[FormerlySerializedAs("sfxSource")]
		[Header("Источники Звука")]
		[SerializeField] private AudioSource _sfxSource;
		[FormerlySerializedAs("musicSource")]
		[SerializeField] private AudioSource _musicSource;

		[Header("База Звуков")]
		[SerializeField] private SOSoundTable _audioData;

		private readonly Dictionary<string, AudioClip> sounds = new Dictionary<string, AudioClip>();
		private readonly Dictionary<string, AudioClip> musicThemes = new Dictionary<string, AudioClip>();

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
				_sfxSource.PlayOneShot(sound);
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
				_musicSource.PlayOneShot(music);
				GetCurrentMusicThemeName = key;
			}
			else
			{
				Debug.LogWarning("Music with key " + key + " not found!");
			}
		}

		public (AudioSource, AudioSource) UpdateAudioSources()=>
			(_sfxSource, _musicSource);

		public void StopSound()
			=> _sfxSource.Stop();
		//todo - useful or not?
		public void PauseSound()
			=> _sfxSource.Pause();
		public void UnpauseSound()
			=> _sfxSource.Play();
		
		public void StopMusic()
			=> _musicSource.Stop();
		public void PauseMusic()
			=> _musicSource.Pause();
		public void UnpauseMusic()
			=> _musicSource.Play();

		private void LoadSounds()
		{
			//var clips = Resources.LoadAll<AudioClip>(_soundPath);
			//foreach (var clip in clips)
			//{
			//	sounds.Add(clip.name, clip);
			//	Debug.Log(clip + " / " + clip.name );
			//}
			
			foreach (var clip in _audioData.soundEffects)
			{
				sounds.Add(clip.Key, clip.Clip);
			}
		}

		private void LoadMusic()
		{
			//var clips = Resources.LoadAll<AudioClip>(_musicPath);
			//foreach (var clip in clips)
			//{
			//	musicThemes.Add(clip.name, clip);
			//}
			
			foreach (var clip in _audioData.musicThemes)
			{
				musicThemes.Add(clip.Key, clip.Clip);
			}
		}
	}
}