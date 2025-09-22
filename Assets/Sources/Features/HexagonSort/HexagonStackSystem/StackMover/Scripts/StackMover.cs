using System;
using Sources.Common.CodeBase.Services;
using Sources.Features.HexagonSort.GridSystem.GridGenerator.Scripts;
using UnityEngine;
using Zenject;

namespace Sources.Features.HexagonSort.HexagonStackSystem.StackMover.Scripts
{
    public class StackMover : MonoBehaviour
    {
        public event Action DragStarted;
        public event Action DragFinished;
        public event Action<GridCell> StackPlaced;

        private StackGenerator.Scripts.HexagonStack _currentStack;

        public int StacksOnGridCount { get; private set; } 
        
        private IInputService _input;
        private IStackDraggingLogic _draggingLogic;
        private IStackSelectionLogic _selectionLogic;
        private IStackPlacementLogic _placementLogic;

        public bool IsDragging { get; private set; }

        private bool CanDrag => _input.IsCursorHold && _currentStack != null;

        [Inject]
        public void Construct(IInputService inputService,
            IStackDraggingLogic draggingLogic,
            IStackSelectionLogic selectionLogic,
            IStackPlacementLogic placementLogic)
        {
            _input = inputService;
            _draggingLogic = draggingLogic;
            _selectionLogic = selectionLogic;
            _placementLogic = placementLogic;

            _input.CursorDown += OnCursorDown;
            _input.CursorUp += OnCursorUp;
        }

        public void ResetStacksOnGridCount() => 
            StacksOnGridCount = 0;

        private void Update()
        {
            if (CanDrag)
                _draggingLogic.Drag(_currentStack, GetClickedRay());
        }

        private void OnCursorUp()
        {
            if (_currentStack == null)
                return;

            GridCell targetCell = _draggingLogic.GetTargetCell();

            if (targetCell?.IsOccupied == false)
            {
                _placementLogic.PlaceOnGrid(_currentStack, targetCell);
                ++StacksOnGridCount;
                
                StackPlaced?.Invoke(targetCell);
            }
            else
                _placementLogic.ReturnToInitialPosition(_currentStack, _currentStack.InitialPosition);

            _currentStack = null;

            _draggingLogic.ResetCell();
            _selectionLogic.ResetSelection();

            IsDragging = false;
            DragFinished?.Invoke();
        }

        private void OnCursorDown()
        {
            if (_selectionLogic.TrySelectStack(GetClickedRay(), out StackGenerator.Scripts.HexagonStack stack))
            {
                _currentStack = stack;

                IsDragging = true;
                DragStarted?.Invoke();
            }
        }

        private Ray GetClickedRay() =>
            Camera.main.ScreenPointToRay(_input.CursorPosition);

        private void OnDestroy() =>
            UnsubscribeFromInput();

        private void UnsubscribeFromInput()
        {
            _input.CursorDown -= OnCursorDown;
            _input.CursorUp -= OnCursorUp;
        }
    }
}