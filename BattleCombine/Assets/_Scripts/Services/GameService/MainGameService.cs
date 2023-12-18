using BattleCombine.Services.Other;
using UnityEngine;


namespace BattleCombine.Services
{
    public class MainGameService : MonoBehaviour
    {
        #region ArcadeGameStats

        [SerializeField] private int arcadeCurrentScore;
        [SerializeField] private int arcadeBestScore;

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

        #endregion
        
        [SerializeField] private ColorSettings currentColorSettings;
        
        [SerializeField] private int enemyAttack;
        [SerializeField] private int enemyHealth;
        [SerializeField] private int enemySpeed;
        [SerializeField] private bool enemyShielded;
        [SerializeField] private Sprite enemyAvatarEnable;
        [SerializeField] private Sprite enemyAvatarDisable;

        public int EnemyAttack => enemyAttack;
        public int EnemyHealth => enemyHealth;
        public int EnemySpeed => enemySpeed;
        public bool EnemyShielded => enemyShielded;
        public Sprite EnemyAvatarEnable => enemyAvatarEnable;
        public Sprite EnemyAvatarDisable => enemyAvatarDisable;

        public ColorSettings GetCurrentColorSettings()
        {
            return currentColorSettings;
        }

        public void SaveEnemy(int attack, int health, int speed, bool shield, Sprite avatarEnable, Sprite avatarDisable)
        {
            enemyAttack = attack;
            enemyHealth = health;
            enemySpeed = speed;
            enemyShielded = shield;
            enemyAvatarEnable = avatarEnable;
            enemyAvatarDisable = avatarDisable;
        }
    }
}