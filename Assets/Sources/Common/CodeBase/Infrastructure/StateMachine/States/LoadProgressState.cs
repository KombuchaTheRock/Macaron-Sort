using Sources.Common.CodeBase.Services.PlayerProgress;

namespace Sources.Common.CodeBase.Infrastructure.StateMachine.States
{
    public class LoadProgressState : IState
    {
        private readonly IGameStateMachine _gameStateMachine;
        private readonly IPlayerProgress _playerProgress;

        public LoadProgressState(IGameStateMachine gameStateMachine, IPlayerProgress playerProgress)
        {
            _gameStateMachine = gameStateMachine;
            _playerProgress = playerProgress;
        }        
        
        public void Enter()
        {
            InitializePlayerProgress();
            
            _gameStateMachine.Enter<LoadLevelState, string>(SceneNames.Gameplay);
        }

        private void InitializePlayerProgress() => 
            _playerProgress.Progress = new Progress();

        public void Exit()
        {
            
        }
    }
}