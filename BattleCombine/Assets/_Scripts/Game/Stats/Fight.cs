using System;
using UnityEngine;

namespace BattleCombine.Gameplay
{
    public class Fight : MonoBehaviour
    {
        private Player _player1;
        private Player _player2;

        public Action onGameOver;
        public Action onFighting;

        public void SetUpPlayers(Player player1, Player player2)
        {
            _player1 = player1;
            _player2 = player2;
        }

        public void Fighting()
        {
            if (_gameOver)
            {
                onGameOver?.Invoke();
                return;
            }

            TakeDamage(_player1, _player2);
            if (_gameOver)
            {
                onGameOver?.Invoke();
                return;
            }

            TakeDamage(_player2, _player1);
            if (_gameOver)
            {
                onGameOver?.Invoke();
                return;
            }

            onFighting?.Invoke();
        }

        private bool _gameOver;

        private void TakeDamage(Player player1, Player player2)
        {
            switch (player1.Shielded)
            {
                case true:
                    player1.ChangeHealth(-(player2.AttackValue / 2));
                    player1.RemoveShield();
                    break;
                case false:
                    player1.ChangeHealth(-(player2.AttackValue));
                    break;
            }

            if (player1.HealthValue > 0) return;
            print($"{player1.GetPlayerName} lose");
            _gameOver = true;
            //player1.StartGame();
        }
    }
}