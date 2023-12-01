using BattleCombine.Data;
using BattleCombine.Gameplay;
using BattleCombine.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAccount : MonoBehaviour, ISaveLoad
{
    [SerializeField] private string playerID;
    [SerializeField] private string playerName;
    [SerializeField] private int exp;
    [SerializeField] private int gold;
    [SerializeField] private int currentScore;
    [SerializeField] private int maxScore;

    public string PlayerID
    {
        get => playerID;
        set => playerID = value;
    }
    public string PlayerName
    {
        get => playerName;
        set => playerName = value;
    }
    public int Exp
    {
        get => exp;
        set => exp = value;
    }
    public int Gold
    {
        get => gold;
        set => gold = value;
    }
    public int CurrentScore
    {
        get => currentScore;
        set => currentScore = value;
    }
    public int MaxScore
    {
        get => maxScore;
        set => maxScore = value;
    }

    public void LoadData(GameData gameData, bool newGameBattle, bool firstStart)
    {
        var gdPad = gameData.playerAccountData;

        PlayerID = gdPad.PlayerID;
        PlayerName = gdPad.PlayerName;
        Exp = gdPad.Exp;
        Gold = gdPad.Gold;
        CurrentScore = gdPad.CurrentScore;
        MaxScore = gdPad.MaxScore;
    }

    public void SaveData(ref GameData gameData, bool newGameBattle, bool firstStart)
    {
        var gdPad = gameData.playerAccountData;

        if (firstStart == true)
        {
            gdPad.PlayerID = PlayerID;
            gdPad.PlayerName = PlayerName;
        }

        gdPad.Exp = Exp;
        gdPad.Gold = Gold;
        gdPad.CurrentScore = CurrentScore;
        gdPad.MaxScore = MaxScore;
    }
}
