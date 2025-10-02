
using Sources.Common.CodeBase.Services;

namespace Sources.Common.CodeBase.Infrastructure.StateMachine.States
{
    public class BootstrapState : IState
    {
        private readonly IGameStateMachine _stateMachine;
        private readonly SceneLoader _sceneLoader;

        public BootstrapState(IGameStateMachine stateMachine, SceneLoader sceneLoader)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
        }

        public void Enter() => 
            _sceneLoader.Load(SceneNames.Bootstrap, OnLoaded);

        public void Exit()
        {
        }

        private void OnLoaded() =>
            _stateMachine.Enter<LoadProgressState>();
    }
}