using UnityEngine;
using Zenject;

public class InitialSceneInstaller : MonoInstaller
{
    [SerializeField] private InitialSceneService initialServicePrefab;

    public override void InstallBindings()
    {
        var initialService = Container.InstantiatePrefabForComponent<InitialSceneService>(initialServicePrefab);
        Container.Bind<InitialSceneService>().FromInstance(initialService).AsSingle();
    }
}