using System.Timers;
using BattleCombine.Gameplay;
using UnityEngine;

namespace BattleCombine.Ai
{
    public class AttackAi : EnemyAi
    {
        public override void Init()
        {
            base.Init();
            BattleRoar();
        }
        
        private void BattleRoar()
        {
            Debug.Log("Ama Attakka!");
            Debug.Log(Weights + " Weight applied");
            Debug.Log(CurrentWay.Count + " Path");
            Debug.Log(MoodHealthPercent + " Health to change mood");
            
            foreach (var tile in CurrentWay)
            {
                tile.StateMachine.ChangeState(tile.ChosenState);
                //tile.Touch();
            }
        }
    }
}