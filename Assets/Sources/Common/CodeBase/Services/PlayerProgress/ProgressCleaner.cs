using Sources.Common.CodeBase.Infrastructure.StateMachine;
using Sources.Common.CodeBase.Infrastructure.StateMachine.States;
using Sources.Features.HexagonSort.HexagonStackSystem.StackMover.Scripts;
using Sources.Features.HexagonSort.Merge.Scripts;
using UnityEngine;
using Zenject;

namespace Sources.Common.CodeBase.Services.PlayerProgress
{
    public class ProgressCleaner : MonoBehaviour
    {
        private IGameStateMachine _stateMachine;
        private IStackMover _stackMover;
        private IStackMerger _stackMerger;

        [Inject]
        private void Construct(IGameStateMachine stateMachine, IStackMover stackMover, IStackMerger stackMerger)
        {
            _stackMerger = stackMerger;
            _stackMover = stackMover;
            _stateMachine = stateMachine;
        }

        public void ClearProgress()
        {
            if (_stackMerger.IsMerging || _stackMover.IsDragging)
                return;
        
            _stateMachine.Enter<ResetProgressState, bool>(true);
        }
    }
}
