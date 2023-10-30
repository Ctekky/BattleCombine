using BattleCombine.Enums;
using ModestTree;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
            //_tile.CantUse = true;
            _tile.SetTileColor(true);
            _tile.SetBorderColor(false);
            StateName = TileState.ChosenState;
            _tile.SetCurrentState(StateName);
        }

        public override void Input()
        {
            if (_tile.GetTileStack.GetGameManager.GetInputMode == InputMod.Touch)
            {
                InputTouchMod();
            }
            else
            {
                if (_tile.GetTileStack.GetCurrentPlayerTileList().Count == 1)
                {
                    //return;
                    InputTouchMod();
                }
                else
                {
                    InputMoveMod();
                }
            }
        }

        public override void LogicUpdate()
        {
        }

        public override void Exit()
        {
            _tile.CantUse = false;
            _tile.ClearTheTilesArray();
           
        }

        private void InputTouchMod()
        {
            _tile.FindTileForAction(_tile, _tile.TilesForChoosing, TileState.AvailableForSelectionState);

            foreach (GameObject tileGameObject in _tile.TilesForChoosing)
            {
                Tile chosingTile = tileGameObject.GetComponent<Tile>();
                chosingTile.StateMachine.ChangeState(chosingTile.EnabledState);
            }

            List<GameObject> listGameObjectTile = new List<GameObject>();

            switch (_tile.GetTileStack.IDPlayer)
            {
                case IDPlayer.Player1:
                    listGameObjectTile = _tile.GetTileStack.TilesListPlayer1;
                    break;
                case IDPlayer.Player2:
                    listGameObjectTile = _tile.GetTileStack.TilesListPlayer2;
                    break;
            }

            listGameObjectTile.RemoveAt(listGameObjectTile.Count - 1);
            _stateMachine.ChangeState(_tile.AvailableForSelectionState);

            if (listGameObjectTile.Count > 0)
            {
                Tile tile = listGameObjectTile.Last().GetComponent<Tile>();
                tile.FindTileForAction(tile, tile.TilesForChoosing, TileState.EnabledState);

                foreach (GameObject tileGameObject in tile.TilesForChoosing)
                {
                    Tile chosingTile = tileGameObject.GetComponent<Tile>();
                    chosingTile.StateMachine.ChangeState(chosingTile.AvailableForSelectionState);
                }

                tile.GetTileStack.NextMoveTiles.Clear();
                tile.GetTileStack.NextMoveTiles.AddRange(tile.TilesForChoosing);
            }

            if (_tile.GetTileStack.GetGameManager.GetCurrentStepInTurn > 1 && listGameObjectTile.Count == 0)
            {
                List<GameObject> listTileGameObject = new List<GameObject>();
                switch (_tile.GetTileStack.IDPlayer)
                {
                    case IDPlayer.Player1:
                        listTileGameObject =
                            _tile.FindTileDisabledTileForNextMove(_tile.GetTileStack.GetTilesForNextMovePlayer1);
                        break;
                    case IDPlayer.Player2:
                        listTileGameObject =
                            _tile.FindTileDisabledTileForNextMove(_tile.GetTileStack.GetTilesForNextMovePlayer2);
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

        private void InputMoveMod()
        {
            TileStack tileStack = _tile.GetTileStack;
            List<GameObject> listCurrentPlayer = new List<GameObject>();

            switch (_tile.GetTileStack.IDPlayer)
            {
                case IDPlayer.Player1:
                    listCurrentPlayer = tileStack.TilesListPlayer1;
                    break;
                case IDPlayer.Player2:
                    listCurrentPlayer = tileStack.TilesListPlayer2;
                    break;
            }

            #region Ñhange state (EnabledState) of the tiles for the next move for the last tile in list (NextMoveTiles)

            GameObject lastTileGameObjectInList = listCurrentPlayer.Last();
            List<GameObject> tileListNextMove = new List<GameObject>();

            tileListNextMove.AddRange(tileStack.NextMoveTiles);

            foreach (GameObject tileGameObject in tileListNextMove)
            {
                Tile chagngeTile = tileGameObject.GetComponent<Tile>();
                chagngeTile.StateMachine.ChangeState(chagngeTile.EnabledState);
            }

            listCurrentPlayer.Remove(lastTileGameObjectInList);
            tileListNextMove.Clear();

            #endregion

            #region Ñhange state (AvailableForSelectionState) of the tiles for the next move for the last tile in list (NextMoveTiles)

            tileListNextMove = listCurrentPlayer.Last().GetComponent<Tile>().TilesForChoosing;
            _tile.GetTileStack.NextMoveTiles.Clear();
            _tile.GetTileStack.NextMoveTiles.AddRange(tileListNextMove);

            foreach (GameObject tileGameObject in _tile.GetTileStack.NextMoveTiles)
            {
                Tile chagngeTile = tileGameObject.GetComponent<Tile>();
                chagngeTile.StateMachine.ChangeState(chagngeTile.AvailableForSelectionState);
            }

            #endregion
        }
    }
}