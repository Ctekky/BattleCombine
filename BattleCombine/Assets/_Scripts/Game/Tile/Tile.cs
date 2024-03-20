using BattleCombine.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using BattleCombine.Enums;
using BattleCombine.ScriptableObjects;
using TMPro;
using UnityEngine;
using System.Linq;
using BattleCombine.Animations;
using BattleCombine.Services;
using BattleCombine.Services.Other;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using Zenject;
using _Scripts.Temp;
using FMODUnity;

namespace BattleCombine.Gameplay
{
    public class Tile : MonoBehaviour, IMovable, ITouchable, IPointerExitHandler //, IPointerEnterHandler
    {
        //[SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private SpriteRenderer borderSprite;
        [SerializeField] private SpriteRenderer tileSprite;
        [SerializeField] private SpriteRenderer typeSprite;
        [SerializeField] private Color tileNormalColor;
        [SerializeField] private Color tileChosenColor;
        [SerializeField] private Color tileChosenEnemyColor;
        [SerializeField] private Color tileNormalBorder;
        [SerializeField] private Color tileChosenBorder;
        [SerializeField] private Color tileChosenEnemyBorder;
        [SerializeField] private TileStack tileStack;
        [SerializeField] private bool startTile = false;
        [SerializeField] private TileState tileCurrentState;
        [SerializeField] private bool _cantUse = false;
        [SerializeField] private int tileID;
        [SerializeField] private List<GameObject> tilesForChoosing = new List<GameObject>();
        [SerializeField] private List<GameObject> tilesNearThisTile = new List<GameObject>();

        [SerializeField] private Color tileEnemyColor;

        [FormerlySerializedAs("currentNormalTile")] [SerializeField]
        private Color currentNormalColor;

        //Animation part
        [SerializeField] private TileTextAnimationHelper tileTextAnimationHelper;
        [SerializeField] private TileAnimationHelper tileAnimationHelper;

        //TODO: set type and modifier to tile
        [SerializeField] private TileType tileType;
        [SerializeField, Range(-100, 100)] private int tileModifier;
        [SerializeField] private TextMeshPro text;
        [SerializeField] private bool isAlignPlayer1 = false;
        [SerializeField] private bool isAlignPlayer2 = false;


        //TODO get separate scriptable object for this
        [SerializeField] private Sprite tileNormalImage;
        [SerializeField] private Sprite tileEnemyEndImage;

        [Inject] private GlobalEventService _globalEventService;

        public bool IsAlignPlayer1 => isAlignPlayer1;
        public bool IsAlignPlayer2 => isAlignPlayer2;

        //TODO: change this to proper algorithm
        private float _firstTilePosScaleX;
        private float _firstTilePosScaleZ;

        public StateMachine StateMachine;
        public AvailableForSelectionState AvailableForSelectionState;
        public ChosenState ChosenState;
        public DisabledState DisabledState;
        public EnabledState EnabledState;
        public FinalChoiceState FinalChoiceState;
        private bool _isCanPlaySound;
        [SerializeField] private SOTileSoundFMODLib TileSoundLib;

        public List<GameObject> TilesForChoosing
        {
            get => tilesForChoosing;
            set => tilesForChoosing = value;
        }

        public bool CanPlaySound
        {
            get => _isCanPlaySound;
            set => _isCanPlaySound = value;
        }

        public List<GameObject> TilesNearThisTile
        {
            get => tilesNearThisTile;
            private set => tilesNearThisTile = value;
        }

        public void PlayFMODSound(TileSound tileSound)
        {
            if(!_isCanPlaySound) return;
            foreach (var soundEvent in TileSoundLib.TileSoundDict.Where(soundEvent => soundEvent.ID == tileSound))
            {
                RuntimeManager.PlayOneShot(soundEvent.soundEventPath);
            }
        }
        
        public bool CantUse
        {
            get => _cantUse;
            set => _cantUse = value;
        }

        public int TileModifier
        {
            get => tileModifier;
            set => tileModifier = value;
        }

        public int TileID
        {
            get => tileID;
            set => tileID = value;
        }

        public SpriteRenderer BorderSpriteTile
        {
            get => borderSprite;
            set => borderSprite = value;
        }

        public Color GetChosenBorder
        {
            get => tileChosenBorder;
        }

