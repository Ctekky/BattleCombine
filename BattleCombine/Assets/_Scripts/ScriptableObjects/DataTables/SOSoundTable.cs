using System.Collections.Generic;
using _Scripts.ScriptableObjects.DataTables;
using UnityEngine;
using UnityEngine.Serialization;

namespace BattleCombine.ScriptableObjects
{
	[CreateAssetMenu(fileName = "New SoundTable", menuName = "Sound table")]
	public class SOSoundTable : ScriptableObject
	{
		public List<SoundDictionary> soundEffects;
		public List<SoundDictionary> musicThemes;
	}
}