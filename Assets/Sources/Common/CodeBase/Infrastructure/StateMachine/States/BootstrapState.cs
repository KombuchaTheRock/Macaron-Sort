
using Sources.Common.CodeBase.Services;

namespace Sources.Common.CodeBase.Infrastructure.StateMachine.States
{
    public class BootstrapState : IState
    {
        private readonly IGameStateMachine _stateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly IStaticDataService _staticData;

        public BootstrapState(IGameStateMachine stateMachine, SceneLoader sceneLoader, IStaticDataService staticData)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
            _staticData = staticData;
        }

        public void Enter()
        {
            _sceneLoader.Load(SceneNames.Bootstrap, OnLoaded);
        }

        private void OnLoaded() =>
            _stateMachine.Enter<LoadProgressState>();

        public void Exit()
        {
        }
    }
}