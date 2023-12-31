using System;
using UnityEngine;

namespace BattleCombine.Gameplay
{
    public class Fight : MonoBehaviour
    {
        private Player _player1;
        private Player _player2;

        public Action<Player> onGameOver;
        public Action onFightEnd;


        private Step step;
        private bool isTypeStandart;
        private int playerCurrentStepSimple;

        private void Awake()
        {
            step = FindObjectOfType<Step>();
            isTypeStandart = step is StandartTypeStep;
        }

        public void SetUpPlayers(Player player1, Player player2)
        {
            _player1 = player1;
            _player2 = player2;
        }

        public void Fighting()
        {
            playerCurrentStepSimple++;

            if (_gameOver)
            {
                onGameOver?.Invoke(_player1);
                return;
            }

            if (isTypeStandart)
            {
                TakeDamage(_player1, _player2);
                if (_gameOver)
                {
                    onGameOver?.Invoke(_player1);
                    return;
                }

                TakeDamage(_player2, _player1);
                if (_gameOver)
                {
                    onGameOver?.Invoke(_player2);
                    return;
                }

                onFightEnd?.Invoke();
            }
            else if (playerCurrentStepSimple % 2 == 0)
            {
                onFightEnd?.Invoke();
            }


            //_player1.SetAttackDefault();
            // _player2.SetAttackDefault();
        }

        public void FightStandart()
        {
            if (_gameOver)
            {
                onGameOver?.Invoke(_player1);
                return;
            }
            //if (isTypeStandart)
            // {

            TakeDamage(_player1, _player2);
            if (_gameOver)
            {
                onGameOver?.Invoke(_player1);
                return;
            }

            TakeDamage(_player2, _player1);
            if (_gameOver)
            {
                onGameOver?.Invoke(_player2);
                return;
            }

            onFightEnd?.Invoke();

            _player1.SetAttackDefault();
            _player2.SetAttackDefault();

            // }
        }

        public void FightSimple(Player currentPlayer, Player nextPlayer)
        {
            playerCurrentStepSimple++;
            TakeDamage(nextPlayer, currentPlayer);
            currentPlayer.SetAttackDefault();
            if (_gameOver)
            {
                onGameOver?.Invoke(nextPlayer);
                return;
            }

            if (playerCurrentStepSimple % 2 == 0)
            {
                onFightEnd?.Invoke();
            }
        }

        private bool _gameOver;
        private bool _nextBattle;

        private void TakeDamage(Player defender, Player attacker)
        {
            var attack = attacker.AttackValue;
            if (attacker.AttackValue <= 0) attack = 0;
            {
                switch (defender.Shielded)
                {
                    case true:
                        defender.ChangeHealth(-(attack / 2));
                        defender.RemoveShield();
                        break;
                    case false:
                        defender.ChangeHealth(-(attack));
                        break;
                }
            }
            defender.UpdateStats();
            attacker.UpdateStats();
            if (defender.HealthValue > 0) return;
            print($"{defender.GetPlayerName} lose");
            _gameOver = true;
        }
    }
}