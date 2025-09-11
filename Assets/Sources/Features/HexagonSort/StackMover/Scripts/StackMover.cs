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

        private int _groundLayer;
        private int _gridLayer;
        private LayerMask _hexagonLayerMask;
        private LayerMask _draggingLayerMask;

        private HexagonStack _currentStack;
        private Vector3 _currentStackInitialPosition;

        private IInputService _input;
        private bool _initialized;

        private Vector3 _hitPosition;

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

            _initialized = true;
        }

        private void OnDestroy()
        {
            _input.CursorDown -= OnCursorDown;
            _input.CursorUp -= OnCursorUp;
        }

        private void Update()
        {
            if (_initialized == false)
                return;

            if (_input.IsCursorHold && _currentStack != null)
            {
                DragStack();
            }
        }

        private void OnDrawGizmos()
        {
            if (_currentStack != null)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawSphere(_hitPosition, 0.05f);
            }
        }

        private void DragStack()
        {
            Physics.Raycast(GetClickedRay(), out RaycastHit hit, MaxRaycastDistance, _draggingLayerMask);
            
            if (hit.collider == null)
                return;

            _hitPosition = hit.point;

            int layer = hit.collider.gameObject.layer;

            Vector3 targetPosition = hit.point + Vector3.up * _dragVerticalOffset;
            MoveStack(targetPosition);

            Debug.Log(hit.collider.gameObject.name + " Layer: " + layer);

            if (layer == _groundLayer)
            {
            }
            else if (layer == _gridLayer)
            {
                SnapToGrid(hit);
            }
        }

        private void SnapToGrid(RaycastHit hit)
        {
            GridCell gridCell = hit.collider.GetComponent<GridCell>();

            _currentStack.transform.position = gridCell.transform.position + Vector3.up * _dragVerticalOffset;
        }


        private void OnCursorUp()
        {
            if (_currentStack == null)
                return;

            _currentStack.transform.position = _currentStackInitialPosition;
            _currentStack = null;
        }

        private void OnCursorDown()
        {
            Physics.Raycast(GetClickedRay(), out RaycastHit hit, MaxRaycastDistance, _hexagonLayerMask);

            if (hit.collider == null)
                return;

            if (hit.collider.TryGetComponent(out Hexagon hexagon) == false)
                return;

            _currentStack = hexagon.Stack;
            _currentStackInitialPosition = _currentStack.transform.position;
        }

        private void MoveStack(Vector3 targetPosition)
        {
            _currentStack.transform.position =
                Vector3.MoveTowards(_currentStack.transform.position, targetPosition, Time.deltaTime * 30f);
        }

        private Ray GetClickedRay() =>
            Camera.main.ScreenPointToRay(_input.CursorPosition);
    }
}