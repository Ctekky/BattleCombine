using System;

namespace BattleCombine.Data
{
    [Serializable]
    public struct EnemyStatsStruct
    {
        public int health;
        public int attack;
        public int speed;
        public bool shield;
    }
}