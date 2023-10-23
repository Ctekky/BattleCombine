using UnityEngine;

namespace BattleCombine.Ai
{
    public class Tank : AiBaseEnemy
    {
        public string _name = "Tank";

        private int _count = 0;
        
        public override void Init()
        {
            base.Init();
            Debug.Log(_name + " bot is arrive!");
        }

        public override void MakeStep()
        {
            base.MakeStep();
            
            CurrentWay[_count].FingerMoved();//.Touch();
            _count++;
        }

        public override void EndAiTurn()
        {
            //end ai turn
            _count = 0;
        }
    }
}