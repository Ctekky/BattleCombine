using System;
using BattleCombine.Enums;
using BattleCombine.ScriptableObjects;

namespace BattleCombine.Data
{
    [Serializable]
    public struct TileTypeDBStruct
    {
        public int ID;
        public TileType TileType;
    }

    [Serializable]
    public struct TileSoundDBStruct
    {
        public TileSound ID;
        public string soundEventPath;
    }
    
}