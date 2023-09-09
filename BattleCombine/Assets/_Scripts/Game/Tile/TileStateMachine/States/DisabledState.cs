using UnityEngine;

namespace _Scripts
{
    public class DisabledState : State
    {
        protected Color color = Color.black;
        public DisabledState(Tile tile, StateMachine stateMachine) : base(tile, stateMachine)
        {
        }
    }
}
