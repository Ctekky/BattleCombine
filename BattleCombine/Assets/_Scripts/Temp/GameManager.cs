using System;
using System.Collections.Generic;
using System.Linq;
using BattleCombine.Enums;
using BattleCombine.ScriptableObjects;
using BattleCombine.Services.InputService;
using BattleCombine.Services.Other;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BattleCombine.Gameplay
{
    public class GameManager : MonoBehaviour
    {
        //добавил, чтоб ловить боту момент смены хода
        public static Action OnPlayerChange;

        [SerializeField] private InputService inputService;
        [SerializeField] private ColorSettings colorSettings;
        [SerializeField] private GameObject player1;
        [SerializeField] private GameObject player2;
        [SerializeField] private GameObject gameField;
        [SerializeField] private InputMod inputMode;
        [SerializeField] private StatsCollector statsCollector;
        [SerializeField] private IncreaseStats increaseStats;
        [SerializeField] private NextTurnButton nextTurnButton;
        [SerializeField] private TileType emptyTile;
        [SerializeField] private Fight fight;

        [SerializeField] private TextMeshPro description;

        [SerializeField] public string _currentPlayerName;
        [SerializeField] private Player _currentPlayer;

        [SerializeField] private int currentStepInTurn;
        [SerializeField] private int currentTurn;

        [SerializeField] private string sceneName;

        [SerializeField] private int stepsInTurn;

        [SerializeField] private int currentBattleIndex;

        private Step stepChecker;
        private SequenceMoves sequenceMoves;
        private bool isTypeStandart;

        //todo - (temp) Kirill Add to control ai hp status (to change state)
        public int GetPlayerAiHealth => player2.GetComponent<Player>().HealthValue;
        
        public int GetCurrentStepInTurn
        {
            get => currentStepInTurn;
        }

        public InputMod GetInputMode
        {
            get => inputMode;
        }

        public Color GetTileColorSetting()
        {
            return colorSettings.tileColor;
        }
        
        public Color GetBorderColorSetting()
        {
            return colorSettings.borderColor;
        }

        private void OnDisable()
        {
            inputService.onFingerUp -= InputFingerUp;
        }

        private void Awake()
        {
            stepChecker = GetComponent<Step>();
            isTypeStandart = false;
            isTypeStandart = stepChecker is StandartTypeStep;
            sequenceMoves = new SequenceMoves(player1.GetComponent<Player>(), player2.GetComponent<Player>());
        }

        private void Start()
        {
            currentBattleIndex = 0;
            if (gameField == null)
            {
                Debug.Log("No gamefield object");
                return;
            }

            var fieldScript = gameField.GetComponent<CreateField>();
            var tileStackScript = gameField.GetComponent<TileStack>();
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

            inputService.onFingerUp += InputFingerUp;
            fight.SetUpPlayers(player1.GetComponent<Player>(), player2.GetComponent<Player>());

            fight.onGameOver += GameOver;
            fight.onFightEnd += FightEnd;
            tileStackScript.onTileChoose += TileChoose;
            _currentPlayerName = player1.GetComponent<Player>()?.GetPlayerName;
            _currentPlayer = player1.GetComponent<Player>();
            _currentPlayer.UpdateStats();
            gameField.GetComponent<CreateField>().SetupGameManager(this);
            NextBattle();
            if (nextTurnButton == null)
            {
                Debug.Log("No button object");
            }
            else
            {
                nextTurnButton.onButtonPressed += ButtonPressed;
            }

            currentTurn = 1;
            currentStepInTurn = 1;
            tileStackScript.SpeedPlayer = _currentPlayer.moveSpeedValue;
            description.text = _currentPlayerName + " turn " + currentTurn + " step " + currentStepInTurn;
        }

        private void InputFingerUp()
        {
            var tileStack = gameField.GetComponent<TileStack>();
            var list = tileStack.GetCurrentPlayerTileList();
            if(list.Count < _currentPlayer.moveSpeedValue) return;
            tileStack.ConfirmTiles();
            ButtonPressed();
            Debug.Log("Finger up event invoked");
            return;
            //TODO - Anton move this code if you need it
            if (list.Count == 1)
            {
                Tile tile = list[0].GetComponent<Tile>();
                if (tile.GetTileState == TileState.ChosenState)
                {
                    //tile.EndTouchOnTile();
                    Debug.Log("One tile in list, CHANGE STATE");
                }
                else
                {
                    Debug.Log("One tile in list");
                    return;
                }
            }
            else if (list.Count < _currentPlayer.moveSpeedValue)
            {
                Debug.Log("Need more tile for end move or one tile in list for change state");
                return;
            }
            else if (list.Count == _currentPlayer.moveSpeedValue)
            {
                tileStack.ConfirmTiles();
                ButtonPressed();
                Debug.Log("Finger up event invoked");
            }
        }

        private void FightEnd()
        {
            var fieldScript = gameField.GetComponent<CreateField>();
            fieldScript.RefreshField();
        }

        private void GameOver(Player player)
        {
            if(player.GetPlayerName == player1.GetComponent<Player>().GetPlayerName)
                Debug.Log("Game over");
            else
            {
                Debug.Log("Next battle");
                NextBattle();
            }
            //SceneManager.LoadScene(sceneName);
        }

        private void NextBattle()
        {
            currentBattleIndex++;
            //TODO: load lastbattle index from savefile
            gameField.GetComponent<CreateField>().SetupField(false, FieldSize.Large);
            //TODO: change AI Type behavior
            //TODO: calculate stats and get next AI type
        }

        private void TileChoose(Tile tile)
        {
            statsCollector.Add(tile);
            increaseStats.Player = _currentPlayer;
            increaseStats.Increase();
            _currentPlayer.UpdateStats();
            tile.ChangeTileType(emptyTile);
            Debug.Log($"Stats to {_currentPlayer} suppose to raise");
        }

        private void ButtonPressed()
        {
            ChangePlayerTurn();
        }

        private void ChangePlayerTurn()
        {
            var player1Name = player1.GetComponent<Player>().GetPlayerName;
            var player2Name = player2.GetComponent<Player>().GetPlayerName;

            if (_currentPlayerName == player1Name)
            {
                _currentPlayer = player2.GetComponent<Player>();
                _currentPlayerName = player2Name;
            }
            else if (_currentPlayerName == player2Name)
            {
                _currentPlayer = player1.GetComponent<Player>();
                _currentPlayerName = player1Name;
                currentStepInTurn++;

                Debug.Log($"Current step in turn {currentStepInTurn.ToString()}");
            }

            gameField.GetComponent<TileStack>().SpeedPlayer = _currentPlayer.moveSpeedValue;
            description.text = _currentPlayerName + " turn " + currentTurn + " step " + currentStepInTurn;
            stepChecker.GetVariables(currentStepInTurn, stepsInTurn);
            if (stepChecker.MoveIsPassed() == false) return;
            Debug.Log("Round is over => Fight!!!");
            if (isTypeStandart)
            {
                fight.FightStandart();
            }
            else
            {
                fight.FightSimple(sequenceMoves.CurrentPlayer, sequenceMoves.NextPlayer);
            }

            sequenceMoves.Next();
            //Kirill Add for AI
            OnPlayerChange?.Invoke();
        }

        public void SpeedIsOver(bool state)
        {
            nextTurnButton.IsTouchable = state;
        }
    }
}