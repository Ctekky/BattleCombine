using System;

namespace BattleCombine.Gameplay
{
    [Serializable]
    public struct Stats
    {
        public int attackValue;
        public int attackValueDefault;

        public int healthValue;
        public int healthValueDefault;

        public int moveSpeedValueDefault;
        public bool shielded;
    }
    
}
