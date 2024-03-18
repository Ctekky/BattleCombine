using _Scripts.Audio;
using Zenject;

namespace _Scripts.Infrastructure.Installers
{
    public class AudioServiceInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindAudioService();
        }
    
        private void BindAudioService()
        {
            Container.BindInterfacesAndSelfTo<AudioService>().AsSingle();
        }
    }
}