using BattleCombine.Interfaces;
using System;
using System.Collections.Generic;
using BattleCombine.Enums;
using BattleCombine.ScriptableObjects;
using TMPro;
using UnityEngine;
using System.Linq;

namespace BattleCombine.Gameplay
{
    public class Tile : MonoBehaviour, ITouchable //, IMovable
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private TileStack _tileStack;
        [SerializeField] private bool _startTile = false;
        [SerializeField] private string _stateName;
        [SerializeField] private List<GameObject> _tilesForChoosing;
        [SerializeField] private List<GameObject> _tilesNearThisTile;

        //TODO: Ð½Ð°Ð·Ð½Ð°ÑÐ°ÑÑ ÑÐ°Ð¹Ð»Ñ ÑÐ¸Ð¿ (Ð¾Ñ ÑÐ¸Ð¿Ð° Ð·Ð°Ð²Ð¸ÑÐ¸Ñ Ð¸ÐºÐ¾Ð½ÐºÐ°) Ð¸ Ð¼Ð¾Ð´Ð¸ÑÐ¸ÐºÐ°ÑÐ¾Ñ (Ð¾Ñ Ð½ÐµÐ³Ð¾ Ð·Ð°Ð²Ð¸ÑÐ¸Ñ ÑÐ¸ÑÐ»Ð¾)
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
        public TileStack GetTileStack { get => _tileStack; }

        public CellType GetTileType => _tileType.cellType;

        //ÐÐ¸Ð¼Ð°: Ð¿Ð¾Ð¿ÑÐ°Ð²Ð¸Ð» Ñ ÐºÐ¾Ð½ÑÑÐ°Ð½ÑÑ Ð½Ð° ÑÐµÑÐ¸Ð°Ð»Ð¸Ð·ÑÐµÐ¼Ð¾Ðµ Ð¿Ð¾Ð»Ðµ
        [SerializeField] private LayerMask tileLayerMask;
        [SerializeField] float range; // Const scale Ð´Ð»Ñ Ð¿Ð¾Ð¸ÑÐºÐ° tile

        public Action<Tile> onTileTouched;

        private void Awake()
        {
            StateMachine = new StateMachine();

            AvailableForSelectionState = new AvailableForSelectionState(this, StateMachine);
            ChosenState = new ChosenState(this, StateMachine);
            DisabledState = new DisabledState(this, StateMachine);
            EnabledState = new EnabledState(this, StateMachine);
            FinalChoiceState = new FinalChoiceState(this, StateMachine);
        }

        private void Start()
        {
            CheckTilesStateNearThisTile(this);
            _tileStack = FindObjectOfType<TileStack>();
            //TODO: ïîìåíÿòü àëãîðèòì 
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
            
            _stateName = StateMachine.CurrentState.ToString(); //Âûâîæó èìÿ êëàññà äëÿ ïîíèìàíèÿ â êàêîì state íàõîäèòñÿ tile
        }

        public void ChangeClolor(Color color)
        {
            spriteRenderer.color = color;
        }

        public void Touch()
        {
            if(StateMachine.CurrentState.ToString() == ChosenState.ToString())
            {
                if(this.gameObject == _tileStack.TilesStack.Peek())
                {
                    StateMachine.CurrentState.Input();
                    StateMachine.CurrentState.LogicUpdate();
                }
                else
                {
                    Debug.Log("Âûáåðè äðóãîé tile!");
                }
            }
            else
            {
                if (_tileStack.TilesStack.Count() < _tileStack.SpeedPlayer)
                {
                    StateMachine.CurrentState.Input();
                    StateMachine.CurrentState.LogicUpdate();
                }
                else
                {
                    Debug.Log("Ñêîðîñòü çàêîí÷èëàñü");
                }
                
            }
        }

        public void FindTileForAction(Tile tile, List<GameObject> list, string nameState)// Ìåòîä íàõîæäåíèÿ ñîñåäíèõ tile äëÿ èçìåíåíèÿ èõ ñîñòîÿíèÿ
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
                GameObject gameObjectTile = colliderTile.gameObject;
                if (colliderTile == thisTileCollider || gameObjectTile.GetComponent<Tile>().StateMachine.CurrentState.ToString() != nameState)
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

        public void ClearTheTilesArray() // ÐÐµÑÐ¾Ð´ Ð¾ÑÐ¸ÑÑÐºÐ¸ Ð¼Ð°ÑÑÐ¸Ð²Ð° Ð½Ð°Ð¹Ð´ÐµÐ½ÑÑ tile 
        {
            TilesForChoosing.Clear();
        }

        private void CheckTilesStateNearThisTile(Tile tile) // Ìåòîä íàõîæäåíèÿ ñîñåäíèõ tile

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
                    TilesNearThisTile.Add(gameObjectTile);
                }
            }
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