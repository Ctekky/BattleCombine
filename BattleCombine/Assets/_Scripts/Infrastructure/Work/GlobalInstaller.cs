using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GlobalInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<GameService>().AsSingle();
        Container.Bind<SceneNameService>().AsSingle();
    }
}
