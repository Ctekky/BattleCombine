using System;
using System.Collections.Generic;
using System.Linq;
using BattleCombine.Data;
using BattleCombine.ScriptableObjects;
using UnityEngine;
using Random = UnityEngine.Random;

public class ChooseEnemy : MonoBehaviour
{
    [SerializeField] private List<EnemyStatsStruct> chosenList;
    [SerializeField] private List<EnemyStatsStruct> finalEnemyStatsList;
    [SerializeField] private SOEnemyStatsTable enemyStatsTable;
    [SerializeField] private SOStatsModifierTable statModifierTable;
    [SerializeField] private int enemyCountToChoose;
    [SerializeField] private int currentScore;

    public int CurrentScore
    {
        get => currentScore;
        set => currentScore = value;
    }

    private void GetSomeEnemies(List<EnemyStatsStruct> baseTable, int count)
    {
        var usedIndex = new List<int>();
        for (var i = 0; i < count; i++)
        {
            var index = 0;
            do
            {
                index = Random.Range(0, baseTable.Count);
                Debug.Log(usedIndex.Contains(index));
            } while (usedIndex.Contains(index));

            usedIndex.Add(index);
            chosenList.Add(baseTable[index]);
        }
    }

    private void ApplyStatModifiers(int enemyIndex, int score)
    {
        var finalEnemyStats = new EnemyStatsStruct();
        finalEnemyStats.health =
            Mathf.RoundToInt(chosenList[enemyIndex].health * (statModifierTable.healthBaseModifier +
                                                              score * statModifierTable.healthMultiplier));
        finalEnemyStats.attack =
            Mathf.RoundToInt(chosenList[enemyIndex].attack * (statModifierTable.attackBaseModifier +
                                                              score * statModifierTable.attackMultiplier));
        foreach (var scoreLine in statModifierTable.speedStatModifier.Where(scoreLine =>
                     score >= scoreLine.minScoreValue && score <= scoreLine.maxScoreValue))
        {
            finalEnemyStats.speed = chosenList[enemyIndex].speed + scoreLine.speedModifierValue;
        }

        finalEnemyStats.shield = chosenList[enemyIndex].shield;
        finalEnemyStatsList.Add(finalEnemyStats);
    }

    private void Start()
    {
        finalEnemyStatsList = new List<EnemyStatsStruct>();
        GetSomeEnemies(enemyStatsTable.enemyStatsStruct, enemyCountToChoose);
        var weakEnemyScore = currentScore - 1;
        if (weakEnemyScore <= 0) weakEnemyScore = 1;
        var normalEnemyScore = currentScore;
        var strongEnemyScore = currentScore + 1;
        ApplyStatModifiers(0, weakEnemyScore);
        ApplyStatModifiers(1, normalEnemyScore);
        ApplyStatModifiers(2, strongEnemyScore);
    }
}