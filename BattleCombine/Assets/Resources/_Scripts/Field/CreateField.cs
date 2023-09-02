using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Scripts
{
    public class CreateField : MonoBehaviour
    {
        [Header("FieldSize")] 
        [SerializeField] private FieldSize sizeType;
        [Header("TileParent")] 
        [SerializeField] private GameObject tileParent;
        [Header("Tile prefab")] 
        [SerializeField] private GameObject tile;
        [FormerlySerializedAs("_offset")]
        [Header("Offsets & scales (test values)")]
        [SerializeField] private float _tileOffset = 1.1f;
        [SerializeField] private float smallFieldScale = 1.45f;
        [SerializeField] private float mediumFieldScale = 1.2f;
        [SerializeField] private float largeFieldScale = 1.04f;

        private List<GameObject> _tileList;
        private Transform _fieldParent;
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
        }
        
        private void AddTileToField()
        {
            var newTile = new FieldCreateFactory(tile);
            var oldTitle = new Vector2(0, 0);

            for (var i = 0; i < _fieldSize; i++)
            {
                for (var j = 0; j < _fieldSize; j++)
                {
                    var currentTile = newTile.Create(_fieldParent);
                    currentTile.transform.position = tileParent.transform.position
                                                     + new Vector3(j * _tileOffset, i * _tileOffset, 0);

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
    }
}