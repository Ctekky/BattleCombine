using BattleCombine.Interfaces;
using UnityEngine;

namespace _Scripts
{
    public class AvailableForSelectionState : State
    {
        protected bool tile_status_help;

        private Color color = Color.red;
        public AvailableForSelectionState(Tile tile, StateMachine stateMachine) : base(tile, stateMachine)
        {
        }
        public override void Enter()
        {
            base.Enter();
            tile_status_help = true;
            tile.ChangeClolor(color);
        }
        public override void Input()
        {
            base.Input();
            tile.FindTileForChoosing(tile, tile.TilesForChoosing);
            foreach (GameObject tileGameObject in tile.TilesForChoosing)
            {
                Tile chosingTile = tileGameObject.GetComponent<Tile>();
                if (chosingTile.StateMachine.CurrentState.ToString() == tile.ChosenState.ToString()) //to do сравнивать строки это зло. но как сравнить state нужно разобраться
                {
                    continue;
                }
                else
                {
                    chosingTile.StateMachine.ChangeState(chosingTile.AvailableForSelectionState);
                }
            }

            stateMachine.ChangeState(tile.ChosenState);
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
        }
        public override void Exit()
        {
            base.Exit();
            tile.ClearTheTilesArray();
            tile_status_help = false;
        }
    }
}

