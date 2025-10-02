using System;
using Sources.Common.CodeBase.Infrastructure.Utilities;
using UnityEngine;

namespace Sources.Features.HexagonSort.GridSystem.GridRotator.Scripts
{
    public class RotationWithSnappingLogic
    {
        public event Action<float> OnAngleChanged;
        public event Action<float> SnapToNextAngle;
        public event Action<float> ReturnToPreviousAngle;

        private Vector2 _previousCursorPosition;
        private float _targetAngle;
        private float _previousAngle;
        private float _rotationSensitivity;

        private readonly float _snapThreshold;
        private readonly float _snapAngle;
        private readonly int _rotationDirection;

        public float CurrentAngle { get; private set; }

        public RotationWithSnappingLogic(float rotationSensitivity, float snapAngle,
            float snapThreshold, bool clockwiseRotation)
        {
            _snapAngle = snapAngle;
            _snapThreshold = snapThreshold;
            _rotationSensitivity = rotationSensitivity;
            _rotationDirection = clockwiseRotation ? -1 : 1;
        }

        public void ActivateRotation(Vector2 cursorPosition)
        {
            _previousCursorPosition = cursorPosition;
            _previousAngle = CurrentAngle;
        }

        public void ActivateSnapping() => 
            Snapping();

        public void RotateToCursor(Vector2 cursorPosition)
        {
            Vector2 delta = cursorPosition - _previousCursorPosition;
            float angleDelta = delta.x * _rotationSensitivity * Time.fixedDeltaTime * _rotationDirection;
            
            CurrentAngle += angleDelta;
            CurrentAngle = AngleUtils.NormalizeAngle(CurrentAngle);

            _previousCursorPosition = cursorPosition;

            OnAngleChanged?.Invoke(CurrentAngle);
        }

        private void Snapping()
        {
            float targetSnapAngle = AngleUtils.FindClosestSnapAngle(CurrentAngle, _snapAngle);
            float angleDiff = AngleUtils.GetAngleDifference(CurrentAngle, targetSnapAngle);

            if (angleDiff <= _snapThreshold)
            {
                CurrentAngle = targetSnapAngle;
                SnapToNextAngle?.Invoke(CurrentAngle);
            }
            else
            {
                CurrentAngle = _previousAngle;
                ReturnToPreviousAngle?.Invoke(CurrentAngle);
            }
        }
    }
}