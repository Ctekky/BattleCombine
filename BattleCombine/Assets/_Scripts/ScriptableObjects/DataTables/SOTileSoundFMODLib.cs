using System.Collections;
using System.Collections.Generic;
using BattleCombine.Data;
using BattleCombine.Enums;
using UnityEngine;

namespace BattleCombine.ScriptableObjects
{
    [CreateAssetMenu(fileName = "New TileSoundDictionary", menuName = "TileSound Dictionary")]
    public class SOTileSoundFMODLib : ScriptableObject
    {
        public List<TileSoundDBStruct> TileSoundDict;
    }
}
