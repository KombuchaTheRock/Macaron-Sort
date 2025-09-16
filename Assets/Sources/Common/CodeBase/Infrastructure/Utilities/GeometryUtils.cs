using UnityEngine;

namespace Sources.Common.CodeBase.Infrastructure.Utilities
{
    public static class GeometryUtils
    {
        public static float InradiusFromOutRadius(float outRadius) => 
            outRadius * Mathf.Cos(Mathf.Deg2Rad * 30f);
    }
}