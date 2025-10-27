using System;
using Sources.Common.CodeBase.Infrastructure;
using Sources.Common.CodeBase.Infrastructure.Utilities;
using Sources.Common.CodeBase.Services.InputService;
using Sources.Features.HexagonSort.GridSystem.GridGenerator.Scripts;
using Sources.Features.HexagonSort.HexagonStackSystem.Scripts;
using Sources.Features.HexagonSort.HexagonStackSystem.StackMover.Scripts;
using UnityEngine;

namespace Sources.Features.HexagonSort.StackCompleter
{
    public class StackCompleter : IDisposable, IStackCompleter
    {
        public event Action<HexagonStackScore> StackCompleted;
        public event Action DeleteAnimationCompleted;

        private readonly IStackSelectionLogic _stackSelectionLogic;
        private readonly IStackCompletionLogic _stackCompletionLogic;
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly IInputService _input;

        private Ray ClickedRay => RaycastUtils.GetClickedRay(_input.CursorPosition);

        private bool _isActive;
        private Coroutine _completeStackRoutine;

        public StackCompleter(IInputService input, IStackSelectionLogic stackSelectionLogic,
            IStackCompletionLogic stackCompletionLogic, ICoroutineRunner coroutineRunner)
        {
            _stackSelectionLogic = stackSelectionLogic;
            _stackCompletionLogic = stackCompletionLogic;
            _coroutineRunner = coroutineRunner;
            _input = input;

            SubscribeUpdates();
        }

        public void Reset()
        {
            if (_completeStackRoutine != null) 
                _coroutineRunner.StopCoroutine(_completeStackRoutine);
        }
        
        public void Activate() =>
            _isActive = true;

        public void Deactivate() =>
            _isActive = false;

        public void Dispose() =>
            CleanUp();

        private void SubscribeUpdates()
        {
            _input.CursorDown += OnCursorDown;

            _stackCompletionLogic.StackCompleted += OnStackCompleted;
            _stackCompletionLogic.DeleteAnimationCompleted += OnDeleteAnimationCompleted;
        }

        private void OnDeleteAnimationCompleted() =>
            DeleteAnimationCompleted?.Invoke();

        private void OnStackCompleted(HexagonStackScore stackScore) =>
            StackCompleted?.Invoke(stackScore);

        private void OnCursorDown()
        {
            if (_isActive == false)
                return;
            
            if (_stackSelectionLogic.TrySelectStackOnGrid(ClickedRay, out HexagonStack stack, out GridCell cell))
                _completeStackRoutine = _coroutineRunner.StartCoroutine(_stackCompletionLogic.CompleteStackRoutine(stack, cell));
        }

        private void CleanUp()
        {
            _input.CursorDown -= OnCursorDown;

            _stackCompletionLogic.StackCompleted -= OnStackCompleted;
            _stackCompletionLogic.DeleteAnimationCompleted -= OnDeleteAnimationCompleted;
        }
    }
}