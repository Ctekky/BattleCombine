using System;
using System.Collections.Generic;
using BattleCombine.Ai;
using BattleCombine.Enums;
using BattleCombine.Gameplay;
using BattleCombine.ScriptableObjects;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using Zenject;

namespace BattleCombine.Services
{
    public class ArcadeGameService : MonoBehaviour
    {
        #region Dependency

        //TODO: change gameService on INJECT obj
        [SerializeField] private GameService _gameService;
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

        [SerializeField] private int currentStepInTurn;
        [SerializeField] private int currentTurn;
        [SerializeField] private int stepsInTurn;

        private PathCheck pathCheck = new PathCheck();
        private bool _isTypeStandart;
        private int _maxPossibleMove;
        private GameObject _previousTile;
        private GameObject _currentTile;
        private bool _canMove = false;
        private bool _isFirstRun = true;

        //todo - (temp) Kirill Add to control ai hp status (to change state)
        public int GetPlayerAiHealth => player2.GetComponent<Player>().HealthValue;

        public int GetCurrentStepInTurn => currentStepInTurn;

        #region System functions

        private void OnDisable()
        {
            inputService.onFingerUp -= InputFingerUp;
        }

        #endregion


        private void Awake()
        {
            _stepChecker = GetComponent<Step>();
            _isTypeStandart = false;
            _isTypeStandart = _stepChecker is StandartTypeStep;
            _sequenceMoves = new SequenceMoves(player1.GetComponent<Player>(), player2.GetComponent<Player>());
        }

        private void Start()
        {
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
            fieldScript.SetupField(false, fieldSize, _gameService.GetCurrentColorSettings());
            currentPlayerName = player1.GetComponent<Player>()?.GetPlayerName;
            currentPlayer = player1.GetComponent<Player>();
            currentPlayer.UpdateStats();

            inputService.onFingerUp += InputFingerUp;
            fight.onGameOver += GameOver;
            fight.onFightEnd += FightEnd;

            currentTurn = 1;
            currentStepInTurn = 1;
            tileStackScript.SpeedPlayer = currentPlayer.moveSpeedValue;
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
        }

        private void GameOver(Player player)
        {
            if (player.GetPlayerName == player1.GetComponent<Player>().GetPlayerName)
            {
                Debug.Log("Game over");
                SceneManager.LoadScene("Initial");
            }
            else
            {
                Debug.Log("Next battle");
                SceneManager.LoadScene("EnemySelectionScene");
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
            ;
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

            if (hasPath == true)
            {
                Debug.LogWarning("We have a way(s), let's go!");
            }
            else
            {
                Debug.LogWarning("We have no way, the field MUST be reloaded!");
            }

        }
        #endregion

        #region EndBattle functions

        private void ChangeCurrentScoreOnWin()
        {
            var currentScore = _gameService.ArcadeCurrentScore++;
            if (currentScore > _gameService.ArcadeBestScore)
            {
                _gameService.ArcadeBestScore = currentScore;
            }

            _gameService.ArcadeCurrentScore = currentScore;
        }

        #endregion
    }
}