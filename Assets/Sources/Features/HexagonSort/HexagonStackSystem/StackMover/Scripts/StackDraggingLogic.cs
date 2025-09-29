using Sources.Common.CodeBase.Services;
using Sources.Features.HexagonSort.GridSystem.GridGenerator.Scripts;
using Sources.Features.HexagonSort.HexagonStackSystem.Scripts;
using UnityEngine;
using Zenject;

namespace Sources.Features.HexagonSort.HexagonStackSystem.StackMover.Scripts
{
    public class StackDraggingLogic : IStackDraggingLogic, IInitializable
    {
        private readonly IStaticDataService _staticData;
        
        private Vector3 _dragHorizontalOffset;
        private LayerMask _draggingLayerMask;
        private GroundChecker _groundChecker;
        private StackMoverConfig _config;

        private GridCell _lastCellUnderCursor;

        public StackDraggingLogic(IStaticDataService staticData) => 
            _staticData = staticData;

        public void Initialize()
        {
            _config = _staticData.GameConfig.StackMoverConfig;
            
            _dragHorizontalOffset = Vector3.forward * _config.DraggingHorizontalOffset;
            _draggingLayerMask = (1 << _config.GroundLayer) | (1 << _config.GridLayer);
            
            _groundChecker = new GroundChecker(_config.MaxRaycastDistance, _draggingLayerMask);
        }

        public void Drag(HexagonStack stack, Ray ray)
        {
            ray.origin += _dragHorizontalOffset;
            
            if (Physics.Raycast(ray, out RaycastHit hit, _config.MaxRaycastDistance, _draggingLayerMask))
            {
                Vector3 targetPosition = hit.point + Vector3.up * _config.DraggingVerticalOffset;
                stack.FollowTarget(targetPosition, _config.DraggingSpeed);

                Ray groundCheckRay = new(stack.transform.position, Vector3.down);
                CheckLayerBy(groundCheckRay);
            }
        }

        private void CheckLayerBy(Ray groundCheckRay)
        {
            int layer = _groundChecker.GetLayerBy(groundCheckRay, out RaycastHit hit);

            if (layer == _config.GroundLayer)
            {
                if (_lastCellUnderCursor == null)
                    return;

                _lastCellUnderCursor.DisableHighlight();
                _lastCellUnderCursor = null;
            }
            else if (layer == _config.GridLayer)
            {
                GridCell cell = hit.collider.GetComponent<GridCell>();
                PickCell(cell);
            }
        }

        private void PickCell(GridCell cell)
        {
            if (cell.IsOccupied)
                return;

            if (_lastCellUnderCursor != null && _lastCellUnderCursor == cell)
                return;

            cell.EnableHighlight();

            _lastCellUnderCursor?.DisableHighlight();
            _lastCellUnderCursor = cell;
        }

        public GridCell GetTargetCell() => 
            _lastCellUnderCursor;
        
        public void ResetCell() => 
            _lastCellUnderCursor = null;
    }
}