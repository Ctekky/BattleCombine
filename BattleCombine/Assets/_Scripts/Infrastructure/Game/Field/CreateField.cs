using System;
using System.Collections.Generic;
using System.Linq;
using BattleCombine.Enums;
using BattleCombine.ScriptableObjects;
using UnityEngine;
using Random = System.Random;

namespace _Scripts
{
    public class CreateField : MonoBehaviour
    {
        [Header("Scale or not on start")] 
        [SerializeField] private bool makeScale;

        [Header("Start Tile set")] 
        [SerializeField] private bool isAiRandomStart;
        [SerializeField] private int defaultAiStartTilePos;
        [SerializeField] private bool isPlayerRandomStart;
        [SerializeField] private int defaultPlayerStartTilePos;

        [Header("FieldSize")] [SerializeField] 
        private FieldSize sizeType;
        [Header("TileParent")] [SerializeField]
        private GameObject tileParent;
        [Header("Tile prefab")] [SerializeField]
        private _Scripts.Tile tile;

        [Header("Offsets & scales (test values)")] [SerializeField, Tooltip("Отступы от края")]
        private float edgeOffset = 0.5f;
        [SerializeField, Tooltip("Отступы между тайлами")]
        private float tileOffset = 1.1f;
        [SerializeField, Tooltip("Скейл в зависимости от размера поля")]
        private float smallFieldScale = 1.45f;
        [SerializeField, Tooltip("Скейл в зависимости от размера поля")]
        private float mediumFieldScale = 1.2f;
        [SerializeField, Tooltip("Скейл в зависимости от размера поля")]
        private float largeFieldScale = 1.04f;

        [Header("TileTypes & Chances - %")] [SerializeField]
        private List<TileTypeDictionary> tileTypeChances;
        
        //todo - remuve static
        public static IEnumerable<_Scripts.Tile> GetTileList => _tileList;
        private static List<_Scripts.Tile> _tileList;
        
        private Transform _fieldParent;
        private GameObject mainField;
        private Random _rand;
        private int _fieldSize;

        private void Start()
        {
            _tileList = new List<_Scripts.Tile>();
            mainField = this.gameObject;
            _fieldParent = tileParent.transform;

            ChangeFieldSize();
        }

        private void ChangeFieldSize()
        {
            _fieldSize = sizeType switch
            {
                FieldSize.Small => 6,
                FieldSize.Medium => 7,
                FieldSize.Large => 8,
                _ => throw new ArgumentOutOfRangeException()
            };

            AddTileToField();
            ModifyTitleSize();
            
            if (!makeScale) return;
            FieldScaler scaler = new();
            scaler.ScaleMainField(this.gameObject, edgeOffset);
        }

        private void AddTileToField()
        {
            var startPlayerTile = SetPlayerStartTileIndex();
            var startAiTile = SetAiStartTileIndex();

            var newTile = new FieldCreateFactory(tile.gameObject);

            for (var i = 0; i < _fieldSize; i++)
            {
                for (var j = 0; j < _fieldSize; j++)
                {
                    var currentTile = newTile.Create(_fieldParent);
                    currentTile.transform.position = tileParent.transform.position
                                                     + new Vector3(j * tileOffset, i * tileOffset, 0);
                    currentTile.transform.Rotate(90, 180, 0);

                    var tileComponent = currentTile.GetComponent<Tile>();
                    ChangeTileType(tileComponent);
                    _tileList.Add(tileComponent);

                    if (i == 0 && j == startPlayerTile)
                        ApplyStartTileStatus(tileComponent);
                    if (i == _fieldSize - 1 && j == startAiTile)
                        ApplyStartTileStatus(tileComponent);
                }
            }
        }

        private void ModifyTitleSize()
        {
            tileParent.transform.localScale = _fieldSize switch
            {
                6 => new Vector3(smallFieldScale, 0, smallFieldScale),
                7 => new Vector3(mediumFieldScale, 0, mediumFieldScale),
                8 => new Vector3(largeFieldScale, 0, largeFieldScale),
                _ => tileParent.transform.localScale
            };
        }

        private void ChangeTileType(_Scripts.Tile currentTile)
        {
            _rand = new();
            var totalWeight = tileTypeChances.Sum(dictionary => dictionary.Value);

            var roll = _rand.Next(0, totalWeight);
            var cumulativeWeight = 0;

            foreach (var dictionary in tileTypeChances)
            {
                cumulativeWeight += dictionary.Value;
                if (roll >= cumulativeWeight) continue;
                currentTile.ChangeTileType(dictionary.Key);
                return;
            }
        }

        private void ApplyStartTileStatus(_Scripts.Tile currentTile)
        {
            currentTile.ChangeStartFlag(true);
        }

        private int SetPlayerStartTileIndex()
        {
            _rand = new();
            
            if (defaultPlayerStartTilePos >= _fieldSize)
                defaultPlayerStartTilePos = _fieldSize - 1;

            return isPlayerRandomStart ? _rand.Next(0, _fieldSize) : defaultPlayerStartTilePos;
        }

        private int SetAiStartTileIndex()
        {
            _rand = new();
            
            if (defaultAiStartTilePos >= _fieldSize)
                defaultAiStartTilePos = _fieldSize - 1;
            
            return isAiRandomStart ? _rand.Next(0, _fieldSize) : defaultAiStartTilePos;
        }
    }

    [Serializable]
    public class TileTypeDictionary
    {
        public TileType Key;
        public int Value;
    }
}