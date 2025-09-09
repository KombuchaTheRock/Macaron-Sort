using Sources.Common.CodeBase.Infrastructure.StateMachine;
using Zenject;

namespace Sources.Common.CodeBase.Infrastructure.Installers
{
    public class GameStateMachineInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindGameStates();
            BindGameStateMachine();
        }

        private void BindGameStates()
        {
            Container.Bind<BootstrapState>()
                .AsSingle();

            Container.Bind<LoadProgressState>()
                .AsSingle();

            Container.Bind<LoadLevelState>()
                .AsSingle();

            Container.Bind<GameLoopState>()
                .AsSingle();
        }

        private void BindGameStateMachine()
        {
            Container.BindInterfacesTo<GameStateMachine>()
                .AsSingle();
        }
    }
}