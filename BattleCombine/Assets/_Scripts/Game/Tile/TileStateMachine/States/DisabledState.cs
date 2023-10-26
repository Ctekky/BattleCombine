using BattleCombine.Enums;
using UnityEngine;

namespace BattleCombine.Gameplay
{
    public class DisabledState : State
    {
        protected Color color = Color.black;
        public DisabledState(Tile tile, StateMachine stateMachine) : base(tile, stateMachine)
        {
        }

        public override void Enter()
        {
            _tile.SetTileColor(false);
            _tile.SetBorderColor(false);
            StateName = TileState.DisabledState;
            _tile.SetCurrentState(StateName);
        }
    }
}
