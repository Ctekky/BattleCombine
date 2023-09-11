using System;
using System.Collections.Generic;
using System.Linq;
using BattleCombine.ScriptableObjects;
using UnityEngine;
using Random = System.Random;

namespace _Scripts
{
    public class CreateField : MonoBehaviour
    {
        public List<GameObject> GetTileList => _tileList;

        [Header("Scale or not on start")] 
        [SerializeField] private bool makeScale;

        [Header("Start Tile set")] 
        [SerializeField] private bool isAiRandomStart;
        [SerializeField] private int defaultAiStartTilePos;
        [SerializeField] private bool isPlayerRandomStart;
        [SerializeField] private int defaultPlayerStartTilePos;

        [Header("MainField")] [SerializeField] 
        private GameObject field;
        [Header("FieldSize")] [SerializeField] 
        private FieldSize sizeType;

        [Header("TileParent")] [SerializeField]
        private GameObject tileParent;
        [Header("Tile prefab")] [SerializeField]
        private GameObject tile;

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

        private List<GameObject> _tileList;
        private Transform _fieldParent;
        private Random rand;
        private int _fieldSize;

        private void Start()
        {
            _tileList = new List<GameObject>();
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
            ScaleMainField();
        }

        private void AddTileToField()
        {
            var startPlayerTile = SetPlayerStartTileIndex();
            var startAiTile = SetAiStartTileIndex();

            var newTile = new FieldCreateFactory(tile);

            for (var i = 0; i < _fieldSize; i++)
            {
                for (var j = 0; j < _fieldSize; j++)
                {
                    var currentTile = newTile.Create(_fieldParent);
                    currentTile.transform.position = tileParent.transform.position
                                                     + new Vector3(j * tileOffset, i * tileOffset, 0);
                    currentTile.transform.Rotate(90, 180, 0);

                    ChangeTileType(currentTile);

                    _tileList.Add(currentTile);

                    if (i == 0 && j == startPlayerTile)
                        ApplyStartTileStatus(currentTile);
                    if (i == _fieldSize - 1 && j == startAiTile)
                        ApplyStartTileStatus(currentTile);
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

        private void ScaleMainField()
        {
            //test scaler (not complete)
            var mainCamera = Camera.main;
            var halfHeight = mainCamera.orthographic ? mainCamera.orthographicSize : 0;
            var halfWidth = mainCamera.aspect * halfHeight;

            var objectWidth = field.GetComponent<Renderer>().bounds.size.x;
            var objectHeight = field.GetComponent<Renderer>().bounds.size.y;

            var scaleX = (halfWidth * 1.7f - edgeOffset * 1.7f) / objectWidth;
            var scaleZ = (halfHeight * 1f - edgeOffset * 1f) / objectHeight;

            field.transform.localScale = new Vector3(scaleX, 1, scaleZ);
        }

        private void ChangeTileType(GameObject currentTile)
        {
            rand = new();
            var tileComponent = currentTile.GetComponent<Tile>();
            var totalWeight = tileTypeChances.Sum(dictionary => dictionary.Value);

            var roll = rand.Next(0, totalWeight);
            var cumulativeWeight = 0;

            foreach (var dictionary in tileTypeChances)
            {
                cumulativeWeight += dictionary.Value;
                if (roll >= cumulativeWeight) continue;
                tileComponent.ChangeTileType(dictionary.Key);
                return;
            }
        }

        private void ApplyStartTileStatus(GameObject tile)
        {
            var tileComponent = tile.GetComponent<Tile>();
            tileComponent.ChangeStartFlag(true);
        }

        private int SetPlayerStartTileIndex()
        {
            rand = new();
            
            if (defaultPlayerStartTilePos >= _fieldSize)
                defaultPlayerStartTilePos = _fieldSize - 1;

            return isPlayerRandomStart ? rand.Next(0, _fieldSize) : defaultPlayerStartTilePos;
        }

        private int SetAiStartTileIndex()
        {
            rand = new();
            
            if (defaultAiStartTilePos >= _fieldSize)
                defaultAiStartTilePos = _fieldSize - 1;
            
            return isAiRandomStart ? rand.Next(0, _fieldSize) : defaultAiStartTilePos;
        }
    }

    [Serializable]
    public class TileTypeDictionary
    {
        public TileType Key;
        public int Value;
    }
}