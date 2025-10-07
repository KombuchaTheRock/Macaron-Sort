using UnityEngine;

namespace Sources.Common.CodeBase.Infrastructure.Utilities
{
    public static class RaycastUtils
    {
        public static Ray GetClickedRay(Vector2 cursorPosition) =>
            Camera.main.ScreenPointToRay(cursorPosition);
        
        public static int GetLayerBy(Ray groundCheckRay, out RaycastHit hit, float maxDistance, LayerMask mask)
        {
            Physics.Raycast(groundCheckRay, out RaycastHit raycastHit, maxDistance, mask);
            hit = raycastHit;

            return hit.transform.gameObject.layer;
        }
    }
}