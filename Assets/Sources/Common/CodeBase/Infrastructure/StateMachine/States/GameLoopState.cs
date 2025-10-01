using Sources.Common.CodeBase.Services.Factories.GameFactory;
using Sources.Features.HexagonSort.Merge.Scripts;

namespace Sources.Common.CodeBase.Infrastructure.StateMachine.States
{
    public class GameLoopState : IState
    {
        private readonly IGameFactory _factory;
        private readonly IStacksSpawner _stacksSpawner;
        private MergeSystem _mergeSystem;

        public GameLoopState(IGameFactory factory, IStacksSpawner stacksSpawner)
        {
            _factory = factory;
            _stacksSpawner = stacksSpawner;
        }

        public void Enter()
        {
            _mergeSystem = _factory.MergeSystem;
            
            _mergeSystem.UpdateOccupiedCells();
            _stacksSpawner.SpawnStacks();
        }

        public void Exit()
        {
            
        }
    }
}