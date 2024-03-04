using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.Audio
{
	public class AudioService : MonoBehaviour
	{
		[Header("Папка со звуками")]
		[SerializeField] private string _soundPath = "SFX";

		private readonly Dictionary<string, AudioClip> sounds = new Dictionary<string, AudioClip>();

		private void Awake()
			=> LoadSounds();

		public void PlaySound(string key, AudioSource source)
		{
			if(source == null)
			{
				Debug.LogWarning("AudioSource not found!");
				return;
			}

			if(sounds.TryGetValue(key, out var sound))
			{
				source.PlayOneShot(sound);
			}
			else
			{
				Debug.LogWarning("Sound with key " + key + " not found!");
			}
		}

		private void LoadSounds()
		{
			//source - Resources/Sounds
			var clips = Resources.LoadAll<AudioClip>(_soundPath);

			foreach (var clip in clips)
			{
				sounds.Add(clip.name, clip);
			}
		}

		public void StopSound(AudioSource source)
			=> source.Stop();

		public void PauseSound(AudioSource source)
			=> source.Pause();

		public void UnpauseSound(AudioSource source)
			=> source.Play();
	}
}