using Sources.Features.HexagonSort.HexagonTile.Scripts;
using Sources.Features.HexagonSort.StackGenerator.Scripts;
using UnityEngine;

namespace Sources.Features.HexagonSort.StackMover.Scripts
{
    public class StackSelection
    {
        private readonly float _maxRaycastDistance;
        private readonly LayerMask _hexagonLayerMask;

        public StackSelection(float maxRaycastDistance, int hexagonLayer)
        {
            _maxRaycastDistance = maxRaycastDistance;
            _hexagonLayerMask = 1 << hexagonLayer;
        }
        
        public HexagonStack SelectedStack { get; private set; }
        
        public bool TrySelectStack(Ray ray, out HexagonStack stack)
        {
            if (Physics.Raycast(ray, out RaycastHit hit, _maxRaycastDistance, _hexagonLayerMask))
            {
                Hexagon hexagon = hit.collider.GetComponent<Hexagon>();
                if (hexagon.Stack.Movement.CanMove)
                {
                    stack = hexagon.Stack;
                    SelectedStack = stack;
                    
                    return true;
                }
            }
        
            stack = null;
            return false;
        }
        
        public void ResetSelection() => 
            SelectedStack = null;
    }
}