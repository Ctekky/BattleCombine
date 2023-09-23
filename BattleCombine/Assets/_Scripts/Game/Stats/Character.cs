using UnityEngine;

namespace BattleCombine.Gameplay
{
    public abstract class Character : MonoBehaviour
    {
        [SerializeField] private Stats stats;
        public int moveSpeedValue;

        public bool Shielded => stats.shielded;
        public int AttackValue => stats.attackValue;
        public int HealthValue => stats.healthValue;


        protected virtual void Start()
        {
            StartGame();
        }

        private void StartGame()
        {
            NextMove();
            stats.shielded = false;
            stats.attackValue = stats.attackValueDefault;
            stats.healthValue = stats.healthValueDefault;
        }

        public void ChangeHealth(int addHealth)
        {
            stats.healthValue = AddValue(stats.healthValue, addHealth);
            /*
            int tmp = stats.healthValue + addHealth;

            if (tmp > stats.healthValueDefault)
            {
                stats.healthValue = stats.healthValueDefault;
            }
            else if (tmp <= 0)
            {
                stats.healthValue = 0;
            }
            else
            {
                stats.healthValue = tmp;
            }
            */
        }

        public void AddAttack(int addAttack)
        {
            stats.attackValue = AddValue(stats.attackValue, addAttack);
            /*
            int tmp = stats.attackValue + addAttack;

            if (tmp > stats.attackValueDefault)
            {
                stats.attackValue = stats.attackValueDefault;
            }
            else if (tmp <= 0)
            {
                stats.attackValue = 0;
            }
            else
            {
                stats.attackValue = tmp;
            }
            */
        }

        private int AddValue(int stat, int value)
        {
            var tmp = stat + value;
            return tmp <= 0 ? 0 : tmp;
        }

        public void AddShield()
        {
            stats.shielded = true;
        }

        public void RemoveShield()
        {
            stats.shielded = false;
        }

        public void MakeMove(int move)
        {
            moveSpeedValue -= move;
        }

        public bool CheckMove()
        {
            return moveSpeedValue != 0;
        }

        public void NextMove()
        {
            moveSpeedValue = stats.moveSpeedValueDefault;
        }
    }
}