        public Color GetChosenEnemyBorder
        {
            get => tileChosenEnemyBorder;
        }

        public TileStack GetTileStack => tileStack;
        public CellType GetTileType => tileType.cellType;
        public TileType GetFullTileType => tileType;
        public SpriteRenderer GetSprite => tileSprite;

        //set tile mask
        [SerializeField] private LayerMask tileLayerMask;

        #region Events

        public Action<Tile> onTileTouched;
        public Action<bool> onSpeedEnded;
        public Action<Tile> onTileRefreshAnimationTriggered;

        #endregion


        private void Awake()
        {
            StateMachine = new StateMachine();
            AvailableForSelectionState = new AvailableForSelectionState(this, StateMachine);
            ChosenState = new ChosenState(this, StateMachine);
            DisabledState = new DisabledState(this, StateMachine);
            EnabledState = new EnabledState(this, StateMachine);
            FinalChoiceState = new FinalChoiceState(this, StateMachine);
            tileTextAnimationHelper.onTileTextAnimationTrigger += OnAnimationTileTextUpTrigger;
            tileAnimationHelper.onTileAnimationTrigger += OnAnimationTileTrigger;
            _isCanPlaySound = false;
        }

        public void SetUpGlobalEventService(GlobalEventService ges)
        {
            _globalEventService = ges;
            _globalEventService.onFieldRefresh += OnFieldRefreshEvent;
        }

        private void OnDisable()
        {
            tileTextAnimationHelper.onTileTextAnimationTrigger -= OnAnimationTileTextUpTrigger;
            _globalEventService.onFieldRefresh -= OnFieldRefreshEvent;
            tileAnimationHelper.onTileAnimationTrigger -= OnAnimationTileTrigger;
        }

        public void OnTileRefreshEvent()
        {
            tileAnimationHelper.SetAnimationBool(true);
        }

        private void OnAnimationTileTrigger()
        {
            onTileRefreshAnimationTriggered?.Invoke(this);
            tileAnimationHelper.SetAnimationBool(false);
        }

