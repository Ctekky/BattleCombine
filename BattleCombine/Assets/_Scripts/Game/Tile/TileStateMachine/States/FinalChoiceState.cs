using BattleCombine.Enums;
using UnityEngine;

namespace BattleCombine.Gameplay
{
    public class FinalChoiceState : State
    {
        private readonly Color color = Color.cyan;
        public FinalChoiceState(Tile tile, StateMachine stateMachine) : base(tile, stateMachine)
        {
        }

        public override void Enter()
        {
            _tile.SetTileColor(true);
            _tile.SetBorderColor(false);
            StateName = TileState.FinalChoiceState;
            _tile.SetCurrentState(StateName);

            _stateMachine.ChangeState(_tile.DisabledState);
        }

        public override void Input()
        {
            
        }
        public override void LogicUpdate()
        {
            
        }

        public override void Exit() 
        {
            
        }
    }
}
