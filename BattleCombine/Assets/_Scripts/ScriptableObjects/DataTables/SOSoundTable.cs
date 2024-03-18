using System.Collections.Generic;
using UnityEngine;

namespace BattleCombine.ScriptableObjects
{
    [CreateAssetMenu(fileName = "New SoundTable", menuName = "Sound table")]
    public class SOSoundTable : ScriptableObject
    {
        public List<string> soundEffectsName;
        public List<string> musicThemeName;
    }
}
