using BattleCombine.Enums;
using UnityEngine;

namespace BattleCombine.Gameplay
{
    public class AvailableForSelectionState : State
    {
        protected bool TileStatusHelp;
        private readonly Color _color = Color.red;

        public AvailableForSelectionState(Tile tile, StateMachine stateMachine) : base(tile, stateMachine)
        {
        }

        public override void Enter()
        {
            TileStatusHelp = true;
            _tile.SetTileColor(false);
            _tile.SetBorderColor(true);
            StateName = TileState.AvailableForSelectionState;
            _tile.SetCurrentState(StateName);

            //if (_tile.GetTileStack.GetGameManager.GetCurrentStepInTurn > 1)
            //{
            switch (_tile.GetTileStack.IDPlayer)
            {
                case IDPlayer.Player1:
                    _tile.SetAlignTileToPlayer1(true);
                    break;
                case IDPlayer.Player2:
                    _tile.SetAlignTileToPlayer2(true);
                    break;
            }
            //}
        }

        public override void Input()
        {
            if (_tile.GetTileStack.GetGameManager.GetInputMode == InputMod.Touch)
            {
                InputTouchMod();
            }
            else
            {
                InputMoveMod();
            }
        }

        public override void LogicUpdate()
        {
        }

        public override void Exit()
        {
            TileStatusHelp = false;
        }

        private void InputTouchMod()
        {
            _tile.FindTileForAction(_tile, _tile.TilesForChoosing, TileState.EnabledState);
            foreach (GameObject tileGameObject in _tile.TilesForChoosing)
            {
                Tile chosingTile = tileGameObject.GetComponent<Tile>();
                chosingTile.StateMachine.ChangeState(chosingTile.AvailableForSelectionState);
                chosingTile.SetAlignTileToPlayer1(_tile.IsAlignPlayer1);
                chosingTile.SetAlignTileToPlayer2(_tile.IsAlignPlayer2);
            }

            switch (_tile.GetTileStack.IDPlayer)
            {
                case IDPlayer.Player1: //TODO
                    if (_tile.GetTileStack.TilesListPlayer1.Count > 0 ||
                        _tile.GetTileStack.GetGameManager.GetCurrentStepInTurn > 1)
                    {
                        _tile.ChangeTileStateInStack();
                    }

                    _tile.GetTileStack.TilesListPlayer1.Add(_tile.gameObject);
                    break;
                case IDPlayer.Player2: //TODO
                    if (_tile.GetTileStack.TilesListPlayer2.Count > 0 ||
                        _tile.GetTileStack.GetGameManager.GetCurrentStepInTurn > 1)
                    {
                        _tile.ChangeTileStateInStack();
                    }

                    _tile.GetTileStack.TilesListPlayer2.Add(_tile.gameObject);
                    break;
            }

            _tile.GetTileStack.NextMoveTiles.Clear();
            _tile.GetTileStack.NextMoveTiles.AddRange(_tile.TilesForChoosing);

            _stateMachine.ChangeState(_tile.ChosenState);
        }

        private void InputMoveMod()
        {
            _tile.FindTileForAction(_tile, _tile.TilesForChoosing, TileState.EnabledState);
            foreach (GameObject tileGameObject in _tile.TilesForChoosing)
            {
                Tile chosingTile = tileGameObject.GetComponent<Tile>();
                chosingTile.StateMachine.ChangeState(chosingTile.AvailableForSelectionState);
                chosingTile.SetAlignTileToPlayer1(_tile.IsAlignPlayer1);
                chosingTile.SetAlignTileToPlayer2(_tile.IsAlignPlayer2);
            }

            switch (_tile.GetTileStack.IDPlayer)
            {
                case IDPlayer.Player1: //TODO
                    if (_tile.GetTileStack.TilesListPlayer1.Count > 0 ||
                        _tile.GetTileStack.GetGameManager.GetCurrentStepInTurn > 1)
                    {
                        _tile.ChangeTileStateInStack();
                    }

                    _tile.GetTileStack.TilesListPlayer1.Add(_tile.gameObject);
                    break;
                case IDPlayer.Player2: //TODO
                    if (_tile.GetTileStack.TilesListPlayer2.Count > 0 ||
                        _tile.GetTileStack.GetGameManager.GetCurrentStepInTurn > 1)
                    {
                        _tile.ChangeTileStateInStack();
                    }

                    _tile.GetTileStack.TilesListPlayer2.Add(_tile.gameObject);
                    break;
            }

            _tile.GetTileStack.NextMoveTiles.Clear();
            _tile.GetTileStack.NextMoveTiles.AddRange(_tile.TilesForChoosing);

            _stateMachine.ChangeState(_tile.ChosenState);
        }
    }
}