using BattleCombine.Interfaces;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts
{
    public class ChosenState : State
    {
        private Color color = Color.yellow;
        public ChosenState(Tile tile, StateMachine stateMachine) : base(tile, stateMachine)
        {

        }
        public override void Enter()
        {
            tile.ChangeClolor(color);
        }
        public override void Input()
        {
            tile.FindTileForAction(tile, tile.TilesForChoosing, "_Scripts.AvailableForSelectionState");

            foreach (GameObject tileGameObject in tile.TilesForChoosing)
            {
                Tile chosingTile = tileGameObject.GetComponent<Tile>();
                int i = 0;

                foreach (GameObject tileNearChosingTileGameObject in chosingTile.TilesNearThisTile)
                {
                    Tile chosingTileInNearTile = tileNearChosingTileGameObject.GetComponent<Tile>();

                    if (chosingTileInNearTile.gameObject == this.tile.gameObject)
                    {
                        continue;
                    }

                    if (chosingTileInNearTile.StateMachine.CurrentState.ToString() == tile.ChosenState.ToString())
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

            tile.GetTileStack.TilesStack.Pop();

            stateMachine.ChangeState(tile.AvailableForSelectionState);
        }
        public override void LogicUpdate()
        {

        }
        public override void Exit()
        {
            tile.ClearTheTilesArray();
        }
    }
}

