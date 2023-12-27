using System.Collections.Generic;
using System.Linq;
using BattleCombine.Data;
using BattleCombine.Interfaces;
using UnityEngine;
using Zenject;


public class PlayerAccount : MonoBehaviour, ISaveLoad
{
    [SerializeField] private string playerID;
    [SerializeField] private string playerName;
    [SerializeField] private int exp;
    [SerializeField] private int gold;
    [SerializeField] private int diamond;
    [SerializeField] private int currentScore;
    [SerializeField] private int maxScore;
    [SerializeField] private int playerAvatarID;
    [SerializeField] private Sprite playerAvatar;

    [SerializeField] private int arcadeAttackModifier;
    [SerializeField] private int arcadeHealthModifier;
    [SerializeField] private int arcadeSpeedModifier;
    [SerializeField] private int arcadeAttackDefault;
    [SerializeField] private int arcadeHealthDefault;
    [SerializeField] private int arcadeSpeedDefault;
    [SerializeField] private bool arcadeShield;
    [SerializeField] private int arcadeAttackCurrent;
    [SerializeField] private int arcadeHealthCurrent;
    [SerializeField] private int arcadeSpeedCurrent;


    [Inject] private ResourceService _resourceService;

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

    public int Diamond
    {
        get => diamond;
        set => diamond = value;
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

    public int PlayerAvatarID
    {
        get => playerAvatarID;
        set => playerAvatarID = value;
    }
    
    private void GetAvatarFromDB(int key)
    {
        foreach (var avatar in _resourceService.GetAvatarDB.avatarList.Where(avatar => key == avatar.ID))
        {
            playerAvatar = avatar.enableSprite;
        }
    }

    public void ZeroModifierOnLose()
    {
        arcadeAttackModifier = 0;
        arcadeHealthModifier = 0;
        arcadeSpeedModifier = 0;
    }


    public BattleStatsData GetPlayerCurrentBattleStat()
    {
        var bsd = new BattleStatsData
        {
            Name = "Player",
            CurrentDamage = arcadeAttackCurrent,
            CurrentHealth = arcadeHealthCurrent,
            CurrentSpeed = arcadeSpeedCurrent,
            DamageDefault = arcadeAttackDefault,
            HealthDefault = arcadeHealthDefault,
            SpeedDefault = arcadeSpeedDefault,
            DamageModifier = arcadeAttackModifier,
            HealthModifier = arcadeHealthModifier,
            SpeedModifier = arcadeSpeedModifier
        };
        return bsd;
    }

    public void CreatePlayerAccount(string newPlayerID, string newPlayerName, int newPlayerExp, int newPlayerGold,
        int newPlayerDiamond, int newPlayerScore, int newPlayerMaxScore, int newPlayerAvatarID,
        BattleStatsData battleStatsData)
    {
        playerID = newPlayerID;
        playerName = newPlayerName;
        exp = newPlayerExp;
        gold = newPlayerGold;
        diamond = newPlayerDiamond;
        currentScore = newPlayerScore;
        maxScore = newPlayerMaxScore;
        playerAvatarID = newPlayerAvatarID;
        arcadeAttackDefault = battleStatsData.DamageDefault;
        arcadeHealthDefault = battleStatsData.HealthDefault;
        arcadeSpeedDefault = battleStatsData.SpeedDefault;
        arcadeAttackModifier = battleStatsData.DamageModifier;
        arcadeAttackModifier = battleStatsData.HealthModifier;
        arcadeSpeedModifier = battleStatsData.SpeedModifier;
        arcadeAttackCurrent = battleStatsData.CurrentDamage;
        arcadeHealthCurrent = battleStatsData.CurrentHealth;
        arcadeSpeedCurrent = battleStatsData.CurrentSpeed;
        arcadeShield = battleStatsData.Shield;
    }

    public void LoadData(GameData gameData, bool newGameBattle, bool firstStart)
    {
        var gdPad = gameData.PlayerAccountData;
        playerID = gdPad.PlayerID;
        playerName = gdPad.PlayerName;
        exp = gdPad.Exp;
        gold = gdPad.Gold;
        diamond = gdPad.Diamond;
        currentScore = gdPad.CurrentScore;
        maxScore = gdPad.MaxScore;
        playerAvatarID = gdPad.PlayerAvatarID;
        GetAvatarFromDB(playerAvatarID);
        if (gameData.IsBattleActive) LoadBattleStatData(gameData.BattleStatsData);
        else
        {
            LoadNewGameStatData(gameData.BattleStatsData);
        }
    }

    public void AttackStatUp(int value)
    {
        arcadeAttackModifier += value;
    }

    public void HealthStatUp(int value)
    {
        arcadeHealthModifier += value;
    }

    public void SpeedStatUp(int value)
    {
        arcadeSpeedModifier += value;
    }

    private void LoadBattleStatData(IEnumerable<BattleStatsData> battleStatsList)
    {
        foreach (var bsd in battleStatsList.Where(bsd => bsd.Name == "Player"))
        {
            arcadeShield = bsd.Shield;
            arcadeAttackDefault = bsd.DamageDefault;
            arcadeHealthDefault = bsd.HealthDefault;
            arcadeSpeedDefault = bsd.SpeedDefault;
            arcadeAttackModifier = bsd.DamageModifier;
            arcadeHealthModifier = bsd.HealthModifier;
            arcadeSpeedModifier = bsd.SpeedModifier;
            arcadeAttackCurrent = bsd.CurrentDamage;
            arcadeHealthCurrent = bsd.CurrentHealth;
            arcadeSpeedCurrent = bsd.CurrentSpeed;
        }
    }

    private void LoadNewGameStatData(IEnumerable<BattleStatsData> battleStatsList)
    {
        foreach (var bsd in battleStatsList.Where(bsd => bsd.Name == "Player"))
        {
            arcadeShield = bsd.Shield;
            arcadeAttackDefault = bsd.DamageDefault;
            arcadeHealthDefault = bsd.HealthDefault;
            arcadeSpeedDefault = bsd.SpeedDefault;
            arcadeAttackModifier = bsd.DamageModifier;
            arcadeHealthModifier = bsd.HealthModifier;
            arcadeSpeedModifier = bsd.SpeedModifier;
            arcadeAttackCurrent = bsd.DamageDefault + bsd.DamageModifier;
            arcadeHealthCurrent = bsd.HealthDefault + bsd.HealthModifier;
            arcadeSpeedCurrent = bsd.SpeedDefault + bsd.SpeedModifier;
        }
    }

    private void SaveBattleStatDataUpdate(GameData gameData)
    {
        foreach (var bsd in gameData.BattleStatsData.Where(bsd => bsd.Name == "Player"))
        {
            bsd.Shield = arcadeShield;
            bsd.DamageDefault = arcadeAttackDefault;
            bsd.HealthDefault = arcadeHealthDefault;
            bsd.SpeedDefault = arcadeSpeedDefault;
            bsd.DamageModifier = arcadeAttackModifier;
            bsd.HealthModifier = arcadeHealthModifier;
            bsd.SpeedModifier = arcadeSpeedModifier;
            bsd.CurrentDamage = arcadeAttackCurrent;
            bsd.CurrentHealth = arcadeHealthCurrent;
            bsd.CurrentSpeed = arcadeSpeedCurrent;
        }
    }

    public void SaveData(ref GameData gameData, bool newGameBattle, bool firstStart)
    {
        var updateBattleStatData = false;
        var gdPad = gameData.PlayerAccountData;

        if (firstStart)
        {
            gdPad.PlayerID = PlayerID;
            gdPad.PlayerName = PlayerName;
        }

        gdPad.Exp = Exp;
        gdPad.Gold = Gold;
        gdPad.Diamond = Diamond;
        gdPad.PlayerAvatarID = PlayerAvatarID;
        foreach (var bsd in gameData.BattleStatsData.Where(bsd => bsd.Name == "Player"))
        {
            updateBattleStatData = true;
        }

        if (updateBattleStatData)
        {
            SaveBattleStatDataUpdate(gameData);
        }
        else
        {
            var currentBSD = GetPlayerCurrentBattleStat();
            gameData.BattleStatsData.Add(currentBSD);
        }
    }
}