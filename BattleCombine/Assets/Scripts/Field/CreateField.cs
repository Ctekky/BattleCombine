using System;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts
{
    public class CreateField : MonoBehaviour
    {
        [Header("FieldSize")] 
        [SerializeField] private FieldSize sizeType;
        
        [Header("FieldPrefabs List")] 
        [SerializeField] private List<GameObject> fields;

        [Header("Tile prefab")]
        [SerializeField] private GameObject tile;
        
        private List<GameObject> tileList;
        private GameObject currentField;
        private Transform fieldParent;
        private int fieldSize;
        

        private void Start()
        {
            tileList = new List<GameObject>();
            ChangeFieldSize();
            CreateNewField();
        }

        private void CreateNewField()
        {
            var newField = new FieldCreateFactory(currentField);
       
            var thisField = newField.Create(gameObject.transform);
            fieldParent = thisField.transform;
            
            AddTileToField();
        }

        private void AddTileToField()
        {
            var newTile = new FieldCreateFactory(tile);
            
            for (var i = 0; i < fieldSize; i++)
            {
                var currentTile = newTile.Create(fieldParent);
                tileList.Add(currentTile);
            }
        }

        private void ChangeFieldSize()
        {
            switch (sizeType)
            {
                case FieldSize.Small:
                    fieldSize = 36;
                    currentField = fields[0];
                    break;
                case FieldSize.Medium:
                    fieldSize = 49;
                    currentField = fields[1];
                    break;
                case FieldSize.Large:
                    fieldSize = 64;
                    currentField = fields[2];
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}