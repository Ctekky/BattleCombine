using UnityEngine;

namespace BattleCombine.Ai
{
    public class Attack : AiBaseEnemy
    {
        public string _name = "Attack";

        private int _count = 0;

        public override void Init()
        {
            base.Init();
            Debug.Log(_name + " bot is arrive!");
        }

        public override void MakeStep()
        {
            CurrentWay[_count].Touch();
            _count++;
        }

        public override void EndAiTurn()
        {
            //end ai turn
            _count = 0;
        }
    }
}