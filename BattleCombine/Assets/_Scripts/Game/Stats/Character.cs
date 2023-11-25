using UnityEngine;

namespace BattleCombine.Gameplay
{
    public abstract class Character : MonoBehaviour
    {
        [SerializeField] private Stats stats;
        public int moveSpeedValue;

        public bool Shielded
        {
            get => stats.shielded;
            set => stats.shielded = value;
        }
        public int AttackValue
        {
            get => stats.attackValue;
            set => stats.attackValue = value;
        }
        public int HealthValue
        {
            get => stats.healthValue;
            set => stats.healthValue = value;
        }
   
        
        protected virtual void Start()
        {
            StartGame();
        }

        private void Awake()
        {
            stats.shielded = false;
            stats.attackValue = stats.attackValueDefault;
            stats.healthValue = stats.healthValueDefault;
            moveSpeedValue = stats.moveSpeedValueDefault;
        }

        private void StartGame()
        {
            NextMove();
        }

        public void ChangeHealth(int addHealth)
        {
            stats.healthValue = AddValue(stats.healthValue, addHealth);
        }

        public void AddAttack(int addAttack)
        {
            stats.attackValue = AddValue(stats.attackValue, addAttack);
        }

        public void SetAttackDefault()
        {
            stats.attackValue = stats.attackValueDefault;
        }

        private int AddValue(int stat, int value)
        {
            return stat + value;
            //return tmp <= 0 ? 0 : tmp;
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