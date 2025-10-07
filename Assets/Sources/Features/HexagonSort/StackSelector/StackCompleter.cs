using System;
using Sources.Common.CodeBase.Infrastructure.Utilities;
using Sources.Common.CodeBase.Services.InputService;
using Sources.Features.HexagonSort.GridSystem.GridGenerator.Scripts;
using Sources.Features.HexagonSort.HexagonStackSystem.Scripts;
using Sources.Features.HexagonSort.HexagonStackSystem.StackMover.Scripts;
using UnityEngine;

namespace Sources.Features.HexagonSort.StackSelector
{
    public class StackCompleter : IDisposable
    {
        public event Action<int> StackCompleted;
        public event Action HexagonDeleteAnimationCompleted;
        
        private readonly IStackSelectionLogic _stackSelectionLogic;
        private readonly IInputService _input;

        public StackCompleter(IInputService input, IStackSelectionLogic stackSelectionLogic, IStackCompletionLogic stackCompletionLogic)
        {
            _stackSelectionLogic = stackSelectionLogic;
            _input = input;
            
            SubscribeUpdates();
        }

        public void Dispose() => 
            CleanUp();

        private void SubscribeUpdates()
        {
            _input.CursorDown += OnCursorDown;
            
        }

        private void OnCursorDown()
        {
            if (_stackSelectionLogic.TrySelectStackOnGrid(ClickedRay, out HexagonStack stack,out GridCell cell))
            {
                
            }
        }

        private Ray ClickedRay => RaycastUtils.GetClickedRay(_input.CursorPosition);

        private void CleanUp()
        {
            
        }
    }
}