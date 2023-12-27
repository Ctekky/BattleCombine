using System;
using System.Collections.Generic;
using System.Linq;
using BattleCombine.Data;
using BattleCombine.Enums;
using BattleCombine.Interfaces;
using BattleCombine.ScriptableObjects;
using BattleCombine.Services;
using BattleCombine.Services.Other;
using UnityEngine;
using Random = System.Random;

namespace BattleCombine.Gameplay
{
    public class CreateField : MonoBehaviour, ISaveLoad
    {
        [SerializeField] private SOTileTypeTable tileTypeTable;

        [Header("Scale or not on start")] [SerializeField]
        private bool makeScale;

        [Header("Start Tile set")] [SerializeField]
        private bool isAiRandomStart;

        [SerializeField] private int defaultAiStartTilePos;
        [SerializeField] private bool isPlayerRandomStart;

        private List<int> defaultPlayerStartTilePos;

        [SerializeField] private TileStack tileStack;

        [Header("FieldSize")] [SerializeField] private FieldSize sizeType;

        [Header("TileParent")] [SerializeField]
        private GameObject tileParent;

        [Header("Tile prefab")] [SerializeField]
        private Tile tile;

        [Header("Offsets & scales (test values)")] [SerializeField, Tooltip("Отступы от края")]
        private float edgeOffset = 0.5f;

        [SerializeField, Tooltip("Отступы между тайлами")]
        private float tileOffset = 1.1f;

        [SerializeField, Tooltip("Скейл в зависимости от размера поля")]
        private float tutorFieldScale = 1.8f;

        [SerializeField, Tooltip("Скейл в зависимости от размера поля")]
        private float smallFieldScale = 1.45f;

        [SerializeField, Tooltip("Скейл в зависимости от размера поля")]
        private float mediumFieldScale = 1.2f;

        [SerializeField, Tooltip("Скейл в зависимости от размера поля")]
        private float largeFieldScale = 1.04f;

        [Header("TileTypes & Chances - %")] [SerializeField]
        private List<TileTypeDictionary> tileTypeChances;

        [Header("Tile Refresh chance - %")] [SerializeField]
        private int tileRefreshChance;

        private ColorSettings _currentTileColorSettings;

        private ArcadeGameService _arcadeGameService;
        private GameManager _gameManager;
        [SerializeField] private List<Tile> _tileList;
        private Transform _fieldParent;
        private GameObject _mainField;
        private Random _rand;
        private int _fieldSize;
        private bool _isTileFullSetup;

        public Tile GetAiStartTile { get; private set; }
        public int GetAiStartTileIndex { get; private set; }
        public List<Tile> GetTileList => _tileList;
        public int GetFieldSize => _fieldSize;

        #region Events

        public Action<Tile> onTileTouched;
        public Action<bool> onSpeedEnded;

        #endregion

        private void Awake()
        {
            _tileList = new List<Tile>();
            _mainField = this.gameObject;
            _fieldParent = tileParent.transform;
            _isTileFullSetup = false;
            tileStack.DescribeStartingTileList();
        }

        public void SetupField(bool changeFieldSize, FieldSize fieldSize, ColorSettings tileColorSettings)
        {
            _currentTileColorSettings = tileColorSettings;
            if (changeFieldSize) sizeType = fieldSize;
            ChangeFieldSize();
        }


        private void Update()
        {
            if (!_isTileFullSetup) SetupTileOnField();
        }

        private void ChangeFieldSize()
        {
            _fieldSize = sizeType switch
            {
                FieldSize.UltraSmall => 5,
                FieldSize.Small => 6,
                FieldSize.Medium => 7,
                FieldSize.Large => 8,
                _ => throw new ArgumentOutOfRangeException()
            };

            defaultPlayerStartTilePos = new List<int>()
            {
                0, _fieldSize
            };

            AddTileToField();

            ModifyTitleSize();
            SetupTileOnField();

            if (!makeScale) return;
            var scaler = new FieldScaler();
            scaler.ScaleMainField(this.gameObject, edgeOffset);
        }

