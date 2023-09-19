using BattleCombine.Enums;
using UnityEngine;

namespace BattleCombine.Gameplay
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
            _tile.ChangeClolor(color);
            StateName = TileState.EnabledState;
            _tile.SetCurrentState(StateName);
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
