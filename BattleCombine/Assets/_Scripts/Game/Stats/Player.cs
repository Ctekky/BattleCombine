using System.Linq;
using BattleCombine.Data;
using BattleCombine.Interfaces;
using _Scripts.Temp;
using BattleCombine.Services;
using UnityEngine;
using Zenject;

namespace BattleCombine.Gameplay
{
    public class Player : Character, ISaveLoad
    {
        [SerializeField] private PlayerUI playerUIScript;
        [SerializeField] private string playerName;
        [SerializeField] private int avatarID;
        [Inject] private MainGameService _mainGameService;
        public string GetPlayerName => playerName;
        public int AvatarID => avatarID;

        protected override void Start()
        {
            base.Start();
            playerUIScript.SetUpAllStats(AttackValue.ToString(), HealthValue.ToString(), Shielded);
        }

        public void UpdateStats()
        {
            playerUIScript.SetUpAllStats(AttackValue.ToString(), HealthValue.ToString(), Shielded);
        }
        
        public void SetupAvatar(EnemyAvatarStruct avatarStruct, int id)
        {
            playerUIScript.SetupAvatar(avatarStruct.enableSprite, avatarStruct.disableSprite);
            avatarID = id;
        }

        public EnemyAvatarStruct GetAvatar()
        {
            var avatarStruct = new EnemyAvatarStruct
            {
                enableSprite = playerUIScript.GetSprite(true),
                disableSprite = playerUIScript.GetSprite(false),
                ID = avatarID
                
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

        public void SetDefaultStats(int attack, int health, int speed, bool shielded)
        {
            SetupDefaultStats(attack, health, speed, shielded);
            UpdateStats();
            playerUIScript.SetupSpeed(speed);
        }

        public void LoadData(GameData gameData, bool newGameBattle, bool firstStart)
        {
            /*
            var gdBs = gameData.BattleStatsData;
            foreach (var bsd in gdBs.Where(bsd => bsd.Name == playerName))
            {
                if (gameData.IsBattleActive)
                {
                    HealthValue = bsd.CurrentHealth;
                    AttackValue = bsd.CurrentDamage;
                    moveSpeedValue = bsd.CurrentSpeed;
                    Shielded = bsd.Shield;
                    playerName = bsd.Name;
                }
                else
                {
                    HealthValue = bsd.HealthDefault + bsd.HealthModifier;
                    AttackValue = bsd.DamageDefault + bsd.DamageModifier;
                    moveSpeedValue = bsd.SpeedDefault + bsd.SpeedModifier;
                    Shielded = bsd.Shield;
                    playerName = bsd.Name;
                }
            }
            */
        }

        public void SaveData(ref GameData gameData, bool newGameBattle, bool firstStart)
        {
            if(_mainGameService.IsEnemySelectionScene) return;
            var gdBs = gameData.BattleStatsData;
            var bsd = new BattleStatsData()
            {
                Name = playerName,
                CurrentHealth = HealthValue,
                HealthModifier = HealthValueModifier,
                HealthDefault = HealthValueDefault,
                CurrentDamage = AttackValue,
                DamageModifier = DamageValueModifier,
                DamageDefault = DamageValueDefault,
                CurrentSpeed = moveSpeedValue,
                SpeedModifier = SpeedValueModifier,
                SpeedDefault = SpeedValueDefault,
                Shield = Shielded
            };
            gdBs.Add(bsd);
        }
    }
}