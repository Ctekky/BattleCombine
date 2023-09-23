using BattleCombine.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
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
        [SerializeField] private TileStack tileStack;
        [SerializeField] private bool startTile = false;
        [SerializeField] private string stateName;
        [SerializeField] private TileState tileCurrentState;
        [SerializeField] private List<GameObject> tilesForChoosing;
        [SerializeField] private List<GameObject> tilesNearThisTile;

        //TODO: set type and modifier to tile
        [SerializeField] private TileType tileType;
        [SerializeField, Range(-100, 100)] private int tileModifier;
        [SerializeField] private TextMeshPro text;
        [SerializeField] private bool isAlignPlayer1;
        [SerializeField] private bool isAlignPlayer2;

        public bool IsAlignPlayer1 => isAlignPlayer1;
        public bool IsAlignPlayer2 => isAlignPlayer2;

        //TODO: change this to proper algorithm
        private GameManager _gameManager;

        public StateMachine StateMachine;
        public AvailableForSelectionState AvailableForSelectionState;
        public ChosenState ChosenState;
        public DisabledState DisabledState;
        public EnabledState EnabledState;
        public FinalChoiceState FinalChoiceState;

        public List<GameObject> TilesForChoosing
        {
            get => tilesForChoosing;
            set => tilesForChoosing = value;
        }

        public List<GameObject> TilesNearThisTile
        {
            get => tilesNearThisTile;
            private set => tilesNearThisTile = value;
        }

        public int TileModifier
        {
            get => tileModifier;
            private set => tileModifier = value;
        }

        public TileStack GetTileStack
        {
            get => tileStack;
        }

        public CellType GetTileType => tileType.cellType;

        //set tile mask
        [SerializeField] private LayerMask tileLayerMask;
        [SerializeField] float range; // set player speed

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
            tileStack = FindObjectOfType<TileStack>();
            //TODO: change algorithm to set up modifier
            tileModifier = 5;

            SetupTile();
        }

        //TODO: change this to proper algorithm
        public void SetGameManager(GameManager gameManager)
        {
            _gameManager = gameManager;
        }

        public void SetAlignTileToPlayer1(bool flag)
        {
            isAlignPlayer1 = flag;
        }

        public void SetAlignTileToPlayer2(bool flag)
        {
            isAlignPlayer2 = flag;
        }

        public void ChangeTileType(TileType type)
        {
            tileType = type;
        }

        public void ChangeTileModifier(int modifier)
        {
            tileModifier = modifier;
        }

        public void ChangeStartFlag(bool isStartTile)
        {
            startTile = isStartTile;
        }

        private void SetupTile()
        {
            text.text = tileModifier.ToString(CultureInfo.CurrentCulture);
            spriteRenderer.sprite = tileType.sprite;
            if (startTile)
            {
                StateMachine.Initialize(AvailableForSelectionState);
            }
            else
            {
                StateMachine.Initialize(EnabledState);
            }
        }

        public void SetCurrentState(TileState currentState)
        {
            tileCurrentState = currentState;
        }

        public void ChangeClolor(Color color)
        {
            spriteRenderer.color = color;
        }

        public void Touch()
        {
            switch (tileStack.IDPlayer)
            {
                case IDPlayer.Player1:
                    ActionForTile(tileStack.TilesStackPlayer1);
                    break;
                case IDPlayer.Player2:
                    ActionForTile(tileStack.TilesStackPlayer2);
                    break;
            }
        }

        public void FindTileForAction(Tile tile, List<GameObject> list,
            TileState nameState) //change state for nearest tiles
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
                var tileState = gameObjectTile.GetComponent<Tile>().tileCurrentState;
                if (colliderTile == thisTileCollider || tileState != nameState)
                {
                    continue;
                }

                if (!list.Contains(gameObjectTile)) list.Add(gameObjectTile);
            }
        }

        public void ClearTheTilesArray() //Clear tiles array
        {
            TilesForChoosing.Clear();
        }

        public void CheckTilesStateNearThisTile(Tile tile) //find near tile method

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

                GameObject gameObjectTile = colliderTile.gameObject;
                TilesNearThisTile.Add(gameObjectTile);
            }
        }

        public void ChangeTileStateInStack() //Change prefirst tlie state
        {
            GameObject gameObjectTile = this.gameObject;
            foreach (GameObject tileGameObject in GetTileStack.NextMoveTiles)
            {
                if (tileGameObject == this.gameObject)
                {
                    continue;
                }
                else
                {
                    Tile tile = tileGameObject.GetComponent<Tile>();
                    tile.StateMachine.ChangeState(tile.EnabledState);
                }
            }
        }

        private void ActionForTile(Stack<GameObject> stack)
        {
            if (_gameManager._currentPlayerName == "Player1" & !isAlignPlayer1) return;
            if (_gameManager._currentPlayerName == "Player2" & !isAlignPlayer2) return;
            if (StateMachine.CurrentState.ToString() == ChosenState.ToString())
            {
                if (this.gameObject == stack.Peek())
                {
                    StateMachine.CurrentState.Input();
                    StateMachine.CurrentState.LogicUpdate();
                    if (stack.Count() < tileStack.SpeedPlayer)
                    {
                        _gameManager.SpeedIsOver(false);
                    }
                }
                else
                {
                    Debug.Log("Pick another tile!");
                }
            }
            else
            {
                if (stack.Count() < tileStack.SpeedPlayer)
                {
                    StateMachine.CurrentState.Input();
                    StateMachine.CurrentState.LogicUpdate();
                    if (stack.Count() == tileStack.SpeedPlayer)
                    {
                        _gameManager.SpeedIsOver(true);
                    }
                }
                else
                {
                    _gameManager.SpeedIsOver(true);
                    Debug.Log("Current move over");
                }
            }
            /*
             if (StateMachine.CurrentState.ToString() == ChosenState.ToString())
            {
                if (this.gameObject == stack.Peek())
                {
                    StateMachine.CurrentState.Input();
                    StateMachine.CurrentState.LogicUpdate();
                }
                else
                {
                    Debug.Log("Pick another tile!");
                }
            }
            else
            {
                if (stack.Count() < tileStack.SpeedPlayer)
                {
                    StateMachine.CurrentState.Input();
                    StateMachine.CurrentState.LogicUpdate();
                }
                else
                {
                    Debug.Log("Current move over");
                }
            }
             */
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Vector3 position = gameObject.transform.position;
            var localScale = gameObject.transform.localScale;
            float tileScaleX = localScale.x + range;
            float tileScaleY = localScale.y + range;
            Vector2 tileScale = new Vector2(tileScaleX, tileScaleY);
            Gizmos.DrawWireCube(position, tileScale);

            Gizmos.color = Color.green;
            float rangeOverlap = (float)((Math.Sqrt(Math.Pow(tileScaleX, 2) + Math.Pow(tileScaleY, 2))) / 2);
            Gizmos.DrawWireSphere(position, rangeOverlap);
        }
    }
}