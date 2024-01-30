using System;
using System.Collections.Generic;
using System.Linq;
using _Scripts.Game.Stats;
using _Scripts.UI;
using BattleCombine.Ai;
using BattleCombine.Data;
using BattleCombine.Enums;
using BattleCombine.Gameplay;
using BattleCombine.Interfaces;
using BattleCombine.ScriptableObjects;
using UnityEngine;
using Zenject;

namespace BattleCombine.Services
{
    public class ArcadeGameService : MonoBehaviour, ISaveLoad
    {
        #region Dependency

        [Inject] private MainGameService _mainGameService;
        [Inject] private SaveManager _saveManager;
        [Inject] private PlayerAccount _playerAccount;
        [SerializeField] private InputService.InputService inputService;
        //TODO: need inject UI to work with

        #endregion

        #region GameField

        [SerializeField] private FieldSize fieldSize;
        [SerializeField] private GameObject gameField;
        [SerializeField] private TileType emptyTile;

        #endregion

        #region Players

        [SerializeField] private GameObject player1;
        [SerializeField] private GameObject player2;

        [SerializeField] public string currentPlayerName;
        [SerializeField] private Player currentPlayer;

        #endregion

        #region Bot

        //добавил, чтоб ловить боту момент смены хода
        public Action onPlayerChange;
        public Action onBattleEnd;
        [SerializeField] private AiHandler aiHandler;

        #endregion

        #region battle and stats

        [SerializeField] private StatsCollector statsCollector;
        [SerializeField] private IncreaseStats increaseStats;
        [SerializeField] private Fight fight;

        #endregion

        #region Moves

        private Step _stepChecker;
        private SequenceMoves _sequenceMoves;

        #endregion

        #region Boosters
        [Header("BoosterHandler")]
        [SerializeField] private BoostHandler _boostHandler;
        #endregion

        [Header("Other")]
        [SerializeField] private int currentStepInTurn;
        [SerializeField] private int currentTurn;
        [SerializeField] private int stepsInTurn;
        [SerializeField] private UiHelper arcadeUIHelper;
        [SerializeField] private int expToLevel;
        [SerializeField] private int attackByLevel;
        [SerializeField] private int healthByLevel;
        [SerializeField] private int speedByLevel;
        [SerializeField] private int whenUpSpeed;

        private PathCheck pathCheck = new PathCheck();
        private bool _isTypeStandart;
        private int _maxPossibleMove;
        private GameObject _previousTile;
        private GameObject _currentTile;
        private bool _canMove = false;
        private bool _isFirstRun = true;
        [SerializeField] private bool isOnWinScreen;
        [SerializeField] private string loserName;

        //todo - (temp) Kirill Add to control ai hp status (to change state)
        public int GetPlayerAiHealth => player2.GetComponent<Player>().HealthValue;

        public int GetCurrentStepInTurn => currentStepInTurn;

        #region System functions

        private void OnDisable()
        {
            inputService.onFingerUp -= InputFingerUp;
            arcadeUIHelper.onStatChange -= StatChange;
            arcadeUIHelper.onLoseClick -= LoseBattle;
            arcadeUIHelper.onWinClick -= WinBattle;
            //TODO add properly lose screen
            arcadeUIHelper.OnEndBattle -= LoseBattle;
        }

        private void WinBattle()
        {
            arcadeUIHelper.ExitBattleScene();
        }

        private void LoseBattle()
        {
            _mainGameService.ArcadeCurrentScore = 0;
            _playerAccount.ZeroModifierOnLose();
            arcadeUIHelper.ExitBattleScene();
        }

        private void OnEnable()
        {
            arcadeUIHelper.onStatChange += StatChange;
            arcadeUIHelper.onLoseClick += LoseBattle;
            arcadeUIHelper.onWinClick += WinBattle;
            arcadeUIHelper.OnEndBattle += LoseBattle;
        }

        private void StatChange(int key)
        {
            switch (key)
            {
                case 1:
                    _playerAccount.AttackStatUp(attackByLevel);
                    break;
                case 2:
                    _playerAccount.HealthStatUp(healthByLevel);
                    break;
                case 3:
                    _playerAccount.SpeedStatUp(_mainGameService.ArcadeCurrentScore % whenUpSpeed == 0
                        ? 0
                        : speedByLevel);
                    break;
            }
        }

        #endregion

        private void Awake()
        {
            InitializeBattlefield();
        }

