
using BattleCombine.Enums;

namespace BattleCombine.Gameplay
{
    public abstract class State
    {
        protected readonly Tile _tile;
        protected readonly StateMachine _stateMachine;
        protected TileState StateName;

        protected State(Tile tile, StateMachine stateMachine)
        {
            this._tile = tile;
            this._stateMachine = stateMachine;
        }
        public virtual void Enter()
        {
        }
        public virtual void Input()
        {
        }
        public virtual void LogicUpdate()
        {

        }
        public virtual void Exit()
        {

        }
    }
}