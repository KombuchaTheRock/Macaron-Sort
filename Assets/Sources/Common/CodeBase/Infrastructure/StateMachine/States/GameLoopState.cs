using Sources.Features.HexagonSort.Merge.Scripts;

namespace Sources.Common.CodeBase.Infrastructure.StateMachine.States
{
    public class GameLoopState : IState
    {
        private readonly IStackSpawner _stackSpawner;
        private readonly IStackMerger _stackMerger;

        public GameLoopState(IStackSpawner stackSpawner, IStackMerger stackMerger)
        {
            _stackSpawner = stackSpawner;
            _stackMerger = stackMerger;
        }

        public void Enter()
        {
            _stackMerger.UpdateOccupiedCells();
            _stackSpawner.SpawnNewStacks();
        }

        public void Exit()
        {
            
        }
    }
}