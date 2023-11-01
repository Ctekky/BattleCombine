using BattleCombine.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using BattleCombine.Enums;
using BattleCombine.ScriptableObjects;
using TMPro;
using UnityEngine;
using System.Linq;
using UnityEngine.EventSystems;
using UnityEngine.Rendering.Universal;
using Unity.VisualScripting;

namespace BattleCombine.Gameplay
{
    public class Tile : MonoBehaviour, IMovable, ITouchable, IPointerExitHandler//, IPointerEnterHandler
    {
        //[SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private SpriteRenderer borderSprite;
        [SerializeField] private SpriteRenderer tileSprite;
        [SerializeField] private SpriteRenderer typeSprite;
        [SerializeField] private Color tileNormalColor;
        [SerializeField] private Color tileChosenColor;
        [SerializeField] private Color tileNormalBorder;
        [SerializeField] private Color tileChosenBorder;
        [SerializeField] private TileStack tileStack;
        [SerializeField] private bool startTile = false;
        [SerializeField] private TileState tileCurrentState;
        [SerializeField] private bool _cantUse = false;
        [SerializeField] private List<GameObject> tilesForChoosing = new List<GameObject>();
        [SerializeField] private List<GameObject> tilesNearThisTile = new List<GameObject>();

        //TODO: set type and modifier to tile
        [SerializeField] private TileType tileType;
        [SerializeField, Range(-100, 100)] private int tileModifier;
        [SerializeField] private TextMeshPro text;
        [SerializeField] private bool isAlignPlayer1 = false;
        [SerializeField] private bool isAlignPlayer2 = false;

        public bool IsAlignPlayer1 => isAlignPlayer1;
        public bool IsAlignPlayer2 => isAlignPlayer2;

        //TODO: change this to proper algorithm
        private GameManager _gameManager;
        private float _firstTilePosScaleX;
        private float _firstTilePosScaleZ;


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
        public bool CantUse
        {
            get => _cantUse;
            set => _cantUse = value;
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
            SetupTile();
            _firstTilePosScaleX = transform.parent.localScale.x;
            _firstTilePosScaleZ = transform.parent.localScale.z;
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
            switch (type.cellType)
            {
                case CellType.Empty:
                    ChangeTileModifier(0);
                    break;
                case CellType.Shield:
                    ChangeTileModifier(0);
                    break;
            }
        }

        public void ChangeTileModifier(int modifier)
        {
            tileModifier = modifier;
            switch (tileModifier)
            {
                case > 0:
                    text.text = "+" + tileModifier.ToString(CultureInfo.CurrentCulture);
                    ChangeTileSprite(tileType.spriteUp);
                    break;
                case 0:
                    text.text = "";
                    ChangeTileSprite(tileType.spriteUp);
                    break;
                case < 0:
                    text.text = tileModifier.ToString(CultureInfo.CurrentCulture);
                    ChangeTileSprite(tileType.spriteDown);
                    break;
            }
        }

        private void ChangeTileSprite(Sprite newSprite)
        {
            typeSprite.sprite = newSprite;
        }

        public void ChangeStartFlag(bool isStartTile)
        {
            startTile = isStartTile;
        }

