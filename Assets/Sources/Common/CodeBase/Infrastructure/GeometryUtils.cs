using UnityEngine;

namespace Sources.Common.CodeBase.Infrastructure
{
    public static class GeometryUtils
    {
        public static float InradiusFromCircumradius(float circumRadius) => 
            circumRadius * Mathf.Cos(Mathf.Deg2Rad * 30f);
    }
}