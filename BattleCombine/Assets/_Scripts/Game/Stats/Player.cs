using BattleCombine.Data;
using BattleCombine.Interfaces;
using UnityEngine;

namespace BattleCombine.Gameplay
{
    public class Player : Character, ISaveLoad
    {
        [SerializeField] private PlayerUI playerUIScript;
        [SerializeField] private string playerName;
        public string GetPlayerName => playerName;

        protected override void Start()
        {
            base.Start();
            playerUIScript.SetUpAllStats(AttackValue.ToString(), HealthValue.ToString(), Shielded);
        }

        public void UpdateStats()
        {
            playerUIScript.SetUpAllStats(AttackValue.ToString(), HealthValue.ToString(), Shielded);
        }

        public void LoadData(GameDataNew gameDataNew)
        {
            int i = 1;
            if (playerName == "Player2")
            {
                i = 0;
            }
            var gdBs = gameDataNew.battleStats;
            playerName = gdBs[i].Name;
            HealthValue = gdBs[i].Health;
            AttackValue = gdBs[i].Damage;
            moveSpeedValue = gdBs[i].Speed;
            Shielded = gdBs[i].Shield;
        }

        public void SaveData(ref GameDataNew gameDataNew)
        {
            int i = 1;
            if (playerName == "Player2")
            {
                i = 0;
            }
            var gdBs = gameDataNew.battleStats;
            gdBs[i].Name = playerName;
            gdBs[i].Health = HealthValue;
            gdBs[i].Damage = AttackValue;
            gdBs[i].Speed = moveSpeedValue;
            gdBs[i].Shield = Shielded;
        }
    }
}