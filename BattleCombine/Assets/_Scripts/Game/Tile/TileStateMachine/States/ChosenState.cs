using BattleCombine.Enums;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCombine.Gameplay
{
    public class ChosenState : State
    {
        private readonly Color color = Color.yellow;

        public ChosenState(Tile tile, StateMachine stateMachine) : base(tile, stateMachine)
        {
        }

        public override void Enter()
        {
            _tile.ChangeClolor(color);
            StateName = TileState.ChosenState;
            _tile.SetCurrentState(StateName);
        }

        public override void Input()
        {
            _tile.FindTileForAction(_tile, _tile.TilesForChoosing, TileState.AvailableForSelectionState);

            foreach (GameObject tileGameObject in _tile.TilesForChoosing)
            {
                Tile chosingTile = tileGameObject.GetComponent<Tile>();
                chosingTile.StateMachine.ChangeState(chosingTile.EnabledState);
            }
            Stack<GameObject> tileGameObjectTile = new Stack<GameObject>();

            switch (_tile.GetTileStack.IDPlayer)
            {
                case IDPlayer.Player1:
                    tileGameObjectTile = _tile.GetTileStack.TilesStackPlayer1;
                    break;
                case IDPlayer.Player2:
                    tileGameObjectTile = _tile.GetTileStack.TilesStackPlayer2;
                    break;
            }

            tileGameObjectTile.Pop();

            _stateMachine.ChangeState(_tile.AvailableForSelectionState);

            if(tileGameObjectTile.Count > 0)
            {
                Tile tile = tileGameObjectTile.Peek().GetComponent<Tile>();
                tile.FindTileForAction(tile, tile.TilesForChoosing, TileState.EnabledState);

                foreach (GameObject tileGameObject in tile.TilesForChoosing)
                {
                    Tile chosingTile = tileGameObject.GetComponent<Tile>();
                    chosingTile.StateMachine.ChangeState(chosingTile.AvailableForSelectionState);
                }
                tile.GetTileStack.NextMoveTiles.Clear();
                tile.GetTileStack.NextMoveTiles.AddRange(tile.TilesForChoosing);
            }
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