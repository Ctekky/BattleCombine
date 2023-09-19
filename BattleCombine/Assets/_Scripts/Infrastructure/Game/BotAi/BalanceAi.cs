using UnityEngine;

namespace BattleCombine.Ai
{
    public class BalanceAi : EnemyAi
    {
        public override void Init()
        {
            base.Init();
            BattleRoar();
        }

        private void BattleRoar()
        {
            Debug.Log("Ama Balanzza!");
            Debug.Log(Weights + " Weight applied");
            Debug.Log(CurrentWay.Count + " Path");
            Debug.Log(MoodHealthPercent + " Health to change mood");
        }
    }
}