using System;
using System.Collections.Generic;
using System.Linq;
using BattleCombine.Data;
using BattleCombine.ScriptableObjects;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public class ChooseEnemy : MonoBehaviour
{
    [SerializeField] private List<EnemyStatsStruct> chosenList;
    [SerializeField] private List<EnemyStatsStruct> finalEnemyStatsList;
    [SerializeField] private List<EnemyAvatarStruct> chosenAvatarList;
    [SerializeField] private SOEnemyStatsTable enemyStatsTable;
    [SerializeField] private SOStatsModifierTable statModifierTable;
    //[SerializeField] private SOEnemyAvatarTable enemyAvatarTable;
    [Inject] private ResourceService _resourceService;
    [SerializeField] private int enemyCountToChoose;
    [SerializeField] private int currentScore;

    public int CurrentScore
    {
        get => currentScore;
        set => currentScore = value;
    }

    private void Start()
    {
        finalEnemyStatsList = new List<EnemyStatsStruct>();
        chosenAvatarList = new List<EnemyAvatarStruct>();
        chosenList = new List<EnemyStatsStruct>();
    }

    private void GetSomeEnemies(List<EnemyStatsStruct> baseTable, int count)
    {
        var usedIndex = new List<int>();
        for (var i = 0; i < count; i++)
        {
            /*
            var index = 0;
            do
            {
                index = Random.Range(0, baseTable.Count);
            } while (usedIndex.Contains(index));

            usedIndex.Add(index);*/
            var index = Random.Range(0, baseTable.Count);
            chosenList.Add(baseTable[index]);
        }
    }

    private void GetSomeAvatars(List<EnemyAvatarStruct> baseTable, int count)
    {
        var usedIndex = new List<int>();
        for (var i = 0; i < count; i++)
        {
            var index = 0;
            do
            {
                index = Random.Range(0, baseTable.Count);
            } while (usedIndex.Contains(index));

            usedIndex.Add(index);
            chosenAvatarList.Add(baseTable[index]);
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

    public void CalculateEnemies(int score)
    {
        finalEnemyStatsList.Clear();
        chosenList.Clear();
        GetSomeEnemies(enemyStatsTable.enemyStatsStruct, enemyCountToChoose);
        var weakEnemyScore = score - 1;
        if (weakEnemyScore <= 0) weakEnemyScore = 0;
        var strongEnemyScore = score + 1;
        ApplyStatModifiers(0, weakEnemyScore);
        ApplyStatModifiers(1, score);
        ApplyStatModifiers(2, strongEnemyScore);
    }

    public List<EnemyAvatarStruct> GetFinalAvatars()
    {
        chosenAvatarList.Clear();
        GetSomeAvatars(_resourceService.GetAvatarDB.avatarList, enemyCountToChoose);
        return chosenAvatarList;
    }

    public List<EnemyStatsStruct> GetFinalEnemies()
    {
        return finalEnemyStatsList;
    }
}