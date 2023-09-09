using Unity.VisualScripting.Antlr3.Runtime;

namespace _Scripts
{
    public abstract class State
    {
        protected Tile tile;
        protected StateMachine stateMachine;

        protected State(Tile tile, StateMachine stateMachine)
        {
            this.tile = tile;
            this.stateMachine = stateMachine;
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