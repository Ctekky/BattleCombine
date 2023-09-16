using BattleCombine.Interfaces;
using System;
using UnityEngine;

namespace BattleCombine.Gameplay
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
            tile_status_help = true;
            tile.ChangeClolor(color);
        }
        public override void Input()
        {
            tile.FindTileForAction(tile, tile.TilesForChoosing, "_Scripts.EnabledState");
            foreach (GameObject tileGameObject in tile.TilesForChoosing)
            {
                Tile chosingTile = tileGameObject.GetComponent<Tile>();
                chosingTile.StateMachine.ChangeState(chosingTile.AvailableForSelectionState);
            }

            tile.GetTileStack.TilesStack.Push(tile.gameObject);

            tile.GetTileStack.NextMoveTiles.Clear();
            tile.GetTileStack.NextMoveTiles.AddRange(tile.TilesForChoosing);

            stateMachine.ChangeState(tile.ChosenState);
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
        }
        public override void Exit()
        {
            tile_status_help = false;
        }
    }
}

