using Zenject;
using BattleCombine.Services;
using UnityEngine;

public class GlobalInstaller : MonoInstaller
{
    [SerializeField] private ResourceService resourceServicePrefab;
    [SerializeField] private PlayerAccount playerAccountPrefab;
    [SerializeField] private SaveManager saveManagerPrefab;
    [SerializeField] private MainGameService gameServicePrefab;

    public override void InstallBindings()
    {
        var resourceService = 
            Container.InstantiatePrefabForComponent<ResourceService>(resourceServicePrefab);
        Container.Bind<ResourceService>().FromInstance(resourceService).AsSingle();
        var playerAccount =
            Container.InstantiatePrefabForComponent<PlayerAccount>(playerAccountPrefab);
        Container.Bind<PlayerAccount>().FromInstance(playerAccount).AsSingle();
        var gameService = Container.InstantiatePrefabForComponent<MainGameService>(gameServicePrefab);
        Container.Bind<MainGameService>().FromInstance(gameService).AsSingle();
        var saveManager = Container.InstantiatePrefabForComponent<SaveManager>(saveManagerPrefab);
        Container.Bind<SaveManager>().FromInstance(saveManager).AsSingle();
    }
}