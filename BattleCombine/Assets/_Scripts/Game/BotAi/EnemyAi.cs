using System;
using System.Collections.Generic;
using System.Timers;
using BattleCombine.Gameplay;
using UnityEngine.Analytics;
using Object = UnityEngine.Object;

namespace BattleCombine.Ai
{
    public abstract class EnemyAi
    {
        public AiHandler _aiHandler;
        public List<int> Weights { get; set; }
        public List<Tile> CurrentWay { get; set; }
        public int MoodHealthPercent { get; set; }
        public int Speed { get; set; }

        public bool IsAiTurn;
        public bool IsAiLose;
        
        protected static Timer timer;
        protected int count;
        protected int currentStep;

        public virtual void Init()
        {
            Weights = new();
            Weights = _aiHandler.CurrentWeights;
            CurrentWay = new();
            CurrentWay = _aiHandler.CurrentWay;
            MoodHealthPercent = _aiHandler.GetMoodHealthPercent;
            Speed = _aiHandler.AiSpeed;
            count = CurrentWay.Count;
        }
    }
}