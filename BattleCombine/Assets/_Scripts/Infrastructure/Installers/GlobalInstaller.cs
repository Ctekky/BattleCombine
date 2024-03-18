using _Scripts.Audio;
using Zenject;
using BattleCombine.Services;
using UnityEngine;

public class GlobalInstaller : MonoInstaller
{
    [SerializeField] private ResourceService resourceServicePrefab;
    [SerializeField] private PlayerAccount playerAccountPrefab;
    [SerializeField] private SaveManager saveManagerPrefab;
    [SerializeField] private MainGameService gameServicePrefab;
    [SerializeField] private GlobalEventService globalEventServicePrefab;
    [SerializeField] private AudioService audioServicePrefab;
    [SerializeField] private AnimationService animationServicePrefab;

    public override void InstallBindings()
    {
        var resourceService = 
            Container.InstantiatePrefabForComponent<ResourceService>(resourceServicePrefab);
        Container.Bind<ResourceService>().FromInstance(resourceService).AsSingle();
        var animationService = Container.InstantiatePrefabForComponent<AnimationService>(animationServicePrefab);
        Container.Bind<AnimationService>().FromInstance(animationService).AsSingle();
        var eventService = Container.InstantiatePrefabForComponent<GlobalEventService>(globalEventServicePrefab);
        Container.Bind<GlobalEventService>().FromInstance(eventService).AsSingle();
        var playerAccount =
            Container.InstantiatePrefabForComponent<PlayerAccount>(playerAccountPrefab);
        Container.Bind<PlayerAccount>().FromInstance(playerAccount).AsSingle();
        var gameService = Container.InstantiatePrefabForComponent<MainGameService>(gameServicePrefab);
        Container.Bind<MainGameService>().FromInstance(gameService).AsSingle();
        var saveManager = Container.InstantiatePrefabForComponent<SaveManager>(saveManagerPrefab);
        Container.Bind<SaveManager>().FromInstance(saveManager).AsSingle();

        
        var audioService = Container.InstantiatePrefabForComponent<AudioService>(audioServicePrefab);
        Container.Bind<AudioService>().FromInstance(audioService).AsSingle();
    }
}