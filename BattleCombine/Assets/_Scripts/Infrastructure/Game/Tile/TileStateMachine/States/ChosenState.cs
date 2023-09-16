using BattleCombine.Interfaces;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCombine.Gameplay
{
    public class ChosenState : State
    {
        private Color color = Color.yellow;
        public ChosenState(Tile tile, StateMachine stateMachine) : base(tile, stateMachine)
        {

        }
        public override void Enter()
        {
            base.Enter();
            tile.ChangeClolor(color);
        }
        public override void Input()
        {
            base.Input();
            tile.FindTileForChoosing(tile, tile.TilesForChoosing);
            foreach (GameObject tileGameObject in tile.TilesForChoosing)
            {
                Tile chosingTile = tileGameObject.GetComponent<Tile>();
                if (chosingTile.StateMachine.CurrentState.ToString() == tile.AvailableForSelectionState.ToString()) //to do ���������� ������ ��� ���. �� ��� �������� state ����� �����������
                {
                    int i = 0;
                    List<GameObject> listGameObjectNearThisTileWithoutTouchTile = chosingTile.TilesNearThisTile;
                    listGameObjectNearThisTileWithoutTouchTile.Remove(tile.gameObject);

                    foreach (GameObject tileNearChosingTileGameObject in chosingTile.TilesNearThisTile)
                    {
                        Tile chosingTileInNearTile = tileNearChosingTileGameObject.GetComponent<Tile>();

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
                    }
                }
            }

            stateMachine.ChangeState(tile.AvailableForSelectionState);
        }
        public override void LogicUpdate()
        {

        }
        public override void Exit()
        {
            base.Exit();
            tile.ClearTheTilesArray();

        }
    }
}

