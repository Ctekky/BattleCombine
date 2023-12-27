using System.Collections.Generic;
using BattleCombine.Data;
using UnityEngine;

namespace BattleCombine.ScriptableObjects
{
    [CreateAssetMenu(fileName = "New tile type table", menuName = "Tile type table")]
    public class SOTileTypeTable : ScriptableObject
    {
        public List<TileTypeDBStruct> tileTypeDB;
        
    }
}