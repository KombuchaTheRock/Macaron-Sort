using System;
using Sources.Common.CodeBase.Services;
using Sources.Features.HexagonSort.GridGenerator.Scripts;
using Sources.Features.HexagonSort.StackGenerator.Scripts;
using UnityEngine;
using Zenject;

namespace Sources.Features.HexagonSort.StackMover.Scripts
{
    public class StackMover : MonoBehaviour
    {
        public event Action StackPlaced;
        
        private HexagonStack _currentStack;
        private Vector3 _currentStackInitialPosition;

        private IInputService _input;
        private IStackDraggingLogic _draggingLogic;
        private IStackSelectionLogic _selectionLogic;
        private IStackPlacementLogic _placementLogic;
        
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
                StackPlaced?.Invoke();
            }
            else
                _placementLogic.ReturnToInitialPosition(_currentStack, _currentStackInitialPosition);

            _currentStack = null;
            _draggingLogic.ResetCell();
            _selectionLogic.ResetSelection();
        }

        private void OnCursorDown()
        {
            if (_selectionLogic.TrySelectStack(GetClickedRay(), out HexagonStack stack))
            {
                _currentStack = stack;
                _currentStackInitialPosition = stack.transform.position;
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