using Cysharp.Threading.Tasks;
using Sources.Common.CodeBase.Services.PlayerProgress;

namespace Sources.Common.CodeBase.Infrastructure.StateMachine.States
{
    public class LoadProgressState : IState
    {
        private readonly IGameStateMachine _gameStateMachine;
        private readonly IGameProgressService _gameProgressService;

        public LoadProgressState(IGameStateMachine gameStateMachine, IGameProgressService gameProgressService)
        {
            _gameStateMachine = gameStateMachine;
            _gameProgressService = gameProgressService;
        }        
        
        public void Enter() => 
            LoadProgressOrInitNew().Forget();

        private async UniTask LoadProgressOrInitNew()
        {
            bool saveExists = await _gameProgressService.SavedProgressExists();

            if (saveExists)
            {
                await _gameProgressService.LoadProgressAsync();
            }
            else
            {
                _gameProgressService.InitializeNewProgress();
                
                await _gameProgressService.SavePersistentProgressAsync();
                await _gameProgressService.SaveControlPointProgressAsync();
            }

            _gameStateMachine.Enter<LoadLevelState, string>(SceneNames.Gameplay);
        }

        public void Exit()
        {
            
        }
    }
}