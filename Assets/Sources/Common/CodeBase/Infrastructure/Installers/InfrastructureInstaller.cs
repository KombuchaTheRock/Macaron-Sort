using Sources.Common.CodeBase.Infrastructure.StateMachine;
using Sources.Common.CodeBase.Services;
using Zenject;

namespace Sources.Common.CodeBase.Infrastructure.Installers
{
    public class InfrastructureInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindGameStateFactory();
            BindSceneLoader();
            BindGameFactory();
        }

        private void BindGameFactory()
        {
            Container.Bind<IGameFactory>()
                .To<GameFactory>()
                .AsSingle()
                .WithArguments(Container);
        }

        private void BindSceneLoader()
        {
            Container.Bind<SceneLoader>()
                .AsSingle()
                .WithArguments(this);
        }

        private void BindGameStateFactory()
        {
            Container.Bind<GameStateFactory>()
                .AsSingle();
        }
    }
}