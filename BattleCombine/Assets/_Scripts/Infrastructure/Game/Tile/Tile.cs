using BattleCombine.Interfaces;
using System;
using System.Collections.Generic;
using BattleCombine.Enums;
using BattleCombine.ScriptableObjects;
using TMPro;
using UnityEngine;

namespace BattleCombine.Gameplay
{
    public class Tile : MonoBehaviour, ITouchable //, IMovable
    {
        [SerializeField] private bool _startTile = false;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private string _stateName;
        [SerializeField] private List<GameObject> _tilesForChoosing;
        [SerializeField] private List<GameObject> _tilesNearThisTile;

        //TODO: назначать тайлу тип (от типа зависит иконка) и модификатор (от него зависит число)
        [SerializeField] private TileType _tileType;
        [SerializeField, Range(-100, 100)] private float _tileModifier;
        [SerializeField] private TextMeshPro _text;

        public StateMachine StateMachine;
        public AvailableForSelectionState AvailableForSelectionState;
        public ChosenState ChosenState;
        public DisabledState DisabledState;
        public EnabledState EnabledState;
        public FinalChoiceState FinalChoiceState;

        public List<GameObject> TilesForChoosing
        {
            get => _tilesForChoosing;
            set => _tilesForChoosing = value;
        }

        public List<GameObject> TilesNearThisTile
        {
            get => _tilesNearThisTile;
            private set => _tilesNearThisTile = value;
        }

        public CellType GetTileType => _tileType.cellType;

        //Дима: поправил с константы на сериализуемое поле
        [SerializeField] private LayerMask tileLayerMask;
        [SerializeField] float range; // Const scale для поиска tile

        public Action<Tile> onTileTouched;

        private void Start()
        {
            StateMachine = new StateMachine();

            AvailableForSelectionState = new AvailableForSelectionState(this, StateMachine);
            ChosenState = new ChosenState(this, StateMachine);
            DisabledState = new DisabledState(this, StateMachine);
            EnabledState = new EnabledState(this, StateMachine);
            FinalChoiceState = new FinalChoiceState(this, StateMachine);

            CheckTilesStateNearThisTile(this);
            //TODO: поменять алгоритм 
            _tileModifier = 5;
            SetupTile();
        }

        public void ChangeTileType(TileType type)
        {
            _tileType = type;
        }

        public void ChangeTileModifier(float modifier)
        {
            _tileModifier = modifier;
        }

        public void ChangeStartFlag(bool isStartTile)
        {
            _startTile = isStartTile;
        }

        private void SetupTile()
        {
            _text.text = _tileModifier.ToString();
            spriteRenderer.sprite = _tileType.sprite;
            if (_startTile)
            {
                StateMachine.Initialize(AvailableForSelectionState);
            }
            else
            {
                StateMachine.Initialize(EnabledState);
            }
        }

        private void Update()
        {
            _stateName =
                StateMachine.CurrentState.ToString(); //Вывожу имя класса для понимания в каком state находится tile
            //CheckTilesStateNearThisTile(this);
        }

        public void ChangeClolor(Color color)
        {
            spriteRenderer.color = color;
        }

        public void Touch()
        {
            StateMachine.CurrentState.Input();
            StateMachine.CurrentState.LogicUpdate();
            onTileTouched?.Invoke(this);
        }

        public void
            FindTileForChoosing(Tile tile,
                List<GameObject> list) // Метод нахождения соседних tile для изменения их состояния
        {
            Vector2 tilePosition = tile.transform.position;
            var localScale = gameObject.transform.localScale;
            float tileOverlapScaleX = localScale.x + range;
            float tileOverlapScaleY = localScale.y + range;
            Vector2 tileScale = new Vector2(tileOverlapScaleX, tileOverlapScaleY);
            Collider2D[] tileColliderForChoosing = Physics2D.OverlapBoxAll(tilePosition, tileScale, 45f, tileLayerMask);

            Collider2D thisTileCollider = this.gameObject.GetComponent<Collider2D>();
            foreach (var colliderTile in tileColliderForChoosing)
            {
                if (colliderTile == thisTileCollider)
                {
                    continue;
                }
                else
                {
                    
                    GameObject gameObjectTile = colliderTile.gameObject;
                    if(!list.Contains(gameObjectTile)) list.Add(gameObjectTile);
                }
            }
        }

        public void ClearTheTilesArray() // Метод очистки массива найденых tile 
        {
            TilesForChoosing.Clear();
        }

        public void CheckTilesStateNearThisTile(Tile tile) // Метод нахождения соседних tile
        {
            FindTileForChoosing(tile, TilesNearThisTile);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Vector3 position = gameObject.transform.position;
            float tileScaleX = gameObject.transform.localScale.x + range;
            float tileScaleY = gameObject.transform.localScale.y + range;
            Vector2 tileScale = new Vector2(tileScaleX, tileScaleY);
            Gizmos.DrawWireCube(position, tileScale);

            Gizmos.color = Color.green;
            float rangeOverlap = (float)((Math.Sqrt(Math.Pow(tileScaleX, 2) + Math.Pow(tileScaleY, 2))) / 2);
            Gizmos.DrawWireSphere(position, rangeOverlap);
        }
    }
}