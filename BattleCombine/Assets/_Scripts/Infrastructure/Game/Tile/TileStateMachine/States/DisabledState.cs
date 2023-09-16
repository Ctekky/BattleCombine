using UnityEngine;

namespace BattleCombine.Gameplay
{
    public class DisabledState : State
    {
        protected Color color = Color.black;
        public DisabledState(Tile tile, StateMachine stateMachine) : base(tile, stateMachine)
        {
        }
    }
}
