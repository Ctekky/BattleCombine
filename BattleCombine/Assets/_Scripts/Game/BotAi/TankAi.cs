using UnityEngine;

namespace BattleCombine.Ai
{
    public class TankAi : EnemyAi
    {
        public override void Init()
        {
            base.Init();
            BattleRoar();
        }

        private void BattleRoar()
        {
            Debug.Log("Ama Tanka!");
            Debug.Log(Weights + " Weight applied");
            Debug.Log(CurrentWay + " No Path");
            Debug.Log(MoodHealthPercent + " Health to change mood");
        }
    }
}