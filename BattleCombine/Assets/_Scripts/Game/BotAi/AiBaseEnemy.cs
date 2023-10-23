using System;
using System.Collections.Generic;
using System.Timers;
using BattleCombine.Gameplay;
using UnityEngine;

namespace BattleCombine.Ai
{
    public abstract class AiBaseEnemy
    {
        //Event to link sound *poon'k* to selection
        public static Action OnTileSelect;

        public AiHandler _aiHandler;
        public List<Tile> CurrentWay { get; set; }
        public int MoodHealthPercent { get; set; }
        public int Speed { get; set; }

        public bool IsAiLose;
        public bool IsAiInitialised;
        
        protected static Timer timer;
        protected int wayLength;
        protected int currentStep;
        

        public virtual void Init()
        {
            CurrentWay = new List<Tile>();
            CurrentWay = _aiHandler.CurrentWay;
            MoodHealthPercent = _aiHandler.GetMoodHealthPercent;
            Speed = _aiHandler.AiSpeed;
            wayLength = CurrentWay.Count;

            IsAiInitialised = true;
        }

        public virtual void MakeStep()
        {
            OnTileSelect?.Invoke();
            
            if (CurrentWay[currentStep].StateMachine.CurrentState ==
                CurrentWay[currentStep].EnabledState)
            {
                CurrentWay[currentStep].StateMachine
                    .ChangeState(CurrentWay[currentStep].AvailableForSelectionState);
            }
        }

        public virtual void EndAiTurn()
        {
            
        }
    }
}