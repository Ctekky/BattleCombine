using UnityEngine;

namespace _Scripts
{
    public class EnabledState : State
    {
        private Color color = Color.blue;
        public EnabledState(Tile tile, StateMachine stateMachine) : base(tile, stateMachine)
        {
        }
        public override void Enter()
        {
            base.Enter();
            tile.ChangeClolor(color);
        }
        public override void Input()
        {

        }
        public override void LogicUpdate()
        {

        }
        public override void Exit()
        {
            base.Exit();

        }
    }
}
