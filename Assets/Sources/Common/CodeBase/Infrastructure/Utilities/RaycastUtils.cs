using UnityEngine;

namespace Sources.Common.CodeBase.Infrastructure.Utilities
{
    public static class RaycastUtils
    {
        public static Ray GetClickedRay(Vector2 cursorPosition) =>
            Camera.main.ScreenPointToRay(cursorPosition);
    }
}