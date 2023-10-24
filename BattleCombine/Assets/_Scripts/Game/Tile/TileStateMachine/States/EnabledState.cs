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
            _tile.SetTileColor(false);
            _tile.SetBorderColor(false);
            StateName = TileState.EnabledState;
            _tile.SetCurrentState(StateName);

            _tile.SetAlignTileToPlayer1(false);
            _tile.SetAlignTileToPlayer2(false);
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
