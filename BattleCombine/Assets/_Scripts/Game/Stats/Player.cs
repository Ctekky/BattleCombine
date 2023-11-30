using BattleCombine.Data;
using BattleCombine.Interfaces;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

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

        public void LoadData(GameData gameData, bool newGameBattle, bool firstStart)
        {
            var gdBs = gameData.battleStatsData;

            int i = 1;
            if (playerName == "Player2")
            {
                i = 0;
            }
            if (newGameBattle == true)
            {
                HealthValue = HealthValueDefault + gdBs[i].HealthModifier;
                AttackValue = DamageValueDefault + gdBs[i].DamageModifier;
                moveSpeedValue = SpeedValueDefault + gdBs[i].SpeedModifier;
            }
            else
            {
                HealthValue = gdBs[i].CurrentHealth;
                AttackValue = gdBs[i].CurrentDamage;
                moveSpeedValue = gdBs[i].CurrentSpeed;
            }
            playerName = gdBs[i].Name;
            Shielded = gdBs[i].Shield;
        }

        public void SaveData(ref GameData gameData, bool newGameBattle, bool firstStart)
        {
            var gdBs = gameData.battleStatsData;

            int i = 1;
            if (playerName == "Player2")
            {
                i = 0;
            }
            gdBs[i].Name = playerName;
            gdBs[i].CurrentHealth = HealthValue;
            gdBs[i].HealthModifier = HealthValueModifier;
            gdBs[i].HealthDefault = HealthValueDefault;
            gdBs[i].CurrentDamage = AttackValue;
            gdBs[i].DamageModifier = DamageValueModifier;
            gdBs[i].DamageDefault = DamageValueDefault;
            gdBs[i].CurrentSpeed = moveSpeedValue;
            gdBs[i].SpeedModifier = SpeedValueModifier;
            gdBs[i].Shield = Shielded;
        }
    }
}