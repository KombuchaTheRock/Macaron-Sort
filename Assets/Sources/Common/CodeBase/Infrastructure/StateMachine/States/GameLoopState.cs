using Sources.Features.HexagonSort.Merge.Scripts;

namespace Sources.Common.CodeBase.Infrastructure.StateMachine.States
{
    public class GameLoopState : IState
    {
        private readonly IStacksSpawner _stacksSpawner;
        private readonly IStackMerger _stackMerger;

        public GameLoopState(IStacksSpawner stacksSpawner, IStackMerger stackMerger)
        {
            _stacksSpawner = stacksSpawner;
            _stackMerger = stackMerger;
        }

        public void Enter()
        {
            _stackMerger.UpdateOccupiedCells();
            _stacksSpawner.SpawnStacks();
        }

        public void Exit()
        {
            
        }
    }
}