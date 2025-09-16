using Sources.Common.CodeBase.Services;
using Sources.Features.HexagonSort.GridGenerator.Scripts;
using Sources.Features.HexagonSort.StackGenerator.Scripts;
using UnityEngine;
using Zenject;

namespace Sources.Features.HexagonSort.StackMover.Scripts
{
    public class StackMover : MonoBehaviour
    {
        [SerializeField] private Vector3 _draggingOffset;
        
        private HexagonStack _currentStack;
        private Vector3 _currentStackInitialPosition;

        private IInputService _input;
        private IStaticDataService _staticData;
        
        private StackSelection _selection;
        private StackDragging _dragging;
        private StackPlacement _placement;

        [Inject]
        public void Construct(IInputService inputService, IStaticDataService staticData)
        {
            _input = inputService;
            _staticData = staticData;
            _input.CursorDown += OnCursorDown;
            _input.CursorUp += OnCursorUp;
        }

        private void Start() => 
            InitializeServices();

        private void InitializeServices()
        {
            StackMoverConfig config = _staticData.GameConfig.StackMoverConfig;
            
            _selection = new StackSelection(config.MaxRaycastDistance, config.HexagonLayer);
            _dragging = new StackDragging(config.MaxRaycastDistance,
                config.DragVerticalOffset,
                config.GridLayer,
                config.GroundLayer);
            _placement = new StackPlacement(config.PlaceOffsetAboveGrid);
        }

        private void Update()
        {
            if (_input.IsCursorHold && _currentStack != null)
            {
                Ray draggingRay = GetClickedRay();
                draggingRay.origin += _draggingOffset;
                
                _dragging.Drag(_currentStack, draggingRay);
            }
        }

        private void OnCursorUp()
        {
            if (_currentStack == null)
                return;

            GridCell targetCell = _dragging.GetTargetCell();
            
            if (targetCell?.IsOccupied == false)
                _placement.PlaceOnGrid(_currentStack, targetCell);
            else
                _placement.ReturnToInitialPosition(_currentStack, _currentStackInitialPosition, 4);

            _currentStack = null;
            _dragging.ResetCell();
            _selection.ResetSelection();
        }

        private void OnCursorDown()
        {
            if (_selection.TrySelectStack(GetClickedRay(), out HexagonStack stack))
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