using Sources.Common.CodeBase.Infrastructure.StateMachine;
using Sources.Common.CodeBase.Infrastructure.StateMachine.States;
using Sources.Common.CodeBase.Services;
using UnityEngine;
using Zenject;

public class ProgressCleaner : MonoBehaviour
{
    private IGameStateMachine _stateMachine;
    private IGameFactory _gameFactory;

    [Inject]
    private void Construct(IGameStateMachine stateMachine, IGameFactory gameFactory)
    {
        _gameFactory = gameFactory;
        _stateMachine = stateMachine;
    }

    public void ClearProgress()
    {
        if (_gameFactory.MergeSystem.IsMerging || _gameFactory.StackMover.IsDragging)
            return;
        
        _stateMachine.Enter<ResetProgressState, bool>(true);
    }
}
