using UnityEngine;

namespace Sources.Features.HexagonSort.StackMover.Scripts
{
    public class GroundChecker
    {
        private readonly float _maxRaycastDistance;
        private readonly LayerMask _groundLayerMask;

        public GroundChecker( float maxRaycastDistance, LayerMask groundLayerMask)
        {
            _maxRaycastDistance = maxRaycastDistance;
            _groundLayerMask = groundLayerMask;
        }
        
        public int CheckGround(Ray groundCheckRay, out RaycastHit hit)
        {
            Physics.Raycast(groundCheckRay, out RaycastHit raycastHit, _maxRaycastDistance, _groundLayerMask);
            hit = raycastHit;
            
            return hit.transform.gameObject.layer;
        }
    }
}