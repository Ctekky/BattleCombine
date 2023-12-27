using System;
using BattleCombine.Data;
using BattleCombine.Gameplay;
using BattleCombine.Services;
using UnityEngine;
using Zenject;

public class ArcadeGameInitializer : MonoBehaviour
{
    [Inject] private PlayerAccount _playerAccount;
    [Inject] private MainGameService _mainGameService;
    [Inject] private SaveManager _saveManager;

    [SerializeField] private Player player;
    [SerializeField] private Player enemy;
    
    private void Start()
    {
        _mainGameService.IsEnemySelectionScene = false;
        SetupEnemyData(_mainGameService.EnemyAttack, _mainGameService.EnemyHealth, _mainGameService.EnemySpeed,
            _mainGameService.EnemyShielded, _mainGameService.EnemyAvatarEnable,
            _mainGameService.EnemyAvatarDisable, _mainGameService.EnemyAvatarID);
        var playerBSD = _playerAccount.GetPlayerCurrentBattleStat();
        SetupPlayerData(_playerAccount.GetPlayerCurrentBattleStat());
    }

    private void SetupEnemyData(int attack, int health, int speed, bool shield, Sprite avatarEnable,
        Sprite avatarDisable, int id)
    {
        var avatarStruct = new EnemyAvatarStruct
        {
            enableSprite = avatarEnable,
            disableSprite = avatarDisable
        };
        enemy.SetStats(attack, health, speed, shield);
        enemy.SetupAvatar(avatarStruct, id);
        enemy.SetupDefaultStats(attack, health, speed, shield);
    }

    private void SetupPlayerData(BattleStatsData playerBSD)
    {
        player.SetStats(playerBSD.CurrentDamage, playerBSD.CurrentHealth, playerBSD.CurrentSpeed, playerBSD.Shield);
        player.SetupFullStats(_playerAccount.GetPlayerCurrentBattleStat());
    }
}