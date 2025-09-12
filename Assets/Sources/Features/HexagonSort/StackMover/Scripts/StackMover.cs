using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Sources.Common.CodeBase.Services;
using Sources.Features.HexagonSort.GridGenerator.Scripts;
using Sources.Features.HexagonSort.HexagonTile.Scripts;
using Sources.Features.HexagonSort.StackGenerator.Scripts;
using UnityEngine;
using Zenject;

namespace Sources.Features.HexagonSort.StackMover.Scripts
{
    public class StackMover : MonoBehaviour
    {
        private const string HexagonLayerName = "Hexagon";
        private const string HexagonGridLayerName = "Grid";
        private const string GroundLayerName = "Ground";
        private const float MaxRaycastDistance = 300;

        [SerializeField] private float _dragVerticalOffset = 1f;
        [SerializeField] private float _placeOffsetAboveGrid = 0.15f;

        private int _groundLayer;
        private int _gridLayer;
        private LayerMask _hexagonLayerMask;
        private LayerMask _draggingLayerMask;

        private HexagonStack _currentStack;
        private HexagonStack _droppedStack;
        private GridCell _gridCellUnderCursor;
        private Vector3 _currentStackInitialPosition;
        private Vector3 _hitPosition;

        private IInputService _input;

        private TweenerCore<Vector3, Vector3, VectorOptions> _transitionToInitialAnim;

        [Inject]
        public void Construct(IInputService inputService)
        {
            _input = inputService;
            _input.CursorDown += OnCursorDown;
            _input.CursorUp += OnCursorUp;

            Initialize();
        }

        private void Initialize()
        {
            _groundLayer = LayerMask.NameToLayer(GroundLayerName);
            _gridLayer = LayerMask.NameToLayer(HexagonGridLayerName);

            _hexagonLayerMask = 1 << LayerMask.NameToLayer(HexagonLayerName);
            _draggingLayerMask = (1 << _groundLayer) | (1 << _gridLayer);
        }

        private void OnDestroy()
        {
            _input.CursorDown -= OnCursorDown;
            _input.CursorUp -= OnCursorUp;
        }

        private void Update()
        {
            if (_input.IsCursorHold == false || _currentStack == null)
                return;

            if (Physics.Raycast(GetClickedRay(), out RaycastHit hit, MaxRaycastDistance, _draggingLayerMask))
                DragStack(hit);
        }

        private void OnCursorUp()
        {
            if (_currentStack == null)
                return;
            
            if (_gridCellUnderCursor?.IsOccupied == false)
            {
                _gridCellUnderCursor.SetStack(_currentStack);
                _currentStack.SetPlacedOnGridTrue();

                _currentStack.transform.position =
                    _gridCellUnderCursor.transform.position + Vector3.up * _placeOffsetAboveGrid;
            }
            else
                MoveCurrentStackToInitialPosition();

            _currentStack = null;
        }

        private void OnCursorDown()
        {
            if (Physics.Raycast(GetClickedRay(), out RaycastHit hit, MaxRaycastDistance, _hexagonLayerMask))
                SelectStack(hit);
        }

        private void DragStack(RaycastHit hit)
        {
            if (hit.collider == null)
                return;

            _hitPosition = hit.point;
            int layer = hit.collider.gameObject.layer;

            Vector3 targetPosition = hit.point + Vector3.up * _dragVerticalOffset;
            MoveStackToTarget(targetPosition);

            if (layer == _groundLayer)
            {
                _gridCellUnderCursor = null;
                _currentStack.SetPlacedOnGridFalse();
            }
            else if (layer == _gridLayer)
            {
                _gridCellUnderCursor = hit.collider.GetComponent<GridCell>();
                SnappingToGrid(_gridCellUnderCursor);
            }
        }

        private void SnappingToGrid(GridCell gridCell)
        {
            if (gridCell.IsOccupied)
                return;

            Vector3 targetPosition = gridCell.transform.position + Vector3.up * _dragVerticalOffset;
            _currentStack.transform.position = Vector3.Lerp(_currentStack.transform.position, targetPosition,
                Time.deltaTime * 30f);
        }

        private void MoveCurrentStackToInitialPosition()
        {
            float transitionDuration = (_currentStack.transform.position - _currentStackInitialPosition).magnitude / 5;

            _transitionToInitialAnim?.Complete();

            _transitionToInitialAnim = _currentStack.transform
                .DOMove(_currentStackInitialPosition, transitionDuration)
                .SetEase(Ease.OutBounce)
                .Play()
                .OnComplete(OnComplete)
                .SetLink(_currentStack.gameObject);

            _droppedStack = _currentStack;
            return;

            void OnComplete() =>
                _droppedStack = null;
        }//Вынести

        private void SelectStack(RaycastHit hit)
        {
            if (hit.collider == null)
                return;

            Hexagon hexagon = hit.collider.GetComponent<Hexagon>();

            if (hexagon.Stack.IsPlacedOnGrid || hexagon.Stack == _droppedStack)
                return;

            _currentStack = hexagon.Stack;
            _currentStackInitialPosition = _currentStack.transform.position;
        }

        private void MoveStackToTarget(Vector3 targetPosition)
        {
            _currentStack.transform.position =
                Vector3.MoveTowards(_currentStack.transform.position, targetPosition, Time.deltaTime * 30f);
        }

        private Ray GetClickedRay() =>
            Camera.main.ScreenPointToRay(_input.CursorPosition);

        private void OnDrawGizmos()
        {
            if (_currentStack != null)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawSphere(_hitPosition, 0.05f);
            }
        }//Вынести
    }
}