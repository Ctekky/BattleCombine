using System;
using BattleCombine.Enums;

namespace BattleCombine.Gameplay
{
    [Serializable]
    public class TileModifierDictionary
    {
        public TileModifier Key;
        public int Value;
        public int Chance;
    }
}