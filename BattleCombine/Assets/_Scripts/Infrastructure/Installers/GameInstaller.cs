using UnityEngine;
using Zenject;

namespace Scripts
{
    public class GameInstaller : Installer<GameInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<PlaceholderFactory<Transform, GameObject>>().To<FieldCreateFactory>().AsSingle();
        }
    }
}