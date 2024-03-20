using System;
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
        [Inject] private SaveManager _saveManager;
        [SerializeField] private int currentScore; //came from GameService

        private void OnEnable()
        {
            enemyUIChosenScript.onEnemyClick += ChooseEnemy;
            enemyUIChosenScript.onDeselectAll += DeselectAllEnemies;
            tempEnemySelection.BattleButtonClickEvent += BattleButtonClickEvent;
            tempEnemySelection.RerollButtonClickEvent += RerollButtonClickEvent;
            enemyUIUpdateStats.onEndRerollTrigger += EndReroll;

        }

        private void OnDisable()
        {
            enemyUIChosenScript.onEnemyClick -= ChooseEnemy;
            enemyUIChosenScript.onDeselectAll -= DeselectAllEnemies;
            tempEnemySelection.BattleButtonClickEvent -= BattleButtonClickEvent;
            tempEnemySelection.RerollButtonClickEvent -= RerollButtonClickEvent;
            enemyUIUpdateStats.onEndRerollTrigger -= EndReroll;
        }

        private void Start()
        {
            GenerateEnemies();
            _saveManager.SaveGame();
        }

        private void GenerateEnemies()
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

        private void DeselectAllEnemies()
        {
            tempEnemySelection.NextButtonDeactivate();
        }

        private void BattleButtonClickEvent()
        {
            _mainGameService.SaveEnemy(currentChosenEnemy.AttackValue, currentChosenEnemy.HealthValue,
                currentChosenEnemy.moveSpeedValue, currentChosenEnemy.Shielded,
                currentChosenEnemy.GetAvatar().enableSprite, currentChosenEnemy.GetAvatar().disableSprite, currentChosenEnemy.GetAvatar().ID);
        }

        private void RerollButtonClickEvent()
        {
            enemyUIUpdateStats.RerollEnemies();
        }

        private void EndReroll()
        {
            GenerateEnemies();
            DeselectAllEnemies();
            tempEnemySelection.ChangeRerollButtonState(true);
        }
        
    }
}