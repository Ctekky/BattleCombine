using BattleCombine.Interfaces;
using System;
using System.Collections.Generic;
using BattleCombine.ScriptableObjects;
using TMPro;
using UnityEngine;

namespace _Scripts
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

        const int TileLayerMask = 1 << 6;
        const float range = 0.3f; // Const scale для поиска tile

        private void Start()
        {
            StateMachine = new StateMachine();

            AvailableForSelectionState = new AvailableForSelectionState(this, StateMachine);
            ChosenState = new ChosenState(this, StateMachine);
            DisabledState = new DisabledState(this, StateMachine);
            EnabledState = new EnabledState(this, StateMachine);
            FinalChoiceState = new FinalChoiceState(this, StateMachine);

            if (_startTile == true)
            {
                StateMachine.Initialize(AvailableForSelectionState);
            }
            else
            {
                StateMachine.Initialize(EnabledState);
            }

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

        private void SetupTile()
        {
            _text.text = _tileModifier.ToString();
            spriteRenderer.sprite = _tileType.sprite;
        }

        private void Update()
        {
            _stateName =
                StateMachine.CurrentState.ToString(); //Вывожу имя класса для понимания в каком state находится tile
        }

        public void ChangeClolor(Color color)
        {
            spriteRenderer.color = color;
        }

        public void Touch()
        {
            StateMachine.CurrentState.Input();
            StateMachine.CurrentState.LogicUpdate();
        }

        public void
            FindTileForChoosing(Tile tile,
                List<GameObject> list) // Метод нахождения соседних tile для изменения их состояния
        {
            Vector2 tilePosition = tile.transform.position;
            float tileOverlapScaleX = gameObject.transform.localScale.x + range;
            float tileOverlapScaleY = gameObject.transform.localScale.y + range;
            Vector2 tileScale = new Vector2(tileOverlapScaleX, tileOverlapScaleY);
            Collider2D[] tileColliderForChoosing = Physics2D.OverlapBoxAll(tilePosition, tileScale, 45f, TileLayerMask);

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
                    list.Add(gameObjectTile);
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