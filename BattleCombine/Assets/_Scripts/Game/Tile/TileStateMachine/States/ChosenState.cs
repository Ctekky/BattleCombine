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
            Stack<GameObject> stackGameObjectTile = new Stack<GameObject>();

            switch (_tile.GetTileStack.IDPlayer)
            {
                case IDPlayer.Player1:
                    stackGameObjectTile = _tile.GetTileStack.TilesStackPlayer1;
                    break;
                case IDPlayer.Player2:
                    stackGameObjectTile = _tile.GetTileStack.TilesStackPlayer2;
                    break;
            }

            stackGameObjectTile.Pop();

            _stateMachine.ChangeState(_tile.AvailableForSelectionState);

            if(stackGameObjectTile.Count > 0)
            {
                //if (_tile.GetTileStack.GetGameManager.GetCurrentStepInTurn == 1)
                //{
                    Tile tile = stackGameObjectTile.Peek().GetComponent<Tile>();
                    tile.FindTileForAction(tile, tile.TilesForChoosing, TileState.EnabledState);

                    foreach (GameObject tileGameObject in tile.TilesForChoosing)
                    {
                        Tile chosingTile = tileGameObject.GetComponent<Tile>();
                        chosingTile.StateMachine.ChangeState(chosingTile.AvailableForSelectionState);
                    }
                    tile.GetTileStack.NextMoveTiles.Clear();
                    tile.GetTileStack.NextMoveTiles.AddRange(tile.TilesForChoosing);
                //}
            }

            if(_tile.GetTileStack.GetGameManager.GetCurrentStepInTurn > 1 && stackGameObjectTile.Count == 0)
            {
                List<GameObject> listTileGameObject  = new List<GameObject>();
                switch (_tile.GetTileStack.IDPlayer)
                {
                    case IDPlayer.Player1:
                        listTileGameObject = _tile.FindTileDisabledTileForNextMove(_tile.GetTileStack.GetNextTurnButton.GetTilesForNextMovePlayer1);
                        break;
                    case IDPlayer.Player2:
                        listTileGameObject = _tile.FindTileDisabledTileForNextMove(_tile.GetTileStack.GetNextTurnButton.GetTilesForNextMovePlayer2);
                        break;
                }

                foreach (GameObject tileGameObject in listTileGameObject)
                {
                    Tile tile = tileGameObject.GetComponent<Tile>();
                    tile.StateMachine.ChangeState(tile.AvailableForSelectionState);
                }

                _tile.GetTileStack.NextMoveTiles.Clear();
                _tile.GetTileStack.NextMoveTiles.AddRange(listTileGameObject);
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