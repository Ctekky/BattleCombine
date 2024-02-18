using System;
using System.Collections.Generic;
using BattleCombine.Enums;

namespace BattleCombine.Data
{
    [Serializable]
    public class GameData
    {
        public PlayerAccountData PlayerAccountData = new PlayerAccountData();
        public List<BattleStatsData> BattleStatsData = new List<BattleStatsData>();
        public FieldSize FieldSize;
        public List<TileData> FieldData = new List<TileData>();
        public List<int> NextMoveTileID = new List<int>();
        public List<int> TileForNextMovePlayer1ID = new List<int>();
        public List<int> TileForNextMovePlayer2ID = new List<int>();
        public List<int> StartTile = new List<int>();
        public int CurrentStep;
        public bool IsBattleActive;
        public bool IsEnemySelectionScene;
        public int EnemyAvatarID;
        public int ArcadePlayerLevel;
        public bool IsEndScreen;
        public string LoserName;
    }
}
