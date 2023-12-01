using System.Collections.Generic;
using BattleCombine.Data;
using BattleCombine.Gameplay;
using UnityEngine;
using UnityEngine.Serialization;

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

        public void UpdateEnemiesStats(List<EnemyStatsStruct> enemyStatsList)
        {
            UpdateEnemyStats(enemyStatsList[0], firstEnemy);
            UpdateEnemyStats(enemyStatsList[1], secondEnemy);
            UpdateEnemyStats(enemyStatsList[2], thirdEnemy);
        }

        public void UpdateConfirmPanelEnemy(Player chosenEnemy)
        {
            confirmPanelEnemy.SetStats(chosenEnemy.AttackValue, chosenEnemy.HealthValue, chosenEnemy.moveSpeedValue,
                chosenEnemy.Shielded);
            confirmPanelEnemy.SetupAvatar(chosenEnemy.GetAvatar());
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
            firstEnemy.SetupAvatar(enemyAvatarList[0]);
            secondEnemy.SetupAvatar(enemyAvatarList[1]);
            thirdEnemy.SetupAvatar(enemyAvatarList[2]);
        }
    }
}