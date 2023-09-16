using BattleCombine.Gameplay;
using BattleCombine.Ai;
using UnityEngine;
using Zenject;

namespace BattleCombine
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private AiHandler _handler;
        public override void InstallBindings()
        {
            CreateFabric();
            BindAiHandler();
        }

        private void CreateFabric()
        {
            Container.Bind<PlaceholderFactory<Transform, GameObject>>().To<FieldCreateFactory>().AsSingle();
        }

        private void BindAiHandler()
        {
            Container.BindInstance(_handler).AsSingle().NonLazy();
        }
    }
}