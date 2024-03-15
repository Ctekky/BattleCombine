
using System;
using System.Collections.Generic;
using BattleCombine.Data;
using BattleCombine.Gameplay;
using BattleCombine.ScriptableObjects;
using UnityEngine;
using Random = UnityEngine.Random;

namespace BattleCombine.UI
{
    public class UIUpdateStats : MonoBehaviour
    {
        [SerializeField] private Player firstEnemy;
        [SerializeField] private Player secondEnemy;
        [SerializeField] private Player thirdEnemy;
        [SerializeField] private Player confirmPanelEnemy;
        [SerializeField] private Player player;
        [SerializeField] private Player confirmPanelPlayer;
        [SerializeField] private SOEnemyRerollAnimationTable enemyRerollAnimationTable;

        public event Action onEndRerollTrigger; 

        private void OnEnable()
        {
            firstEnemy.GetUi().onEndRerollTrigger += EndReroll;
        }

        private void OnDisable()
        {
            firstEnemy.GetUi().onEndRerollTrigger -= EndReroll;
        }

        private void EndReroll()
        {
            onEndRerollTrigger?.Invoke();
        }
        public void UpdateEnemiesStats(List<EnemyStatsStruct> enemyStatsList)
        {
            UpdateEnemyStats(enemyStatsList[0], firstEnemy);
            UpdateEnemyStats(enemyStatsList[1], secondEnemy);
            UpdateEnemyStats(enemyStatsList[2], thirdEnemy);
        }

        public void UpdateConfirmPanelEnemy(Player chosenEnemy)
        {
            confirmPanelEnemy.SetDefaultStats(chosenEnemy.AttackValue, chosenEnemy.HealthValue,
                chosenEnemy.moveSpeedValue,
                chosenEnemy.Shielded);
            confirmPanelEnemy.SetStats(chosenEnemy.AttackValue, chosenEnemy.HealthValue, chosenEnemy.moveSpeedValue,
                chosenEnemy.Shielded);
            confirmPanelEnemy.SetupAvatar(chosenEnemy.GetAvatar(), chosenEnemy.AvatarID);
        }

        public void UpdateConfirmPanelPlayer(Player ourPlayer)
        {
            confirmPanelPlayer.SetDefaultStats(ourPlayer.AttackValue, ourPlayer.HealthValue, ourPlayer.moveSpeedValue,
                ourPlayer.Shielded);
            confirmPanelPlayer.SetDefaultStats(ourPlayer.AttackValue, ourPlayer.HealthValue, ourPlayer.moveSpeedValue,
                ourPlayer.Shielded);
            confirmPanelPlayer.SetupAvatar(ourPlayer.GetAvatar(), ourPlayer.AvatarID);
        }

        private void UpdateEnemyStats(EnemyStatsStruct enemyStatsStruct, Player enemyObject)
        {
            enemyObject.SetStats(enemyStatsStruct.attack, enemyStatsStruct.health, enemyStatsStruct.speed,
                enemyStatsStruct.shield);
        }

        public void UpdatePlayerStats(int attack, int health, int speed, bool shielded)
        {
            player.SetStats(attack, health, speed, shielded);
        }

        public void UpdateEnemiesAvatars(List<EnemyAvatarStruct> enemyAvatarList)
        {
            firstEnemy.SetupAvatar(enemyAvatarList[0], enemyAvatarList[0].ID);
            secondEnemy.SetupAvatar(enemyAvatarList[1], enemyAvatarList[1].ID);
            thirdEnemy.SetupAvatar(enemyAvatarList[2], enemyAvatarList[2].ID);
        }

        public void RerollEnemies()
        {
            var animationNameIndex = Random.Range(0, enemyRerollAnimationTable.rerollAnimationName.Count);
            firstEnemy.PlayRerollAnimation(enemyRerollAnimationTable.rerollAnimationName[animationNameIndex]);
            animationNameIndex = Random.Range(0, enemyRerollAnimationTable.rerollAnimationName.Count);
            secondEnemy.PlayRerollAnimation(enemyRerollAnimationTable.rerollAnimationName[animationNameIndex]);
            animationNameIndex = Random.Range(0, enemyRerollAnimationTable.rerollAnimationName.Count);
            thirdEnemy.PlayRerollAnimation(enemyRerollAnimationTable.rerollAnimationName[animationNameIndex]);
        }
    }
}