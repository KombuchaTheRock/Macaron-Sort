using Sources.Common.CodeBase.Infrastructure.StateMachine;
using Sources.Common.CodeBase.Infrastructure.StateMachine.States;
using Sources.Common.CodeBase.Services;
using Sources.Common.CodeBase.Services.Factories.GameFactory;
using Sources.Features.HexagonSort.HexagonStackSystem.StackMover.Scripts;
using UnityEngine;
using Zenject;

public class ProgressCleaner : MonoBehaviour
{
    private IGameStateMachine _stateMachine;
    private IGameFactory _gameFactory;
    private IStackMover _stackMover;

    [Inject]
    private void Construct(IGameStateMachine stateMachine, IGameFactory gameFactory, IStackMover stackMover)
    {
        _stackMover = stackMover;
        _gameFactory = gameFactory;
        _stateMachine = stateMachine;
    }

    public void ClearProgress()
    {
        if (_gameFactory.MergeSystem.IsMerging || _stackMover.IsDragging)
            return;
        
        _stateMachine.Enter<ResetProgressState, bool>(true);
    }
}
