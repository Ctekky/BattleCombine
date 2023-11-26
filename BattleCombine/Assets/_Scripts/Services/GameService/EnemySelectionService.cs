using BattleCombine.Gameplay;
using BattleCombine.UI;
using UnityEngine;

namespace BattleCombine.Services
{
    public class EnemySelectionService : MonoBehaviour
    {
        [SerializeField] private ChooseEnemy choseEnemyScript;
        [SerializeField] private UIUpdateStats enemyUIUpdateStats;
        [SerializeField] private UIChosenEnemy enemyUIChosenScript;
        [SerializeField] private Player currentChosenEnemy;
        [SerializeField] private int currentScore; //came from GameService

        private void OnEnable()
        {
            enemyUIChosenScript.onEnemyClick += ChooseEnemy;
        }

        private void OnDisable()
        {
            enemyUIChosenScript.onEnemyClick -= ChooseEnemy;
        }

        private void Start()
        {
            choseEnemyScript.CalculateEnemies(currentScore);
            enemyUIUpdateStats.UpdateEnemiesStats(choseEnemyScript.GetFinalEnemies());
            enemyUIUpdateStats.UpdatePlayerStats(3, 25, 3, false);
            enemyUIUpdateStats.UpdateEnemiesAvatars(choseEnemyScript.GetFinalAvatars());
        }

        private void ChooseEnemy(Player player)
        {
            enemyUIUpdateStats.UpdateConfirmPanelEnemy(player);
        }
    }
}