        private void OnAnimationTileTextUpTrigger()
        {
            //test this parameter (mb not here)
            SetAnimationTextBool(false);
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

        private void OnFieldRefreshEvent()
        {
            if (GetTileState == TileState.DisabledState) return;
            if (GetTileType == CellType.Shield) return;
            switch (TileModifier)
            {
                case >= 9:
                    return;
                case -1:
                    ChangeTileModifier(TileModifier + 2, true);
                    break;
                default:
                    ChangeTileModifier(TileModifier + 1, true);
                    break;
            }
        }

        private void Start()
        {
            CheckTilesStateNearThisTile(this);
            var localScale = transform.parent.localScale;
            _firstTilePosScaleX = localScale.x;
            _firstTilePosScaleZ = localScale.z;
        }

        public void SetAlignTileToPlayer1(bool flag)
        {
            isAlignPlayer1 = flag;
        }

        private void SetAnimationTextBool(bool flag)
        {
            tileTextAnimationHelper.SetAnimationBool(flag);
        }

        public void SetAlignTileToPlayer2(bool flag)
        {
            isAlignPlayer2 = flag;
        }

        public void ChangeTileType(TileType type)
        {
            tileType = type;
            if (type.cellType == CellType.Void)
            {
                tileSprite.sprite = tileType.spriteUp;
                gameObject.layer = 2;
            }
            else
            {
                tileSprite.sprite = tileNormalImage;
                gameObject.layer = 6;
            }

            switch (type.cellType)
            {
                case CellType.Empty:
                    ChangeTileModifier(0, false);
                    break;
                case CellType.Shield:
                    ChangeTileModifier(0, false);
                    break;
                case CellType.Void:
                    ChangeTileModifier(0, false);
                    break;
            }
        }

        public void ChangeTileModifier(int modifier, bool animationFlag)
        {
            tileModifier = modifier;
            if (animationFlag)
            {
                SetAnimationTextBool(true);
            }
            else
            {
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
        }

        private void ChangeTileSprite(Sprite newSprite)
        {
            typeSprite.sprite = newSprite;
        }

        public void ChangeStartFlag(bool isStartTile)
        {
            startTile = isStartTile;
        }

        public void UpdateTileInInspector()
        {
            ChangeTileType(tileType);
            ChangeTileModifier(tileModifier, false);
        }

        public void SetupTile(TileStack mainTileStack, ColorSettings tileColorSettings, bool isLevelDesign)
        {
            tileStack = mainTileStack;
            tileNormalColor = Color.white;
            tileNormalBorder = new Color(255, 255, 255, 0);
            tileChosenColor = tileColorSettings.tileColor;
            tileChosenBorder = tileColorSettings.borderColor;
            tileChosenEnemyColor = tileColorSettings.tileEnemyColor;
            tileChosenEnemyBorder = tileColorSettings.borderEnemyColor;
            tileSprite.color = tileNormalColor;
            borderSprite.color = tileNormalBorder;
            if (isLevelDesign) return;
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

        public void ChangeStateMachine(TileState currentState)
        {
            switch (currentState)
            {
                case TileState.AvailableForSelectionState:
                    StateMachine.ChangeState(AvailableForSelectionState);
                    break;
                case TileState.ChosenState:
                    StateMachine.ChangeState(ChosenState);
                    break;
                case TileState.DisabledState:
                    StateMachine.ChangeState(DisabledState);
                    break;
                case TileState.EnabledState:
                    StateMachine.ChangeState(EnabledState);
                    break;
                case TileState.FinalChoiceState:
                    StateMachine.ChangeState(FinalChoiceState);
                    break;
                default:
                    Debug.Log("No state");
                    break;
            }
        }

        public TileState GetTileState
        {
            get => tileCurrentState;
        }

        public void SetBorderColor(bool state, IDPlayer playerID)
        {
            if (state)
            {
                if (playerID == IDPlayer.Player1)
                {
                    borderSprite.color = tileChosenBorder;
                }
                else if (playerID == IDPlayer.Player2)
                {
                    borderSprite.color = tileChosenEnemyBorder;
                }
            }
            else
            {
                borderSprite.color = tileNormalBorder;
            }
        }

        public void SetTileColor(bool state, IDPlayer playerID)
        {
            if (state)
            {
                if (playerID == IDPlayer.Player1)
                {
                    tileSprite.color = tileChosenColor;
                }
                else if (playerID == IDPlayer.Player2)
                {
                    tileSprite.color = tileChosenEnemyColor;
                }
            }
            else
            {
                tileSprite.color = tileNormalColor;
            }
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

        public void FindTileForAction(Tile tile, List<GameObject> list,
            TileState nameState) //change state for nearest tiles
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

        public void TileEnemyEndTurn(bool flag)
        {
            if (flag) currentNormalColor = tileNormalColor;
            tileNormalColor = flag ? tileEnemyColor : currentNormalColor;
            typeSprite.color = flag ? tileEnemyColor : currentNormalColor;
        }

        private void ActionForTileTouch(List<GameObject> list)
        {
            //if (_gameManager._currentPlayerName == "Player1" & !isAlignPlayer1) return;
            //if (_gameManager._currentPlayerName == "Player2" & !isAlignPlayer2) return;

            if (StateMachine.CurrentState.ToString() == ChosenState.ToString())
            {
                if (this.gameObject == list.Last())
                {
                    StateMachine.CurrentState.Input();
                    StateMachine.CurrentState.LogicUpdate();
                    if (list.Count() < tileStack.SpeedPlayer)
                    {
                        onSpeedEnded?.Invoke(false);
                        //_gameManager.SpeedIsOver(false);
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
                        onSpeedEnded?.Invoke(true);
                        //_gameManager.SpeedIsOver(true);
                    }
                }
                else
                {
                    onSpeedEnded?.Invoke(true);
                    //_gameManager.SpeedIsOver(true);
                    Debug.Log("Current move over");
                }
            }
        }

        private void ActionForTileFingerMove(List<GameObject> list)
        {
            //if (_gameManager._currentPlayerName == "Player1" & !isAlignPlayer1) return;
            //if (_gameManager._currentPlayerName == "Player2" & !isAlignPlayer2) return;
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
                        onSpeedEnded?.Invoke(true);
                        //_gameManager.SpeedIsOver(true);
                    }
                }
                else
                {
                    onSpeedEnded?.Invoke(true);
                    //_gameManager.SpeedIsOver(true);
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