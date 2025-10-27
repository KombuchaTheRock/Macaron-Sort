using System;
using Sources.Common.CodeBase.Infrastructure.Utilities;
using Sources.Common.CodeBase.Services.InputService;
using Sources.Features.HexagonSort.GridSystem.GridGenerator.Scripts;
using Sources.Features.HexagonSort.HexagonStackSystem.Scripts;
using UnityEngine;
using Zenject;

namespace Sources.Features.HexagonSort.HexagonStackSystem.StackMover.Scripts
{
    public class StackMover : ITickable, IDisposable, IStackMover
    {
        public event Action DragStarted;
        public event Action DragFinished;
        public event Action<GridCell> StackPlaced;

        private HexagonStack _currentStack;

        private IInputService _input;
        private IStackDraggingLogic _draggingLogic;
        private IStackSelectionLogic _selectionLogic;
        private IStackPlacementLogic _placementLogic;

        private HexagonStack _swapStack;
        private GridCell _lastCellUnderStack;

        public GridCell InitialCell { get; private set; }
        public bool IsDragging { get; private set; }

        private bool CanDrag => _input.IsCursorHold && _currentStack != null;
        private Ray CheckingRay => RaycastUtils.GetClickedRay(_input.CursorPosition);

        private bool _onGridSelectionEnabled;
        private bool _isActive;

        public StackMover(IInputService inputService,
            IStackDraggingLogic draggingLogic,
            IStackSelectionLogic selectionLogic,
            IStackPlacementLogic placementLogic)
        {
            _input = inputService;
            _draggingLogic = draggingLogic;
            _selectionLogic = selectionLogic;
            _placementLogic = placementLogic;
            _onGridSelectionEnabled = false;
            _isActive = true;

            SubscribeUpdates();
        }

        public void Activate() =>
            _isActive = true;

        public void Deactivate() =>
            _isActive = false;

        public void ActivateOnGridSelection() =>
            _onGridSelectionEnabled = true;

        public void DeactivateOnGridSelection() =>
            _onGridSelectionEnabled = false;

        public void Dispose() =>
            CleanUp();

        public void Tick()
        {
            if (_isActive == false)
                return;

            if (CanDrag)
            {
                _draggingLogic.Drag(_currentStack, CheckingRay);

                if (_onGridSelectionEnabled && _draggingLogic.CellUnderStack != null)
                    SwapStacks();
            }
        }

        private void SwapStacks()
        {
            if (_draggingLogic.CellUnderStack == InitialCell)
                return;

            if (_lastCellUnderStack != null && _lastCellUnderStack != _draggingLogic.CellUnderStack)
                SwapBack();

            if (_draggingLogic.CellUnderStack.IsOccupied == false)
                return;

            Debug.Log("Swap");
            
            _lastCellUnderStack = _draggingLogic.CellUnderStack;
            _swapStack = _draggingLogic.CellUnderStack.Stack;

            _draggingLogic.CellUnderStack.FreeCell();
            _placementLogic.PlaceOnGrid(_swapStack, InitialCell);
        }

        private void SwapBack()
        {
            Debug.Log("SwapBack");
            
            InitialCell.FreeCell();
            _placementLogic.PlaceOnGrid(_swapStack, _lastCellUnderStack);

            _lastCellUnderStack = null;
            _swapStack = null;
        }

        private void OnCursorUp()
        {
            if (_isActive == false)
                return;

            if (_currentStack == null)
                return;

            GridCell targetCell = _draggingLogic.GetTargetCell();

            if (targetCell is { IsOccupied: false, IsLocked: false })
            {
                _placementLogic.PlaceOnGrid(_currentStack, targetCell);

                StackPlaced?.Invoke(targetCell);

                if (InitialCell != null && _lastCellUnderStack == null) 
                    InitialCell.FreeCell();
            }
            else
            {
                if (_onGridSelectionEnabled)
                {
                    if (_lastCellUnderStack != null && _lastCellUnderStack != _draggingLogic.CellUnderStack)
                        SwapBack();

                    _placementLogic.PlaceOnGrid(_currentStack, InitialCell);
                }
                else
                    _placementLogic.ReturnToInitialPosition(_currentStack, _currentStack.InitialPosition);
            }

            _currentStack = null;
            _lastCellUnderStack = null;
            _swapStack = null;
            InitialCell = null;
            
            _draggingLogic.ResetCell();
            _selectionLogic.ResetSelection();

            IsDragging = false;
            DragFinished?.Invoke();
        }

        private void OnCursorDown()
        {
            if (_isActive == false)
                return;

            if (TrySelectStack(out HexagonStack stack, out GridCell cell) == false)
                return;

            stack.SetInitialPosition(stack.transform.position);
            _currentStack = stack;

            if (_onGridSelectionEnabled)
                InitialCell = cell;

            IsDragging = true;
            DragStarted?.Invoke();
        }

        private bool TrySelectStack(out HexagonStack stack, out GridCell cell)
        {
            cell = null;

            return _onGridSelectionEnabled
                ? _selectionLogic.TrySelectStackOnGrid(CheckingRay, out stack, out cell)
                : _selectionLogic.TrySelectFreeStack(CheckingRay, out stack);
        }

        private void SubscribeUpdates()
        {
            _input.CursorDown += OnCursorDown;
            _input.CursorUp += OnCursorUp;
        }

        private void CleanUp()
        {
            _input.CursorDown -= OnCursorDown;
            _input.CursorUp -= OnCursorUp;
        }
    }
}