using BattleCombine.Enums;
using UnityEngine;

namespace BattleCombine.Gameplay
{
    public class IncreaseStats : MonoBehaviour
    {
        private Player _player;
        private StatsCollector _statsCollector;

        private void Awake()
        {
            _statsCollector = FindObjectOfType<StatsCollector>();
            _player = FindObjectOfType<Player>(true);
        }

        public void Increase()
        {
            while (_statsCollector.IsHasItem() && _player.moveSpeedValue == 0)
            {
                var tile = _statsCollector.Get();
                if (CellType.Attack == tile.GetTileType)
                {
                    //_player.AddAttack(tile.Tile_value);
                }

                else if (CellType.Health == tile.GetTileType)
                {
                    //_player.ChangeHealth(tile.Tile_value);
                }

                else if (CellType.Shield == tile.GetTileType)
                {
                    //_player.AddShield();
                }
            }

            if (_player.moveSpeedValue == 0) _player.NextMove();
        }
    }
}