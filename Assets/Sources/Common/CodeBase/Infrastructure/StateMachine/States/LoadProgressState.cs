using Cysharp.Threading.Tasks;
using Sources.Common.CodeBase.Services.PlayerProgress;
using Sources.Common.CodeBase.Services.Settings;

namespace Sources.Common.CodeBase.Infrastructure.StateMachine.States
{
    public class LoadProgressState : IState
    {
        private readonly IGameStateMachine _gameStateMachine;
        private readonly IGameProgressService _gameProgressService;
        private readonly IGameSettings _gameSettings;

        public LoadProgressState(IGameStateMachine gameStateMachine, IGameProgressService gameProgressService,
            IGameSettings gameSettings)
        {
            _gameStateMachine = gameStateMachine;
            _gameProgressService = gameProgressService;
            _gameSettings = gameSettings;
        }

        public void Enter()
        {
            LoadProgressOrInitNew().Forget();
        }

        public void Exit()
        {
        }

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

            await _gameSettings.LoadSettings();
            _gameSettings.ApplySettings();

            _gameStateMachine.Enter<LoadLevelState, string>(SceneNames.Gameplay);
        }
    }
}