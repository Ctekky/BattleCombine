using System.Linq;
using BattleCombine.Data;
using BattleCombine.Interfaces;
using BattleCombine.Services.Other;
using UnityEngine;
using Zenject;

namespace BattleCombine.Services
{
    public class MainGameService : MonoBehaviour, ISaveLoad
    {
        #region ArcadeGameStats

        [SerializeField] private int arcadeCurrentScore;
        [SerializeField] private int arcadeBestScore;
        [SerializeField] private int arcadePlayerLevel;

        public int ArcadeCurrentScore
        {
            get => arcadeCurrentScore;
            set => arcadeCurrentScore = value;
        }

        public int ArcadeBestScore
        {
            get => arcadeBestScore;
            set => arcadeBestScore = value;
        }

        public int ArcadePlayerLevel
        {
            get => arcadePlayerLevel;
            set => arcadePlayerLevel = value;
        }

        #endregion

        [SerializeField] private ColorSettings currentColorSettings;
        [SerializeField] private bool isBattleActive;
        [SerializeField] private bool isEnemySelectionScene;
        [Inject] private ResourceService _resourceService;

        #region MyRegion EnemyData

        [SerializeField] private int enemyAttack;
        [SerializeField] private int enemyHealth;
        [SerializeField] private int enemySpeed;
        [SerializeField] private bool enemyShielded;
        [SerializeField] private Sprite enemyAvatarEnable;
        [SerializeField] private Sprite enemyAvatarDisable;
        [SerializeField] private int enemyAvatarID;
        

        public int EnemyAttack => enemyAttack;
        public int EnemyHealth => enemyHealth;
        public int EnemySpeed => enemySpeed;
        public bool EnemyShielded => enemyShielded;
        public Sprite EnemyAvatarEnable => enemyAvatarEnable;
        public Sprite EnemyAvatarDisable => enemyAvatarDisable;
        public int EnemyAvatarID => enemyAvatarID;

        #endregion

        private void Awake()
        {
            isBattleActive = false;
        }

        public ColorSettings GetCurrentColorSettings()
        {
            return currentColorSettings;
        }

        public bool IsBattleActive => isBattleActive;
        public bool IsEnemySelectionScene
        {
            get => isEnemySelectionScene;
            set => isEnemySelectionScene = value;
        }

        public void ChangeActiveBattle(bool flag)
        {
            isBattleActive = flag;
        }

        public void SaveEnemy(int attack, int health, int speed, bool shield, Sprite avatarEnable, Sprite avatarDisable, int avatarID)
        {
            enemyAttack = attack;
            enemyHealth = health;
            enemySpeed = speed;
            enemyShielded = shield;
            enemyAvatarEnable = avatarEnable;
            enemyAvatarDisable = avatarDisable;
            enemyAvatarID = avatarID;
        }

        private void GetAvatarFromDB(int key)
        {
            foreach (var avatar in _resourceService.GetAvatarDB.avatarList.Where(avatar => key == avatar.ID))
            {
                enemyAvatarEnable = avatar.enableSprite;
                enemyAvatarDisable = avatar.disableSprite;
            }
        }

        public void LoadData(GameData gameData, bool newGameBattle, bool firstStart)
        {
            arcadeCurrentScore = gameData.PlayerAccountData.CurrentScore;
            ArcadeBestScore = gameData.PlayerAccountData.MaxScore;
            isBattleActive = gameData.IsBattleActive;
            isEnemySelectionScene = gameData.IsEnemySelectionScene;
            arcadePlayerLevel = gameData.ArcadePlayerLevel;
            foreach (var bsd in gameData.BattleStatsData.Where(bsd => bsd.Name == "Enemy"))
            {
                enemyAttack = bsd.CurrentDamage;
                enemyHealth = bsd.CurrentHealth;
                enemySpeed = bsd.CurrentSpeed;
                enemyShielded = bsd.Shield;
                GetAvatarFromDB(gameData.EnemyAvatarID);
                enemyAvatarID = gameData.EnemyAvatarID;
            }
        }

        public void SaveData(ref GameData gameData, bool newGameBattle, bool firstStart)
        {
            gameData.PlayerAccountData.CurrentScore = arcadeCurrentScore;
            gameData.PlayerAccountData.MaxScore = arcadeBestScore;
            gameData.EnemyAvatarID = enemyAvatarID;
            gameData.IsBattleActive = isBattleActive;
            gameData.IsEnemySelectionScene = isEnemySelectionScene;
            gameData.ArcadePlayerLevel = arcadePlayerLevel;
        }
    }
}