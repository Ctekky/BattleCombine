using System.Collections.Generic;

namespace BattleCombine.Ai
{
    public abstract class EnemyAi
    {
        public AiHandler _aiHandler;
        public virtual List<int> Weights { get; set; }
        public virtual List<_Scripts.Tile> CurrentWay { get; set; }
        public virtual int MoodHealthPercent { get; set; }
        public virtual int Speed { get; set; }

        public virtual void Init()
        {
            Weights = new();
            Weights = _aiHandler.CurrentWeights;
            CurrentWay = new();
            CurrentWay = _aiHandler.CurrentWay;
            MoodHealthPercent = _aiHandler.GetMoodHealthPercent;
            Speed = _aiHandler.AiSpeed;
        }
    }
}