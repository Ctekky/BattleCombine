using Zenject;
using BattleCombine.Services;
using UnityEngine;

public class MainServiceInstaller : MonoInstaller
{
    [SerializeField] private PlayerAccount playerAccountPrefab;
    [SerializeField] private SaveManager saveManagerPrefab;
    [SerializeField] private MainGameService gameServicePrefab;
    public override void InstallBindings()
    {
        var playerAccount =
            Container.InstantiatePrefabForComponent<PlayerAccount>(playerAccountPrefab);
        Container.Bind<PlayerAccount>().FromInstance(playerAccount).AsSingle();
        DontDestroyOnLoad(playerAccount);
        var saveManager = Container.InstantiatePrefabForComponent<SaveManager>(saveManagerPrefab);
        Container.Bind<SaveManager>().FromInstance(saveManager).AsSingle();
        DontDestroyOnLoad(saveManager);
        var gameService = Container.InstantiatePrefabForComponent<MainGameService>(gameServicePrefab);
        Container.Bind<MainGameService>().FromInstance(gameServicePrefab).AsSingle();
        DontDestroyOnLoad(gameService);
    }
}