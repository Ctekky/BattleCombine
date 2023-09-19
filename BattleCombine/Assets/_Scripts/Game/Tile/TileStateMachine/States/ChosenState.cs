using BattleCombine.Enums;
using UnityEngine;

namespace BattleCombine.Gameplay
{
    public class ChosenState : State
    {
        private readonly Color _color = Color.yellow;

        public ChosenState(Tile tile, StateMachine stateMachine) : base(tile, stateMachine)
        {
        }

        public override void Enter()
        {
            _tile.ChangeClolor(_color);
            StateName = TileState.ChosenState;
            _tile.SetCurrentState(StateName);
        }

        public override void Input()
        {
            _tile.FindTileForAction(_tile, _tile.TilesForChoosing, TileState.AvailableForSelectionState);

            foreach (GameObject tileGameObject in _tile.TilesForChoosing)
            {
                Tile chosingTile = tileGameObject.GetComponent<Tile>();
                int i = 0;

                foreach (GameObject tileNearChosingTileGameObject in chosingTile.TilesNearThisTile)
                {
                    Tile chosingTileInNearTile = tileNearChosingTileGameObject.GetComponent<Tile>();

                    if (chosingTileInNearTile.gameObject == this._tile.gameObject)
                    {
                        continue;
                    }

                    if (chosingTileInNearTile.StateMachine.CurrentState.ToString() == _tile.ChosenState.ToString())
                    {
                        i = 0;
                        break;
                    }
                    else
                    {
                        i++;
                    }
                }

                if (i >= 1)
                {
                    chosingTile.StateMachine.ChangeState(chosingTile.EnabledState);
                    i = 0;
                }
            }

            _tile.GetTileStack.TilesStack.Pop();
            _stateMachine.ChangeState(_tile.AvailableForSelectionState);
        }

        public override void LogicUpdate()
        {
        }

        public override void Exit()
        {
            _tile.ClearTheTilesArray();
        }
    }
}