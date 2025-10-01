using Sources.Common.CodeBase.Services.Factories.GameFactory;
using Sources.Features.HexagonSort.Merge.Scripts;

namespace Sources.Common.CodeBase.Infrastructure.StateMachine.States
{
    public class GameLoopState : IState
    {
        private readonly IStacksSpawner _stacksSpawner;
        private MergeSystem _mergeSystem;

        public GameLoopState(IGameFactory factory, IStacksSpawner stacksSpawner)
        {
            _stacksSpawner = stacksSpawner;
            _mergeSystem = factory.MergeSystem;
        }

        public void Enter()
        {
            _mergeSystem.UpdateOccupiedCells();
            _stacksSpawner.SpawnStacks();
        }

        public void Exit()
        {
            
        }
    }
}