        private void SetupTile()
        {
            tileNormalColor = Color.white;
            tileNormalBorder = new Color(255, 255, 255, 0);
            tileChosenColor = _gameManager.GetTileColorSetting();
            tileChosenBorder = _gameManager.GetBorderColorSetting();
            tileSprite.color = tileNormalColor;
            borderSprite.color = tileNormalBorder;
            ChangeTileModifier(tileModifier);
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

        public TileState GetTileState
        {
            get => tileCurrentState;
        }

        public void SetBorderColor(bool state)
        {
            borderSprite.color = state ? tileChosenBorder : tileNormalBorder;
        }

        public void SetTileColor(bool state)
        {
            tileSprite.color = state ? tileChosenBorder : tileNormalColor;
        }

        public void Touch()
        {
            switch (tileStack.IDPlayer)
            {
                case IDPlayer.Player1:
                    ActionForTileTouch(tileStack.TilesListPlayer1);
                    break;
                case IDPlayer.Player2:
                    ActionForTileTouch(tileStack.TilesListPlayer2);
                    break;
            }
        }

        public void FingerMoved()
        {
            if (tileStack.GetGameManager.GetInputMode == InputMod.Touch)
            {
                return;
            }
            else
            {
                switch (tileStack.IDPlayer)
                {
                    case IDPlayer.Player1:
                        ActionForTileFingerMove(tileStack.TilesListPlayer1);
                        break;
                    case IDPlayer.Player2:
                        ActionForTileFingerMove(tileStack.TilesListPlayer2);
                        break;
                }
            }
        }
        public void FindTileForAction(Tile tile, List<GameObject> list, TileState nameState) //change state for nearest tiles
        {
            Vector2 tilePosition = tile.transform.position;
            var localScale = gameObject.transform.localScale;
            float tileOverlapScaleX = localScale.x * _firstTilePosScaleX;
            float tileOverlapScaleY = localScale.y * _firstTilePosScaleZ;
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
            float tileOverlapScaleX = localScale.x * _firstTilePosScaleX;
            float tileOverlapScaleY = localScale.y * _firstTilePosScaleZ;
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
                if (tileGameObject == this.gameObject ||
                    tileGameObject.GetComponent<Tile>().tileCurrentState == TileState.DisabledState)
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

        public List<GameObject> FindTileDisabledTileForNextMove(List<GameObject> list)
        {
            List<GameObject> listAfterSort = new List<GameObject>();
            foreach (GameObject tileGameObject in list)
            {
                Tile tile = tileGameObject.GetComponent<Tile>();
                if (tile.tileCurrentState == TileState.DisabledState ||
                    tile.tileCurrentState == TileState.FinalChoiceState)
                {
                    continue;
                }

                listAfterSort.Add(tileGameObject);
            }

            return listAfterSort;
        }

        private void ActionForTileTouch(List<GameObject> list)
        {
            if (_gameManager._currentPlayerName == "Player1" & !isAlignPlayer1) return;
            if (_gameManager._currentPlayerName == "Player2" & !isAlignPlayer2) return;

            if (StateMachine.CurrentState.ToString() == ChosenState.ToString())
            {
                if (this.gameObject == list.Last())
                {
                    StateMachine.CurrentState.Input();
                    StateMachine.CurrentState.LogicUpdate();
                    if (list.Count() < tileStack.SpeedPlayer)
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
                if (list.Count() < tileStack.SpeedPlayer)
                {
                    StateMachine.CurrentState.Input();
                    StateMachine.CurrentState.LogicUpdate();
                    if (list.Count() == tileStack.SpeedPlayer)
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
        }

        private void ActionForTileFingerMove(List<GameObject> list)
        {
            if (_gameManager._currentPlayerName == "Player1" & !isAlignPlayer1) return;
            if (_gameManager._currentPlayerName == "Player2" & !isAlignPlayer2) return;
            if (StateMachine.CurrentState.ToString() == ChosenState.ToString())
            {
                if (this.gameObject == list.Last())
                {
                    return;
                }
                else if (this.gameObject == list[(list.Count - 2)]) //list.ElementAt(list.Count - 2))
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
                if (list.Count() < tileStack.SpeedPlayer)
                {
                    StateMachine.CurrentState.Input();
                    StateMachine.CurrentState.LogicUpdate();
                    if (list.Count() == tileStack.SpeedPlayer)
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
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _cantUse = false;
            //Debug.Log("Exit");
        }

        /*public void OnPointerEnter(PointerEventData eventData)
        {
            if(tileCurrentState == TileState.ChosenState)
            {
                _cantUse = true;
                Debug.Log("Enter");
            }
        }*/

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Vector3 position = gameObject.transform.position;
            var localScale = gameObject.transform.localScale;
            float tileScaleX = localScale.x * _firstTilePosScaleX;
            float tileScaleY = localScale.y * _firstTilePosScaleZ;
            Vector2 tileScale = new Vector2(tileScaleX, tileScaleY);
            Gizmos.DrawWireCube(position, tileScale);

            Gizmos.color = Color.green;
            float rangeOverlap = (float)((Math.Sqrt(Math.Pow(tileScaleX, 2) + Math.Pow(tileScaleY, 2))) / 2);
            Gizmos.DrawWireSphere(position, rangeOverlap);
       }
    }
}