        private void InitializeBattlefield()
        {
            _stepChecker = GetComponent<Step>();
            _isTypeStandart = false;
            _isTypeStandart = _stepChecker is StandartTypeStep;
            _sequenceMoves = new SequenceMoves(player1.GetComponent<Player>(), player2.GetComponent<Player>());
        }

        private void Start()
        {
            isOnWinScreen = false;
            loserName = "";
            if (gameField == null)
            {
                Debug.Log("No game field object");
                return;
            }

            var tileStackScript = gameField.GetComponent<TileStack>();
            tileStackScript.SetupArcadeGameService(this);
            tileStackScript.onTileChoose += TileChoose;
            tileStackScript.SetupPlayer();
            var fieldScript = gameField.GetComponent<CreateField>();
            if (fieldScript == null)
            {
                Debug.Log("No field script");
                return;
            }

            if (fight == null)
            {
                Debug.Log("No fight script");
                return;
            }

            if (inputService == null)
            {
                Debug.Log("No input service");
                return;
            }

            if (aiHandler == null)
            {
                Debug.Log("No AI Handler object");
                return;
            }

            aiHandler.SetupAIHandler(this, inputService);
            fight.SetUpPlayers(player1.GetComponent<Player>(), player2.GetComponent<Player>());
            fieldScript.SetupArcadeGameService(this);
            fieldScript.SetupField(false, fieldSize, _mainGameService.GetCurrentColorSettings());
            currentPlayerName = player1.GetComponent<Player>()?.GetPlayerName;
            currentPlayer = player1.GetComponent<Player>();
            currentPlayer.UpdateStats();

            inputService.onFingerUp += InputFingerUp;
            fight.onGameOver += GameOver;
            fight.onFightEnd += FightEnd;

            currentTurn = 1;
            currentStepInTurn = 1;
            tileStackScript.SpeedPlayer = currentPlayer.moveSpeedValue;
            if (_saveManager.CheckForSavedData() && _mainGameService.IsBattleActive) _saveManager.LoadGame();
            _mainGameService.ChangeActiveBattle(true);
            if (isOnWinScreen)
                GameOver(player1.GetComponent<Player>().GetPlayerName == loserName
                    ? player1.GetComponent<Player>()
                    : player2.GetComponent<Player>());
        }

        public void UpdateCurrentPlayerSpeed()
        {
            gameField.GetComponent<TileStack>().SpeedPlayer = currentPlayer.moveSpeedValue;
            Debug.Log("УстановкаТекСкорости" + currentPlayer.moveSpeedValue + " " + gameField.GetComponent<TileStack>().SpeedPlayer);
        }

        private void InputFingerUp()
        {
            var tileStack = gameField.GetComponent<TileStack>();
            var list = tileStack.GetCurrentPlayerTileList();
            if (list.Count < currentPlayer.moveSpeedValue) return;
            tileStack.ConfirmTiles();
            ButtonPressed();
            Debug.Log("Finger up event invoked");
        }

        private void FightEnd()
        {
            var fieldScript = gameField.GetComponent<CreateField>();
            fieldScript.RefreshField();
            _saveManager.SaveGame();
            _boostHandler.BoostUpdate();
        }

        private void GameOver(Player player)
        {
            onBattleEnd?.Invoke();
            isOnWinScreen = true;
            loserName = player.GetPlayerName;
            _saveManager.SaveGame();
            if (player.GetPlayerName == player1.GetComponent<Player>().GetPlayerName)
            {
                //TODO - rewards
                arcadeUIHelper.ShowMatchResult(false, _mainGameService.ArcadeCurrentScore,
                    _mainGameService.ArcadeBestScore, 0, 0, 0, 0);
            }
            else
            {
                ChangeCurrentScoreOnWin();
                if (_mainGameService.ArcadeCurrentScore % expToLevel == 0)
                {
                    _mainGameService.ArcadePlayerLevel++;
                    arcadeUIHelper.ShowLevelUp(_mainGameService.ArcadePlayerLevel, attackByLevel, healthByLevel,
                        _mainGameService.ArcadeCurrentScore % whenUpSpeed == 0 ? 0 : speedByLevel);
                }

                arcadeUIHelper.ShowMatchResult(true, _mainGameService.ArcadeCurrentScore,
                    _mainGameService.ArcadeBestScore, 0, 0, 0, 0);
                //SceneManager.LoadScene("EnemySelectionScene");
            }
        }


