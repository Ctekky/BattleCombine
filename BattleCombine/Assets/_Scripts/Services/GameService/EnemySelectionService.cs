using _Scripts.UI;
using BattleCombine.Gameplay;
using BattleCombine.UI;
using UnityEngine;
using Zenject;

namespace BattleCombine.Services
{
    public class EnemySelectionService : MonoBehaviour
    {
        [SerializeField] private ChooseEnemy choseEnemyScript;
        [SerializeField] private UIUpdateStats enemyUIUpdateStats;
        [SerializeField] private UIChosenEnemy enemyUIChosenScript;
        [SerializeField] private TemporaryEnemySelectUIHelper tempEnemySelection;
        [SerializeField] private Player currentChosenEnemy;
        [SerializeField] private Player currentPlayer;
        [Inject] private MainGameService _mainGameService;
        [SerializeField] private int currentScore; //came from GameService

        private void OnEnable()
        {
            enemyUIChosenScript.onEnemyClick += ChooseEnemy;
            tempEnemySelection.onBattleButtonClick += BattleButtonClick;
        }

        private void OnDisable()
        {
            enemyUIChosenScript.onEnemyClick -= ChooseEnemy;
            tempEnemySelection.onBattleButtonClick -= BattleButtonClick;
        }

        private void Start()
        {
            currentScore = _mainGameService.ArcadeCurrentScore;
            choseEnemyScript.CalculateEnemies(currentScore);
            enemyUIUpdateStats.UpdateEnemiesStats(choseEnemyScript.GetFinalEnemies());
            enemyUIUpdateStats.UpdatePlayerStats(3, 25, 3, false);
            enemyUIUpdateStats.UpdateEnemiesAvatars(choseEnemyScript.GetFinalAvatars());
        }

        private void ChooseEnemy(Player player)
        {
            currentChosenEnemy = player;
            enemyUIUpdateStats.UpdateConfirmPanelEnemy(player);
        }

        private void BattleButtonClick()
        {
            _mainGameService.SaveEnemy(currentChosenEnemy.AttackValue, currentChosenEnemy.HealthValue,
                currentChosenEnemy.moveSpeedValue, currentChosenEnemy.Shielded,
                currentChosenEnemy.GetAvatar().enableSprite, currentChosenEnemy.GetAvatar().disableSprite);
        }
    }
}