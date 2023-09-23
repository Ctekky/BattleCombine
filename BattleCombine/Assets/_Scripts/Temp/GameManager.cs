using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace BattleCombine.Gameplay
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private GameObject player1;
        [SerializeField] private GameObject player2;
        [SerializeField] private GameObject gameField;
        [SerializeField] private StatsCollector statsCollector;
        [SerializeField] private IncreaseStats increaseStats;
        [SerializeField] private NextTurnButton nextTurnButton;

        [SerializeField] public string _currentPlayerName;
        [SerializeField] private Player _currentPlayer;

        [SerializeField] private int currentStepInTurn;
        [SerializeField] private int currentTurn;

        [SerializeField] private int stepsInTurn;

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
        }

        private void ButtonPressed()
        {
            var tileStack = gameField.GetComponent<TileStack>().TilesStack;
            foreach (var tileComponent in tileStack.Select(tile => tile.GetComponent<Tile>()))
            {
                statsCollector.Add(tileComponent);
                tileComponent.StateMachine.ChangeState(tileComponent.FinalChoiceState);
            }

            increaseStats.Player = _currentPlayer;
            increaseStats.Increase();
            _currentPlayer.UpdateStats();
            Debug.Log($"Stats to {_currentPlayer} suppose to raise");
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

            if (currentStepInTurn > stepsInTurn)
            {
                Debug.Log("Round is over => Fight!!!");
            }
        }

        public void SpeedIsOver(bool state)
        {
            nextTurnButton.IsTouchable = state;
        }
    }
}