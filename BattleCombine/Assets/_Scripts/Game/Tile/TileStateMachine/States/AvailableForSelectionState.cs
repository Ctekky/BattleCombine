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
            _tile.ChangeClolor(_color);
            StateName = TileState.AvailableForSelectionState;
            _tile.SetCurrentState(StateName);
        }

        public override void Input()
        {
            _tile.FindTileForAction(_tile, _tile.TilesForChoosing, TileState.EnabledState);
            foreach (GameObject tileGameObject in _tile.TilesForChoosing)
            {
                Tile chosingTile = tileGameObject.GetComponent<Tile>();
                chosingTile.StateMachine.ChangeState(chosingTile.AvailableForSelectionState);
            }

            _tile.GetTileStack.TilesStack.Push(_tile.gameObject);

            _tile.GetTileStack.NextMoveTiles.Clear();
            _tile.GetTileStack.NextMoveTiles.AddRange(_tile.TilesForChoosing);

            _stateMachine.ChangeState(_tile.ChosenState);
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
        }

        public override void Exit()
        {
            TileStatusHelp = false;
        }
    }
}