        private void SetupTileOnField()
        {
            int i = 0;
            foreach (var tileInList in _tileList)
            {
                tileInList.CheckTilesStateNearThisTile(tileInList);
                tileInList.TileID = i;
                i++;
            }

            _isTileFullSetup = true;
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
                    tileComponent.SetupTile(tileStack, _currentTileColorSettings);
                    _tileList.Add(tileComponent);

                    tileComponent.onTileTouched += touchedTile => onTileTouched?.Invoke(touchedTile);
                    tileComponent.onSpeedEnded += isSpeedEnded => onSpeedEnded?.Invoke(isSpeedEnded);

                    if (i != _fieldSize - 1 || j != startAiTile) continue;

                    //ApplyStartTileStatus(tileComponent);
                    GetAiStartTile = tileComponent;
                    FindStartTileIndex(tileComponent);

                    tileComponent.SetAlignTileToPlayer2(true);
                }
            }

            var index = -1;
            foreach (var tile in _tileList)
            {
                index++;
                if (index != startPlayerTile[0] && index != startPlayerTile[1])
                    continue;

                ApplyStartTileStatus(tile);
                tileStack.AddTileToStartingList(tile);
                tile.SetupTile(tileStack, _currentTileColorSettings);
                tile.SetAlignTileToPlayer2(true);
            }
        }

        private void ModifyTitleSize()
        {
            tileParent.transform.localScale = _fieldSize switch
            {
                5 => new Vector3(tutorFieldScale, 0, tutorFieldScale),
                6 => new Vector3(smallFieldScale, 0, smallFieldScale),
                7 => new Vector3(mediumFieldScale, 0, mediumFieldScale),
                8 => new Vector3(largeFieldScale, 0, largeFieldScale),
                _ => tileParent.transform.localScale
            };
        }

        private void RefreshEmptyTile(Tile currentTile)
        {
            _rand = new Random();
            var roll = _rand.Next(0, 100);
            if (roll >= tileRefreshChance) return;
            ChangeTileType(currentTile);
            currentTile.StateMachine.ChangeState(currentTile.EnabledState);
        }

        public void RefreshField()
        {
            foreach (var currentTile in _tileList)
            {
                if (currentTile.GetTileState == TileState.DisabledState)
                {
                    RefreshEmptyTile(currentTile);
                }
                else if ((currentTile.GetTileType != CellType.Shield && currentTile.TileModifier < 9))
                {
                    if (currentTile.TileModifier == -1)
                        currentTile.ChangeTileModifier(currentTile.TileModifier + 2);

                    else
                        currentTile.ChangeTileModifier(currentTile.TileModifier + 1);
                }
            }
        }

        private void ApplyStartTileStatus(Tile currentTile)
        {
            //todo - change flaging, to AiLink
            currentTile.ChangeStartFlag(true);
        }

        private List<int> SetPlayerStartTileIndex()
        {
            _rand = new Random();

            //return isPlayerRandomStart ? _rand.Next(0, _fieldSize) : defaultPlayerStartTilePos;

            if (!isPlayerRandomStart)
                return defaultPlayerStartTilePos;

            defaultPlayerStartTilePos[0] = _rand.Next(0, _fieldSize / 2);
            defaultPlayerStartTilePos[1] = _rand.Next(defaultPlayerStartTilePos[0] + 2, _fieldSize);
            return defaultPlayerStartTilePos;
        }

        public void SetupArcadeGameService(ArcadeGameService arcadeGameService)
        {
            _arcadeGameService = arcadeGameService;
        }

        public void SetupGameManager(GameManager gameManager)
        {
            _gameManager = gameManager;
        }

        private int SetAiStartTileIndex()
        {
            _rand = new Random();

            if (defaultAiStartTilePos >= _fieldSize)
                defaultAiStartTilePos = _fieldSize - 1;

            return isAiRandomStart ? _rand.Next(0, _fieldSize) : defaultAiStartTilePos;
        }

        private void FindStartTileIndex(Tile startTile)
        {
            var count = -1;
            foreach (var tile in _tileList)
            {
                count++;
                if (tile != startTile)
                    continue;

                GetAiStartTileIndex = count;
                break;
            }
        }

        //todo - delete old func and comments!
        //Хотел порефакторить метод, чтобы избавиться от повторящихся значений в довольно страшненьком классе...
        //Чисто ради эексперимента попробовал использовать Linq, который я почти не знаю!
        //First - возвращает первое значение, которое соответствует требованиям -
        //В данном случае, это "значение - меньше чем Рол". т.е. именно тот вес, который требуется...
        //Как уже объяснял ранее, событие соответствует требованиям по шансу выпадения в % записанных в инспекторе.
        //в итоге сэкономил меньше строчек, чем потратил на этот коммент :D Зато выглядит симпатичнее, да...

        #region OldChangeTileType

        //private void ChangeTileType(Tile currentTile)
        //{
        //	_rand = new Random();
        //	var totalWeight = tileTypeChances.Sum(dictionary => dictionary.Value);
        //	var roll = _rand.Next(0, totalWeight);
        //	var cumulativeWeight = 0;
        //	foreach (var dictionary in tileTypeChances)
        //	{
        //		cumulativeWeight += dictionary.Value;
        //		if(roll >= cumulativeWeight) continue;
        //		currentTile.ChangeTileType(dictionary.Key);
        //		ChangeTileModifier(currentTile, dictionary.Key.modifierChances);
        //		return;
        //	}
        //}

        #endregion

        private void ChangeTileType(Tile currentTile)
        {
            var totalWeight = tileTypeChances.Sum(dictionary => dictionary.Value);
            var roll = new Random().Next(0, totalWeight);
            var selectedType = tileTypeChances.First(dictionary =>
            {
                roll -= dictionary.Value;
                return roll < 0;
            }).Key;

            currentTile.ChangeTileType(selectedType);
            ChangeTileModifier(currentTile, selectedType.modifierChances);
        }

        #region OldChangeTileModificator

        //private void ChangeTileModifier(Tile currentTile, List<TileModifierDictionary> table)
        //{
        //	_rand = new Random();
        //	var totalWeight = table.Sum(dictionary => dictionary.chance);
        //
        //	var roll = _rand.Next(0, totalWeight);
        //	var cumulativeWeight = 0;
        //
        //	foreach (var dictionary in table)
        //	{
        //		cumulativeWeight += dictionary.chance;
        //		if(roll >= cumulativeWeight) continue;
        //		currentTile.ChangeTileModifier(dictionary.value);
        //		return;
        //	}
        //}

        #endregion

        private void ChangeTileModifier(Tile currentTile, IReadOnlyCollection<TileModifierDictionary> table)
        {
            var totalWeight = table.Sum(dictionary => dictionary.chance);
            var roll = new Random().Next(0, totalWeight);
            var selectedModifier = table.First(dictionary =>
            {
                roll -= dictionary.chance;
                return roll < 0;
            }).value;

            currentTile.ChangeTileModifier(selectedModifier);
        }

        public void LoadData(GameData gameData, bool newGameBattle, bool firstStart)
        {
            sizeType = gameData.FieldSize;
            if (_tileList.Count != gameData.FieldData.Count)
            {
                Debug.Log("Field size in save not equal field size in game");
                return;
            }

            foreach (var td in gameData.FieldData)
            {
                _tileList[td.position].ChangeTileType(GetTileTypeFromTable(td.tileTypeID));
                _tileList[td.position].ChangeTileModifier(td.tileModifier);
                _tileList[td.position].ChangeStateMachine(td.tileCurrentState);
                _tileList[td.position].ChangeStartFlag(td.isStartTile);
            }
        }

        public void SaveData(ref GameData gameData, bool newGameBattle, bool firstStart)
        {
            gameData.FieldSize = sizeType;
            gameData.FieldData.Clear();
            for (var i = 0; i < _tileList.Count; i++)
            {
                var tileData = new TileData()
                {
                    position = i,
                    tileModifier = _tileList[i].TileModifier,
                    tileTypeID = GetTileTypeID(_tileList[i]),
                    tileCurrentState = _tileList[i].GetTileState,
                    isStartTile = _tileList[i].GetTileState == TileState.AvailableForSelectionState
                };
                gameData.FieldData.Add(tileData);
            }
        }

        private TileType GetTileTypeFromTable(int id)
        {
            var result = ScriptableObject.CreateInstance<TileType>();
            foreach (var tileType in tileTypeTable.tileTypeDB.Where(tileType => tileType.ID == id))
            {
                result = tileType.TileType;
            }

            return result;
        }

        private int GetTileTypeID(Tile tile)
        {
            var result = 0;
            foreach (var line in tileTypeTable.tileTypeDB.Where(line => line.TileType.cellType == tile.GetTileType))
            {
                result = line.ID;
            }

            return result;
        }
    }
}