using System;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts
{
    public class CreateFieldOldCanvas : MonoBehaviour
    {
        [Header("FieldSize")] 
        [SerializeField] private FieldSize sizeType;
        
        [Header("FieldPrefabs List")] 
        [SerializeField] private List<GameObject> fields;

        [Header("Tile prefab")]
        [SerializeField] private GameObject tile;
        
        private List<GameObject> _tileList;
        private GameObject _currentField;
        private Transform _fieldParent;
        private int _fieldSize;
        

        private void Start()
        {
            _tileList = new List<GameObject>();
            ChangeFieldSize();
            CreateNewField();
        }

        private void CreateNewField()
        {
            var newField = new FieldCreateFactory(_currentField);
       
            var thisField = newField.Create(gameObject.transform);
            _fieldParent = thisField.transform;
            
            AddTileToField();
        }

        private void AddTileToField()
        {
            var newTile = new FieldCreateFactory(tile);
            
            for (var i = 0; i < _fieldSize; i++)
            {
                var currentTile = newTile.Create(_fieldParent);
                _tileList.Add(currentTile);
            }
        }

        private void ChangeFieldSize()
        {
            switch (sizeType)
            {
                case FieldSize.Small:
                    _fieldSize = 36;
                    _currentField = fields[0];
                    break;
                case FieldSize.Medium:
                    _fieldSize = 49;
                    _currentField = fields[1];
                    break;
                case FieldSize.Large:
                    _fieldSize = 64;
                    _currentField = fields[2];
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}