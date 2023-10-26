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
        public static Action OnTileSelectPlaySfx;

        public AiHandler AiHandler;
        public List<Tile> CurrentWay { get; set; }
        public int MoodHealthPercent { get; set; }
        public int Speed { get; set; }

        public bool IsAiLose;
        public bool IsAiInitialised;
        
        protected int _currentStep;
        

        public virtual void Init()
        {
            CurrentWay = new List<Tile>();
            CurrentWay = AiHandler.CurrentWay;
            MoodHealthPercent = AiHandler.GetMoodHealthPercent;
            Speed = AiHandler.AiSpeed;

            IsAiInitialised = true;
        }

        public virtual void MakeStep()
        {
            OnTileSelectPlaySfx?.Invoke();
            
            if (CurrentWay[_currentStep].StateMachine.CurrentState ==
                CurrentWay[_currentStep].EnabledState)
            {
                CurrentWay[_currentStep].StateMachine
                    .ChangeState(CurrentWay[_currentStep].AvailableForSelectionState);
            }
        }

        public virtual void EndAiTurn()
        {
            
        }
    }
}