using Sources.Common.CodeBase.Infrastructure.StateMachine.States;
using Sources.Common.CodeBase.Services.Factories.GameFactory;
using Sources.Common.CodeBase.Services.Factories.HexagonFactory;
using Sources.Common.CodeBase.Services.PlayerProgress;
using Sources.Common.CodeBase.Services.SoundService;
using Sources.Common.CodeBase.Services.WindowService;
using Zenject;

namespace Sources.Common.CodeBase.Infrastructure.Installers
{
    public class FactoriesInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindGameStateFactory();
            BindGameFactory();
            BindHexagonFactory();
            BindAudioFactory();
            BindSaveDataFactory();
            BindUIFactory();
        }
        
        private void BindUIFactory()
        {
            Container.Bind<IUIFactory>()
                .To<UIFactory>()
                .AsSingle()
                .WithArguments(Container);
        }
        
        private void BindAudioFactory()
        {
            Container.Bind<IAudioFactory>()
                .To<AudioFactory>()
                .AsSingle()
                .WithArguments(Container);
        }
        
        private void BindHexagonFactory()
        {
            Container.Bind<IHexagonFactory>()
                .To<HexagonFactory>()
                .AsSingle()
                .WithArguments(Container);
        }

        private void BindSaveDataFactory()
        {
            Container.Bind<ISaveDataFactory>()
                .To<SaveDataFactory>()
                .AsSingle();
        }
        
        private void BindGameStateFactory()
        {
            Container.Bind<GameStateFactory>()
                .AsSingle();
        }
        
        private void BindGameFactory()
        {
            Container.Bind<IGameFactory>()
                .To<GameFactory>()
                .AsSingle()
                .WithArguments(Container);
        }
    }
}