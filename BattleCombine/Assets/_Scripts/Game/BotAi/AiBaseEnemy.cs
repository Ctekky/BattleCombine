using System.Collections.Generic;
using System.Timers;
using BattleCombine.Gameplay;

namespace BattleCombine.Ai
{
    public abstract class AiBaseEnemy
    {
        public AiHandler _aiHandler;
        public List<int> Weights { get; set; }
        public List<Tile> CurrentWay { get; set; }
        public int MoodHealthPercent { get; set; }
        public int Speed { get; set; }

        public bool IsAiTurn;
        public bool IsAiLose;
        public bool IsAiInitialised;
        
        protected static Timer timer;
        protected int wayLength;
        protected int currentStep;
        

        public virtual void Init()
        {
            Weights = new();
            Weights = _aiHandler.CurrentWeights;
            CurrentWay = new();
            CurrentWay = _aiHandler.CurrentWay;
            MoodHealthPercent = _aiHandler.GetMoodHealthPercent;
            Speed = _aiHandler.AiSpeed;
            wayLength = CurrentWay.Count;

            IsAiInitialised = true;
        }

        public virtual void MakeStep()
        {

        }

        public virtual void EndAiTurn()
        {
            
        }
    }
}