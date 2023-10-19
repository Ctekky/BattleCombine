using System;
using System.Collections.Generic;
using System.Linq;
using BattleCombine.Enums;
using BattleCombine.ScriptableObjects;
using BattleCombine.Services.InputService;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BattleCombine.Gameplay
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private InputService inputService;
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

        private Step stepChecker;
        private SequenceMoves sequenceMoves;
        private bool isTypeStandart;


        public int GetCurrentStepInTurn
        {
            get => currentStepInTurn;
        }

        public InputMod GetInputMode
        {
            get => inputMode;
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
            if (gameField == null)
            {
                Debug.Log("No gamefield object");
                return;
            }

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

            inputService.onFingerUp += InputFingerUp;
            fight.SetUpPlayers(player1.GetComponent<Player>(), player2.GetComponent<Player>());

            fight.onGameOver += GameOver;
            fight.onFightEnd += FightEnd;
            _currentPlayerName = player1.GetComponent<Player>()?.GetPlayerName;
            _currentPlayer = player1.GetComponent<Player>();
            gameField.GetComponent<CreateField>().SetupGameManager(this);
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
            description.text = _currentPlayerName + " turn " + currentTurn + " step " + currentStepInTurn;
        }

        private void InputFingerUp()
        {
            Debug.Log("Finger up event invoked");
        }

        private void FightEnd()
        {
            var fieldScript = gameField.GetComponent<CreateField>();
            fieldScript.RefreshField();
        }

        private void GameOver()
        {
            Debug.Log("Battle is over");
            SceneManager.LoadScene(sceneName);
        }

        private void ButtonPressed()
        {
            var tileStack = GetTileList();
            foreach (var tileComponent in tileStack.Select(tile => tile.GetComponent<Tile>()))
            {
                statsCollector.Add(tileComponent);
                tileComponent.StateMachine.ChangeState(tileComponent.FinalChoiceState);
            }

            increaseStats.Player = _currentPlayer;
            increaseStats.Increase();
            _currentPlayer.UpdateStats();
            //foreach (var tileComponent in tileStack.Select(tile => tile.GetComponent<Tile>()))
            //{
            //    tileComponent.ChangeTileType(emptyTile);
            //}

            Debug.Log($"Stats to {_currentPlayer} suppose to raise");
            ChangePlayerTurn();
        }

        private List<GameObject> GetTileList()
        {
            var tileStack = gameField.GetComponent<TileStack>();
            return tileStack.IDPlayer switch
            {
                IDPlayer.Player1 => tileStack.TilesListPlayer1,
                IDPlayer.Player2 => tileStack.TilesListPlayer2,
                IDPlayer.AIPlayer => tileStack.TilesListPlayer2,
                _ => null
            };
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

            description.text = _currentPlayerName + " turn " + currentTurn + " step " + currentStepInTurn;
            // if (currentStepInTurn <= stepsInTurn) return;  
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
        }

        public void SpeedIsOver(bool state)
        {
            nextTurnButton.IsTouchable = state;
        }
    }
}