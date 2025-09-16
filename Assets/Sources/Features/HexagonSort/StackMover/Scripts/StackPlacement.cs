using Sources.Features.HexagonSort.GridGenerator.Scripts;
using Sources.Features.HexagonSort.StackGenerator.Scripts;
using UnityEngine;

namespace Sources.Features.HexagonSort.StackMover.Scripts
{
    public class StackPlacement
    {
        private readonly float _placeOffsetAboveGrid;

        public StackPlacement(float placeOffsetAboveGrid) => 
            _placeOffsetAboveGrid = placeOffsetAboveGrid;

        public void PlaceOnGrid(HexagonStack stack, GridCell cell)
        {
            cell.SetStack(stack);
            cell.UnHighlight();
            stack.Movement.CanMove = false;
        
            stack.transform.position = cell.transform.position + Vector3.up * _placeOffsetAboveGrid;
        }
    
        public void ReturnToInitialPosition(HexagonStack stack, Vector3 initialPosition, float speed) => 
            stack.Movement.MoveToTarget(initialPosition, speed);
    }
}