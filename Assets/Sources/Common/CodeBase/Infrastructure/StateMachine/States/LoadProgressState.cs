namespace Sources.Common.CodeBase.Infrastructure.StateMachine.States
{
    public class LoadProgressState : IState
    {
        private readonly IGameStateMachine _gameStateMachine;

        public LoadProgressState(IGameStateMachine gameStateMachine)
        {
            _gameStateMachine = gameStateMachine;
        }        
        
        public void Enter()
        {
            _gameStateMachine.Enter<LoadLevelState, string>(SceneNames.Gameplay);
        }

        public void Exit()
        {
            
        }
    }
}