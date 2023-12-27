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
        [SerializeField] private UiHelper tempEnemySelection;
        [SerializeField] private Player currentChosenEnemy;
        [SerializeField] private Player currentPlayer;
        [Inject] private MainGameService _mainGameService;
        [Inject] private PlayerAccount _playerAccount;
        [SerializeField] private int currentScore; //came from GameService

        private void OnEnable()
        {
            enemyUIChosenScript.onEnemyClick += ChooseEnemy;
            tempEnemySelection.BattleButtonClickEvent += BattleButtonClickEvent;
        }

        private void OnDisable()
        {
            enemyUIChosenScript.onEnemyClick -= ChooseEnemy;
            tempEnemySelection.BattleButtonClickEvent -= BattleButtonClickEvent;
        }

        private void Start()
        {
            _mainGameService.IsEnemySelectionScene = true;
            currentScore = _mainGameService.ArcadeCurrentScore;
            choseEnemyScript.CalculateEnemies(currentScore);
            enemyUIUpdateStats.UpdateEnemiesStats(choseEnemyScript.GetFinalEnemies());
            var playerBSD = _playerAccount.GetPlayerCurrentBattleStat();
            enemyUIUpdateStats.UpdatePlayerStats(playerBSD.DamageDefault + playerBSD.DamageModifier,
                playerBSD.HealthDefault + playerBSD.HealthModifier, playerBSD.SpeedDefault + playerBSD.SpeedModifier,
                playerBSD.Shield);
            enemyUIUpdateStats.UpdateEnemiesAvatars(choseEnemyScript.GetFinalAvatars());
            enemyUIUpdateStats.UpdateConfirmPanelPlayer(currentPlayer);
            _mainGameService.ChangeActiveBattle(false);
        }

        private void ChooseEnemy(Player player)
        {
            currentChosenEnemy = player;
            enemyUIUpdateStats.UpdateConfirmPanelEnemy(player);
            tempEnemySelection.NextButtonActivate();
        }

        private void BattleButtonClickEvent()
        {
            _mainGameService.SaveEnemy(currentChosenEnemy.AttackValue, currentChosenEnemy.HealthValue,
                currentChosenEnemy.moveSpeedValue, currentChosenEnemy.Shielded,
                currentChosenEnemy.GetAvatar().enableSprite, currentChosenEnemy.GetAvatar().disableSprite, currentChosenEnemy.GetAvatar().ID);
        }
        
    }
}