        private void TileChoose(Tile tile)
        {
            statsCollector.Add(tile);
            increaseStats.Player = currentPlayer;
            increaseStats.Increase();
            currentPlayer.UpdateStats();
            tile.ChangeTileType(emptyTile);
            Debug.Log($"Stats to {currentPlayer} suppose to raise");
        }

        private void ButtonPressed()
        {
            ChangePlayerTurn();

            if (currentStepInTurn == 1)
            {
                return;
            }
            else
            {
                if (gameField.GetComponent<TileStack>().NextMoveTiles.Count == 0)
                {
                    Debug.LogWarning("At the start of the turn there are no tiles to move, the field MUST be reloaded");
                }
                else
                {
                    PathCheck(gameField.GetComponent<TileStack>().NextMoveTiles);
                }
            }
        }

        private void ChangePlayerTurn()
        {
            var player1Name = player1.GetComponent<Player>().GetPlayerName;
            var player2Name = player2.GetComponent<Player>().GetPlayerName;

            if (currentPlayerName == player1Name)
            {
                currentPlayer = player2.GetComponent<Player>();
                currentPlayerName = player2Name;
            }
            else if (currentPlayerName == player2Name)
            {
                currentPlayer = player1.GetComponent<Player>();
                currentPlayerName = player1Name;
                currentStepInTurn++;

                Debug.Log($"Current step in turn {currentStepInTurn.ToString()}");
            }

            gameField.GetComponent<TileStack>().SpeedPlayer = currentPlayer.moveSpeedValue;
            _stepChecker.GetVariables(currentStepInTurn, stepsInTurn);
            if (_stepChecker.MoveIsPassed() == false) return;
            Debug.Log("Round is over => Fight!!!");
            if (_isTypeStandart)
            {
                fight.FightStandart();
            }
            else
            {
                fight.FightSimple(_sequenceMoves.CurrentPlayer, _sequenceMoves.NextPlayer);
            }

            _sequenceMoves.Next();
            //Kirill Add for AI
            onPlayerChange?.Invoke();
        }

        #region Checking the possibility of movement

        public void PathCheck(List<GameObject> listTilesGO)
        {
            bool hasPath = false;
            foreach (var tileGO in listTilesGO)
            {
                hasPath = pathCheck.FindPath(tileGO, currentPlayer.moveSpeedValue);
                if (hasPath == true)
                {
                    break;
                }
            }

            Debug.LogWarning(hasPath ? "We have a way(s), let's go!" : "We have no way, the field MUST be reloaded!");
        }

        #endregion

        #region EndBattle functions

        private void ChangeCurrentScoreOnWin()
        {
            var currentScore = _mainGameService.ArcadeCurrentScore;
            currentScore++;
            if (currentScore > _mainGameService.ArcadeBestScore)
            {
                _mainGameService.ArcadeBestScore = currentScore;
            }

            _mainGameService.ArcadeCurrentScore = currentScore;
        }

        #endregion

        public void LoadData(GameData gameData, bool newGameBattle, bool firstStart)
        {
            currentStepInTurn = gameData.CurrentStep;
            loserName = gameData.LoserName;
            isOnWinScreen = gameData.IsEndScreen;
        }

        private void SaveBattleStatDataUpdate(GameData gameData, BattleStatsData enemyBSD)
        {
            foreach (var bsd in gameData.BattleStatsData.Where(bsd => bsd.Name == "Enemy"))
            {
                bsd.Shield = enemyBSD.Shield;
                bsd.CurrentDamage = enemyBSD.CurrentDamage;
                bsd.CurrentHealth = enemyBSD.CurrentHealth;
                bsd.CurrentSpeed = enemyBSD.CurrentSpeed;
            }
        }

        public void SaveData(ref GameData gameData, bool newGameBattle, bool firstStart)
        {
            var enemy = player2.GetComponent<Player>();
            var flag = false;
            var enemyBSD = new BattleStatsData
            {
                CurrentDamage = enemy.AttackValue,
                CurrentHealth = enemy.HealthValue,
                CurrentSpeed = enemy.moveSpeedValue,
                Shield = enemy.Shielded
            };
            foreach (var bsd in gameData.BattleStatsData.Where(bsd => bsd.Name == "Enemy"))
            {
                flag = true;
            }

            if (flag) SaveBattleStatDataUpdate(gameData, enemyBSD);
            else
            {
                gameData.BattleStatsData.Add(enemyBSD);
            }

            gameData.CurrentStep = currentStepInTurn;
            gameData.LoserName = loserName;
            gameData.IsEndScreen = isOnWinScreen;
        }
    }
}