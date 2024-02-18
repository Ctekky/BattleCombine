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
        public static event Action OnTileSelectPlaySfx;

        public AiHandler AiHandler;
        public List<Tile> CurrentWay { get; set; } = new ();
        public int MoodHealthPercent { get; set; }
        public int Speed { get; set; }
        public bool GetStance => _isAiInitialised;

        public bool IsAiLose;
        
        protected int _currentStep;
        private bool _isAiInitialised;

        public virtual void Init()
        {
            MoodHealthPercent = AiHandler.GetMoodHealthPercent;
            Speed = AiHandler.AiSpeed;

            _isAiInitialised = true;
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