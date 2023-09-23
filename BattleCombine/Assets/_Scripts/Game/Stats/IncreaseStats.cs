using System;
using BattleCombine.Enums;
using UnityEngine;

namespace BattleCombine.Gameplay
{
    public class IncreaseStats : MonoBehaviour
    {
        private Player _player;
        private StatsCollector _statsCollector;

        public Player Player
        {
            get => _player;
            set => _player = value;
        }

        private void Awake()
        {
            _statsCollector = FindObjectOfType<StatsCollector>();
            _player = FindObjectOfType<Player>(true);
        }

        public void Increase()
        {
            while (_statsCollector.IsHasItem())
            {
                var tile = _statsCollector.Get();
                switch (tile.GetTileType)
                {
                    case CellType.Attack:
                        Debug.Log($"Added to {_player} value of Attack {tile.TileModifier.ToString()}");
                        _player.AddAttack(tile.TileModifier);
                        break;
                    case CellType.Health:
                        Debug.Log($"Added to {_player} value of Health {tile.TileModifier.ToString()}");
                        _player.ChangeHealth(tile.TileModifier);
                        break;
                    case CellType.Shield:
                        _player.AddShield();
                        break;
                    default:
                        break;
                }
            }
        }
    }
}