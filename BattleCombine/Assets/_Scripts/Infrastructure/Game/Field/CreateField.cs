using System;
using System.Collections.Generic;
using BattleCombine.ScriptableObjects;
using UnityEngine;
using Random = System.Random;

namespace _Scripts
{
    public class CreateField : MonoBehaviour
    {
        [Header("Scale or not on start")] [SerializeField]
        private bool makeScale;

        [Header("MainField")] [SerializeField] private GameObject field;
        [Header("FieldSize")] [SerializeField] private FieldSize sizeType;

        [Header("TileParent")] [SerializeField]
        private GameObject tileParent;

        [Header("Tile prefab")] [SerializeField]
        private GameObject tile;

        [Header("Tiles Types")] [SerializeField]
        private List<TileType> tileTypes;

        [Header("Offsets & scales (test values)")] [SerializeField]
        private float edgeOffset = 0.5f;

        [SerializeField] private float tileOffset = 1.1f;
        [SerializeField] private float smallFieldScale = 1.45f;
        [SerializeField] private float mediumFieldScale = 1.2f;
        [SerializeField] private float largeFieldScale = 1.04f;
        
        [Header("TileType Chances - %")]
        [SerializeField] private int attackTileChance;
        [SerializeField] private int defenceTileChance;
        [SerializeField] private int healthTileChance;

        private List<GameObject> _tileList;
        private Transform _fieldParent;
        private Random rand;
        private int _fieldSize;

        public List<GameObject> GetTileList => _tileList;

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
            var tempSort = new List<int>(){attackTileChance, defenceTileChance, healthTileChance};
            tempSort.Sort();
            
            var tileComponent = currentTile.GetComponent<Tile>();
            var tileRoll = rand.Next(1, 101);

            if (tileRoll <= tempSort[0])
            {
                tileComponent.ChangeTileType(tileTypes[2]);
            }
            else
            {
                tileComponent.ChangeTileType(tileRoll <= tempSort[1] ? tileTypes[1] : tileTypes[0]);
            }
        }
    }
}