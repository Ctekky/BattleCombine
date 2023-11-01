using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class AnimationServiceInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<AnimationService>().AsSingle();
    }
}
