using UnityEngine;
using Zenject;

public class ResourceServiceInstaller : MonoInstaller
{
    [SerializeField] private AssetsData data;

    public override void InstallBindings()
    {
        Container.Bind<ResourceService>().AsSingle().WithArguments(data);
    }


}
