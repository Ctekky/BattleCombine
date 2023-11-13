using System;
using System.Collections.Generic;
using System.Linq;
using BattleCombine.Enums;
using UnityEngine;
using Random = System.Random;

namespace BattleCombine.Gameplay
{
	public class CreateField : MonoBehaviour
	{
		public Action<Tile> onTileTouched;

		[Header("Scale or not on start")] [SerializeField]
		private bool makeScale;
		[Header("Start Tile set")] [SerializeField]
		private bool isAiRandomStart;

		[SerializeField] private int defaultAiStartTilePos;
		[SerializeField] private bool isPlayerRandomStart;
		
		private List<int> defaultPlayerStartTilePos;

		[SerializeField] private TileStack tileStack;

		[Header("FieldSize")] [SerializeField]
		private FieldSize sizeType;
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

		[SerializeField] private GameManager gameManager;

		private List<Tile> _tileList;
		private Transform _fieldParent;
		private GameObject _mainField;
		private Random _rand;
		private int _fieldSize;
		private bool _isTileFullSetup;

		public Tile GetAiStartTile {get; private set;}
		public int GetAiStartTileIndex {get; private set;}
		public List<Tile> GetTileList => _tileList;
		public int GetFieldSize => _fieldSize;

		private void Awake()
		{
			_tileList = new List<Tile>();
			_mainField = this.gameObject;
			_fieldParent = tileParent.transform;
			_isTileFullSetup = false;
			tileStack.DescribeStartingTileList();
			
		}

		public void SetupField(bool changeFieldSize, FieldSize fieldSize)
		{
			if (changeFieldSize) sizeType = fieldSize;
			ChangeFieldSize();
		}

		private void Update()
		{
			if(!_isTileFullSetup) SetupTileOnField();
		}

		private void ChangeFieldSize()
		{
			_fieldSize = sizeType switch {
				FieldSize.UltraSmall => 5,
				FieldSize.Small => 6,
				FieldSize.Medium => 7,
				FieldSize.Large => 8,
				_ => throw new ArgumentOutOfRangeException()
			};

			defaultPlayerStartTilePos = new List<int>() {
				0, _fieldSize
			};

			AddTileToField();
			ModifyTitleSize();

			if(!makeScale) return;
			var scaler = new FieldScaler();
			scaler.ScaleMainField(this.gameObject, edgeOffset);
		}

		private void SetupTileOnField()
		{
			foreach (var tileInList in _tileList)
			{
				tileInList.CheckTilesStateNearThisTile(tileInList);
			}

			_isTileFullSetup = true;
		}

		private void AddTileToField()
		{
			var startPlayerTile = SetPlayerStartTileIndex();
			var startAiTile = SetAiStartTileIndex();

			var newTile = new FieldCreateFactory(tile.gameObject);
	
			for(var i = 0; i < _fieldSize; i++)
			{
				for(var j = 0; j < _fieldSize; j++)
				{
					var currentTile = newTile.Create(_fieldParent);
					currentTile.transform.position = tileParent.transform.position
					                                 + new Vector3(j * tileOffset, i * tileOffset, 0);

					currentTile.transform.Rotate(90, 180, 0);

					var tileComponent = currentTile.GetComponent<Tile>();
					ChangeTileType(tileComponent);

					tileComponent.SetGameManager(gameManager);

					_tileList.Add(tileComponent);

					tileComponent.onTileTouched += touchedTile => onTileTouched?.Invoke(touchedTile);

					if(i != _fieldSize - 1 || j != startAiTile) continue;

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
				if(index != startPlayerTile[0] && index != startPlayerTile[1])
					continue;

				ApplyStartTileStatus(tile);
				tileStack.AddTileToStartingList(tile);
				tile.SetAlignTileToPlayer2(true);
			}
		}

		private void ModifyTitleSize()
		{
			tileParent.transform.localScale = _fieldSize switch {
				5 => new Vector3(tutorFieldScale, 0, tutorFieldScale),
				6 => new Vector3(smallFieldScale, 0, smallFieldScale),
				7 => new Vector3(mediumFieldScale, 0, mediumFieldScale),
				8 => new Vector3(largeFieldScale, 0, largeFieldScale),
				_ => tileParent.transform.localScale
			};
		}

		private void ChangeTileModifier(Tile currentTile, List<TileModifierDictionary> table)
		{
			_rand = new Random();
			var totalWeight = table.Sum(dictionary => dictionary.Chance);

			var roll = _rand.Next(0, totalWeight);
			var cumulativeWeight = 0;

			foreach (var dictionary in table)
			{
				cumulativeWeight += dictionary.Chance;
				if(roll >= cumulativeWeight) continue;
				currentTile.ChangeTileModifier(dictionary.Value);
				return;
			}
		}

		private void RefreshEmptyTile(Tile currentTile)
		{
			_rand = new Random();
			var roll = _rand.Next(0, 100);
			if(roll >= tileRefreshChance) return;
			ChangeTileType(currentTile);
			currentTile.StateMachine.ChangeState(currentTile.EnabledState);
		}

		public void RefreshField()
		{
			foreach (var currentTile in _tileList)
			{
				if(currentTile.GetTileState == TileState.DisabledState)
				{
					RefreshEmptyTile(currentTile);
				}
				else if((currentTile.GetTileType != CellType.Shield && currentTile.TileModifier < 9))
				{
					if(currentTile.TileModifier == -1)
						currentTile.ChangeTileModifier(currentTile.TileModifier + 2);

					else
						currentTile.ChangeTileModifier(currentTile.TileModifier + 1);
				}
			}
		}

		private void ChangeTileType(Tile currentTile)
		{
			_rand = new Random();
			var totalWeight = tileTypeChances.Sum(dictionary => dictionary.Value);
			var roll = _rand.Next(0, totalWeight);
			var cumulativeWeight = 0;
			foreach (var dictionary in tileTypeChances)
			{
				cumulativeWeight += dictionary.Value;
				if(roll >= cumulativeWeight) continue;
				currentTile.ChangeTileType(dictionary.Key);
				ChangeTileModifier(currentTile, dictionary.Key.modifierChances);
				return;
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

			if(!isPlayerRandomStart)
				return defaultPlayerStartTilePos;

			defaultPlayerStartTilePos[0] = _rand.Next(0, _fieldSize / 2);
			defaultPlayerStartTilePos[1] = _rand.Next(defaultPlayerStartTilePos[0]+2, _fieldSize);
			return defaultPlayerStartTilePos;
		}

		public void SetupGameManager(GameManager gm)
		{
			gameManager = gm;
		}

		private int SetAiStartTileIndex()
		{
			_rand = new Random();

			if(defaultAiStartTilePos >= _fieldSize)
				defaultAiStartTilePos = _fieldSize - 1;

			return isAiRandomStart ? _rand.Next(0, _fieldSize) : defaultAiStartTilePos;
		}
		
		private void FindStartTileIndex(Tile startTile)
		{
			var count = -1;
			foreach (var tile in _tileList)
			{
				count++;
				if(tile != startTile)
					continue;

				GetAiStartTileIndex = count;
				break;
			}
		}
	}
}