using System;
using System.Collections.Generic;
using BattleCombine.Enums;

namespace BattleCombine.Data
{
    [Serializable]
    public class LevelDesignFieldData
    {
        public FieldSize FieldSize;
        public List<TileData> FieldData = new List<TileData>();
        public List<int> StartTile = new List<int>();
    }
}
