using System;
using BattleCombine.Enums;
using System.Collections.Generic;
using System.Linq;
using BattleCombine.Data;
using BattleCombine.Interfaces;
using BattleCombine.Services;
using UnityEngine;

namespace BattleCombine.Gameplay
{
    public class TileStack : MonoBehaviour, ISaveLoad
    {
        [SerializeField] private int speedPlayer;
        [SerializeField] private IDPlayer player;
        [SerializeField] private ArcadeGameService arcadeGameService;
        [SerializeField] private List<GameObject> nextMoveTiles;
        [SerializeField] private List<GameObject> tilesForNextMovePlayer1;
        [SerializeField] private List<GameObject> tilesForNextMovePlayer2;
        [SerializeField] private CreateField field;

        [SerializeField] private List<GameObject> tileListPlayer1;
        [SerializeField] private List<GameObject> tileListPlayer2;

        [SerializeField] private List<Tile> startingPlayerTiles;

        public int SpeedPlayer
        {
            get => speedPlayer;
            set => speedPlayer = value;
        }

        public List<GameObject> GetTilesForNextMovePlayer1 => tilesForNextMovePlayer1;
        public List<GameObject> GetTilesForNextMovePlayer2 => tilesForNextMovePlayer2;
        public ArcadeGameService GetArcadeGameService => arcadeGameService;

        public Action<Tile> onTileChoose;

        public List<GameObject> TilesListPlayer1
        {
            get => tileListPlayer1;
            set => tileListPlayer1 = value;
        }

        public List<GameObject> TilesListPlayer2
        {
            get => tileListPlayer2;
            set => tileListPlayer2 = value;
        }

        public void AddTileToStartingList(Tile tile)
        {
            startingPlayerTiles.Add(tile);
        }

        public bool CheckForStartingTile(Tile tile)
        {
            return startingPlayerTiles.Contains(tile);
        }

        public void SetupArcadeGameService(ArcadeGameService aGS)
        {
            this.arcadeGameService = aGS;
        }

        public void ChangeStartingTileState(Tile currentTile, bool status)
        {
            foreach (var tile in startingPlayerTiles.Where(tile => currentTile != tile))
            {
                if (status) tile.StateMachine.ChangeState(tile.EnabledState);
                else tile.StateMachine.ChangeState(tile.AvailableForSelectionState);
            }
        }

        public TileState CheckAnotherTileForState(Tile currentTile)
        {
            foreach (var tile in startingPlayerTiles.Where(tile => currentTile != tile))
            {
                return tile.GetTileState;
            }

            return TileState.DisabledState;
        }

        public List<GameObject> NextMoveTiles
        {
            get => nextMoveTiles;
            set => nextMoveTiles = value;
        }

        public IDPlayer IDPlayer
        {
            get => player;
            private set => player = value;
        }

        private void Start()
        {
            player = IDPlayer.Player1;
        }

        public void SetupPlayer()
        {
            player = IDPlayer.Player1;
        }

        public void DescribeStartingTileList()
        {
            startingPlayerTiles = new List<Tile>();
        }

        private void OnDisable()
        {
            nextMoveTiles.Clear();
            tilesForNextMovePlayer1.Clear();
            tilesForNextMovePlayer2.Clear();
            tileListPlayer1.Clear();
            tileListPlayer2.Clear();
        }

        public void ConfirmTiles()
        {
            switch (IDPlayer)
            {
                case IDPlayer.Player1:
                    ConfirmSelectedTiles(TilesListPlayer1, NextMoveTiles,
                        tilesForNextMovePlayer2);
                    ClearTileLists(TilesListPlayer1, NextMoveTiles,
                        tilesForNextMovePlayer1);
                    SetFlagOnTilePlayer2(tilesForNextMovePlayer2);
                    NextMoveTiles.AddRange(tilesForNextMovePlayer2);
                    break;
                case IDPlayer.Player2:
                    ConfirmSelectedTiles(TilesListPlayer2, NextMoveTiles,
                        tilesForNextMovePlayer1);
                    ClearTileLists(TilesListPlayer2, NextMoveTiles,
                        tilesForNextMovePlayer2);
                    SetFlagOnTilePlayer1(tilesForNextMovePlayer1);
                    NextMoveTiles.AddRange(tilesForNextMovePlayer1);
                    break;
                case IDPlayer.AIPlayer:
                    ConfirmSelectedTiles(TilesListPlayer2, NextMoveTiles,
                        tilesForNextMovePlayer1);
                    ClearTileLists(TilesListPlayer2, NextMoveTiles,
                        tilesForNextMovePlayer2);
                    SetFlagOnTilePlayer1(tilesForNextMovePlayer1);
                    NextMoveTiles.AddRange(tilesForNextMovePlayer1);
                    break;
            }

            PassingTheTurnToTheNextPlayer();
        }

