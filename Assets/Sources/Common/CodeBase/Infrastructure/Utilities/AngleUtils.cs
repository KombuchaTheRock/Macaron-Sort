using UnityEngine;

namespace Sources.Common.CodeBase.Infrastructure.Utilities
{
    public static class AngleUtils
    {
        public static float GetAngleDifference(float currentAngle, float targetSnapAngle) =>
            Mathf.Abs(Mathf.DeltaAngle(currentAngle, targetSnapAngle));

        public static float FindClosestSnapAngle(float currentAngle, float snapAngle)
        {
            int snapIndex = Mathf.RoundToInt(currentAngle / snapAngle);
            float targetSnapAngle = snapIndex * snapAngle;

            return targetSnapAngle;
        }

        public static bool IsAngleReached(float currentAngle, float targetAngle) =>
            Mathf.Abs(Mathf.DeltaAngle(currentAngle, targetAngle)) < 0.1f;
        
        public static float NormalizeAngle(float angle)
        {
            angle %= 360f;
            if (angle < 0f)
                angle += 360f;
            return angle;
        }
    }
}