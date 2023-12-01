using System;

namespace BattleCombine.Gameplay
{
    [Serializable]
    public struct Stats
    {
        public int attackValue;
        public int attackValueDefault;
        public int attackValueModifier;

        public int healthValue;
        public int healthValueDefault;
        public int healthValueModifier;

        public int moveSpeedValueDefault;
        public int moveSpeedValueModifier;

        public bool shielded;

    }
    
}
