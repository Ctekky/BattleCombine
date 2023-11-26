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