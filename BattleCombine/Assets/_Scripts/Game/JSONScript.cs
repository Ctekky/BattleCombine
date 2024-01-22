using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json.Linq;
using System.Linq;
using Kilosoft.Tools;
using System;
using BattleCombine.Enums;

public class JSONScript : MonoBehaviour
{
    [SerializeField] private List<PlayerStats> playerStats;
    [SerializeField] private List<EnemyStats> enemyStats;

    private string playerStatsRef = "https://tools.aimylogic.com/api/googlesheet2json?sheet=2.0 Player_stats&id=1SFbBWAJXGuWusJYaqJH5cBWSOWr1WG8b7GPF4INlzmU";
    private string enemyStatsRef = "https://tools.aimylogic.com/api/googlesheet2json?sheet=3.0 Enemy_stats&id=1SFbBWAJXGuWusJYaqJH5cBWSOWr1WG8b7GPF4INlzmU";

    [Serializable] public class PlayerStats
    {
        public int playerHealth;
        public int playerAttack;
        public int playerSpeed;
        public bool playerShield;
    }
    [Serializable] public class EnemyStats
    {
        public AiArchetypes enemyArchetype;
        public int enemyHealth;
        public int enemyAttack;
        public int enemySpeed;
        public bool enemyShield;
    }
    [EditorButton("Update Stats Button")]
    public void UpdateStats()
    {
        StartCoroutine(SheetData());
    }
    IEnumerator SheetData()
    {
        UnityWebRequest wwwPlayerStats = UnityWebRequest.Get(playerStatsRef);
        UnityWebRequest wwwEnemyStats = UnityWebRequest.Get(enemyStatsRef);

        yield return wwwPlayerStats.SendWebRequest();
        yield return wwwEnemyStats.SendWebRequest();

        if (wwwPlayerStats == null || wwwEnemyStats == null)
        {
            Debug.Log("ERROR, DB not loaded");
        }
        else
        {
            string jsonPlayerStats = wwwPlayerStats.downloadHandler.text;
            string jsonEnemyStats = wwwEnemyStats.downloadHandler.text;

            JToken playerStatsString = JToken.Parse(jsonPlayerStats);
            JToken enemyStatsString = JToken.Parse(jsonEnemyStats);

            var playerStatsToken = playerStatsString.First();
            playerStats[0].playerHealth = (int)playerStatsToken["HP"];
            playerStats[0].playerAttack = (int)playerStatsToken["Attack"];
            playerStats[0].playerSpeed = (int)playerStatsToken["Speed"];
            playerStats[0].playerShield = (bool)playerStatsToken["Shield"];

            var i = 0;
            foreach (var item in enemyStatsString)
            {
                enemyStats[i].enemyHealth = (int)item["HP"];
                enemyStats[i].enemyAttack = (int)item["Attack"];
                enemyStats[i].enemySpeed = (int)item["Speed"];
                enemyStats[i].enemyShield = (bool)item["Shield"];
                i++;
            }
        }
    }
}
