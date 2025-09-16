using Sources.Features.HexagonSort.GridGenerator.Scripts;
using Sources.Features.HexagonSort.StackGenerator.Scripts;
using UnityEngine;

namespace Sources.Features.HexagonSort.StackMover.Scripts
{
    public class StackDragging
    {
        private readonly float _dragVerticalOffset;
        private readonly int _gridLayer;
        private readonly int _groundLayer;
        private readonly LayerMask _draggingLayerMask;
        private readonly float _maxRaycastDistance;
    
        private GridCell _lastCellUnderCursor;

        public StackDragging(float maxRaycastDistance, float dragVerticalOffset, int gridLayer, int groundLayer)
        {
            _maxRaycastDistance = maxRaycastDistance;
            _dragVerticalOffset = dragVerticalOffset;
            _gridLayer = gridLayer;
            _groundLayer = groundLayer;
            
            _draggingLayerMask = (1 << _groundLayer) | (1 << _gridLayer);
        }

        public void Drag(HexagonStack stack, Ray ray)
        {
            if (Physics.Raycast(ray, out RaycastHit hit, _maxRaycastDistance, _draggingLayerMask))
            {
                Vector3 targetPosition = hit.point + Vector3.up * _dragVerticalOffset;
                stack.Movement.FollowingTarget(targetPosition);
                
                Ray groundCheckRay = new(stack.transform.position, Vector3.down);
                
                CheckGround(groundCheckRay);
            }
        }
    
        private void CheckGround(Ray groundCheckRay)
        {
            Physics.Raycast(groundCheckRay, out RaycastHit hit, _maxRaycastDistance, _draggingLayerMask);
            
            int layer = hit.transform.gameObject.layer;
            
            if (layer == _groundLayer)
            {
                if (_lastCellUnderCursor == null) 
                    return;
            
                _lastCellUnderCursor.UnHighlight();
                _lastCellUnderCursor = null;
            }
            else if (layer == _gridLayer)
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
            
            cell.Highlight();
            
            _lastCellUnderCursor?.UnHighlight();
            _lastCellUnderCursor = cell;
        }
    
        public GridCell GetTargetCell() => _lastCellUnderCursor;
        public void ResetCell() => _lastCellUnderCursor = null;
    }
}