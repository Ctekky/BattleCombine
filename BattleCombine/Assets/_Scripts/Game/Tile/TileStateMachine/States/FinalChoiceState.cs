using BattleCombine.Enums;
using UnityEngine;

namespace BattleCombine.Gameplay
{
    public class FinalChoiceState : State
    {
        public FinalChoiceState(Tile tile, StateMachine stateMachine) : base(tile, stateMachine)
        {
        }

        public override void Enter()
        {
            StateName = TileState.FinalChoiceState;
            _tile.SetCurrentState(StateName);
        }
    }
}