        private void ConfirmSelectedTiles(List<GameObject> listChosenTile, List<GameObject> list,
            List<GameObject> listNextMoveOpponent) //Confirm tile for add to characteristics
        {
            foreach (var tile in listChosenTile.Select(tileGameObjectStack => tileGameObjectStack.GetComponent<Tile>()))
            {
                onTileChoose?.Invoke(tile);
                tile.StateMachine.ChangeState(tile.FinalChoiceState);
            }

            foreach (var tile in list.Select(tileGameObjectList => tileGameObjectList.GetComponent<Tile>()))
            {
                tile.StateMachine.ChangeState(tile.EnabledState);
            }

            foreach (var tile in listNextMoveOpponent.Select(tileGameObjectStack =>
                         tileGameObjectStack.GetComponent<Tile>()))
            {
                if (tile.GetTileState == TileState.DisabledState)
                {
                    continue;
                }
                else
                {
                    tile.StateMachine.ChangeState(tile.AvailableForSelectionState);
                }
            }
        }

        private void ClearTileLists(List<GameObject> listChosenTile, List<GameObject> list,
            List<GameObject> tileNextMove)
        {
            listChosenTile.Clear();
            tileNextMove.Clear();
            tileNextMove.AddRange(list);
            list.Clear();
        }

        private void PassingTheTurnToTheNextPlayer()
        {
            switch (IDPlayer)
            {
                case IDPlayer.Player1:
                    IDPlayer = IDPlayer.Player2;
                    Debug.Log(IDPlayer.ToString() + " moves");
                    break;
                case IDPlayer.Player2:
                    IDPlayer = IDPlayer.Player1;
                    Debug.Log(IDPlayer.ToString() + " moves");
                    break;
                case IDPlayer.AIPlayer:
                    IDPlayer = IDPlayer.Player1;
                    Debug.Log(IDPlayer.ToString() + " moves");
                    break;
                default:
                    Debug.Log("Player is not defined");
                    break;
            }
        }

        public List<GameObject> GetCurrentPlayerTileList()
        {
            var list = new List<GameObject>();
            switch (IDPlayer)
            {
                case IDPlayer.Player1:
                    list = tileListPlayer1;
                    break;
                case IDPlayer.Player2:
                    list = tileListPlayer2;
                    break;
                case IDPlayer.AIPlayer:
                    list = tileListPlayer2;
                    break;
            }

            return list;
        }

        private void SetFlagOnTilePlayer1(List<GameObject> tileList)
        {
            foreach (var tile in tileList.Select(tileGameObject => tileGameObject.GetComponent<Tile>()))
            {
                tile.SetAlignTileToPlayer1(flag: true);
            }
        }

        private void SetFlagOnTilePlayer2(List<GameObject> tileList)
        {
            foreach (var tile in tileList.Select(tileGameObject => tileGameObject.GetComponent<Tile>()))
            {
                tile.SetAlignTileToPlayer2(flag: true);
            }
        }

        public void LoadData(GameData gameData, bool newGameBattle, bool firstStart)
        {
            nextMoveTiles.Clear();
            tilesForNextMovePlayer1.Clear();
            tilesForNextMovePlayer2.Clear();
            startingPlayerTiles.Clear();
            foreach (var tile in field.GetTileList)
            {
                var tileScript = tile.GetComponent<Tile>();
                foreach (var tileID in gameData.NextMoveTileID.Where(tileID => tileID == tileScript.TileID))
                {
                    NextMoveTiles.Add(tile.gameObject);
                }

                foreach (var tileID in gameData.TileForNextMovePlayer1ID.Where(tileID => tileScript.TileID == tileID))
                {
                    tilesForNextMovePlayer1.Add(tile.gameObject);
                }

                foreach (var tileID in gameData.TileForNextMovePlayer2ID.Where(tileID => tileScript.TileID == tileID))
                {
                    tilesForNextMovePlayer2.Add(tile.gameObject);
                }
                foreach (var tileID in gameData.StartTile.Where(tileID => tileScript.TileID == tileID))
                {
                    startingPlayerTiles.Add(tile);
                }
                
            }
            
        }

        public void SaveData(ref GameData gameData, bool newGameBattle, bool firstStart)
        {
            gameData.NextMoveTileID.Clear();
            gameData.TileForNextMovePlayer1ID.Clear();
            gameData.TileForNextMovePlayer2ID.Clear();
            gameData.StartTile.Clear();
            foreach (var tileScript in nextMoveTiles.Select(tile => tile.GetComponent<Tile>()))
            {
                gameData.NextMoveTileID.Add(tileScript.TileID);
            }

            foreach (var tileScript in tilesForNextMovePlayer1.Select(tile => tile.GetComponent<Tile>()))
            {
                gameData.TileForNextMovePlayer1ID.Add(tileScript.TileID);
            }

            foreach (var tileScript in tilesForNextMovePlayer2.Select(tile => tile.GetComponent<Tile>()))
            {
                gameData.TileForNextMovePlayer2ID.Add(tileScript.TileID);
            }

            foreach (var tileScript in startingPlayerTiles.Select(tile => tile.GetComponent<Tile>()))
            {
                gameData.StartTile.Add(tileScript.TileID);
            }
        }
    }
}