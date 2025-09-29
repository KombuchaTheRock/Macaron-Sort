using Sources.Common.CodeBase.Infrastructure.StateMachine;
using Sources.Common.CodeBase.Infrastructure.StateMachine.States;
using Zenject;

namespace Sources.Common.CodeBase.Infrastructure.Installers
{
    public class GameStateMachineInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindGameStateMachine();
            BindGameStates();
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
            
            Container.Bind<ResetProgressState>()
                .AsSingle();
        }

        private void BindGameStateMachine()
        {
            Container.BindInterfacesTo<GameStateMachine>()
                .AsSingle();
        }
    }
}