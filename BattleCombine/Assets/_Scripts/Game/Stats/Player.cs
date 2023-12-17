using BattleCombine.Data;
using BattleCombine.Interfaces;
using _Scripts.Temp;
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
        
        public void SetupAvatar(EnemyAvatarStruct avatarStruct)
        {
            playerUIScript.SetupAvatar(avatarStruct.enableSprite, avatarStruct.disableSprite);
        }

        public EnemyAvatarStruct GetAvatar()
        {
            var avatarStruct = new EnemyAvatarStruct
            {
                enableSprite = playerUIScript.GetSprite(true),
                disableSprite = playerUIScript.GetSprite(false)
            };
            return avatarStruct;
        }

        public void ChangeAvatarState(bool state)
        {
            playerUIScript.ChangeAvatarState(state);
        }

        public void SetStats(int attack, int health, int speed, bool shielded)
        {
            SetupStats(attack, health, speed, shielded);
            UpdateStats();
            playerUIScript.SetupSpeed(speed);
        }

        public void LoadData(GameData gameData, bool newGameBattle, bool firstStart)
        {
            var gdBs = gameData.battleStatsData;

            int i = 0;
            if (playerName == "Player2")
            {
                i = 1;
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

            int i = 0;
            if (playerName == "Player2")
            {
                i = 1;
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