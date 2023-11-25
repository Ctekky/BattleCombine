using System;
using BattleCombine.Enums;

namespace BattleCombine.Data
{
    [Serializable]
    public class TileModifierDictionary
    {
        public TileModifier key;
        public int value;
        public int chance;
    }
}