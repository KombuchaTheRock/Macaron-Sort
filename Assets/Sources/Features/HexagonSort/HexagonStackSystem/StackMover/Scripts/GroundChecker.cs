using UnityEngine;

namespace Sources.Features.HexagonSort.HexagonStackSystem.StackMover.Scripts
{
    public class GroundChecker
    {
        private readonly float _maxRaycastDistance;
        private readonly LayerMask _groundLayerMask;

        public GroundChecker(float maxRaycastDistance, LayerMask groundLayerMask)
        {
            _maxRaycastDistance = maxRaycastDistance;
            _groundLayerMask = groundLayerMask;
        }

        public static int GetLayerBy(Ray groundCheckRay, out RaycastHit hit, float maxDistance, LayerMask groundLayerMask)
        {
            Physics.Raycast(groundCheckRay, out RaycastHit raycastHit, maxDistance, groundLayerMask);
            hit = raycastHit;

            return hit.transform.gameObject.layer;
        }
    }
}