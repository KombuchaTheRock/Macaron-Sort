using Sources.Common.CodeBase.Services;
using Sources.Features.HexagonSort.GridGenerator.Scripts;
using Sources.Features.HexagonSort.StackGenerator.Scripts;

namespace Sources.Common.CodeBase.Infrastructure.StateMachine.States
{
    public class LoadLevelState : IPayloadedState<string>
    {
        private readonly IGameStateMachine _stateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly IGameFactory _factory;
        private readonly IStackGenerator _stackGenerator;
        private readonly IGridGenerator _gridGenerator;
        private string _currentLevelName;

        public LoadLevelState(IGameStateMachine stateMachine, SceneLoader sceneLoader, IGameFactory factory, IStackGenerator stackGenerator, IGridGenerator gridGenerator)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
            _factory = factory;
            _stackGenerator = stackGenerator;
            _gridGenerator = gridGenerator;
        }

        public void Enter(string levelName)
        {
            _currentLevelName = levelName;
            _sceneLoader.Load(levelName, OnLoaded);
        }

        private void OnLoaded()
        {
            InitializeGameWorld();

            _stateMachine.Enter<GameLoopState>();
        }

        private void InitializeGameWorld()
        {
            _factory.CreateInstanceRoot();

            _gridGenerator.GenerateGrid();
            _stackGenerator.GenerateStacks();
        }

        public void Exit()
        {
        }
    }
}