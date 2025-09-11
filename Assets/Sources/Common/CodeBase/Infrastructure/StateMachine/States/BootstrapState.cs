
namespace Sources.Common.CodeBase.Infrastructure.StateMachine.States
{
    public class BootstrapState : IState
    {
        private const string BootstrapScene = "Bootstrap";
        
        private readonly IGameStateMachine _stateMachine;
        private readonly SceneLoader _sceneLoader;

        public BootstrapState(IGameStateMachine stateMachine, SceneLoader sceneLoader)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
        }
        
        public void Enter() => 
            _sceneLoader.Load(BootstrapScene, OnLoaded);

        private void OnLoaded() =>
            _stateMachine.Enter<LoadProgressState>();

        public void Exit()
        {
        }
    }
}