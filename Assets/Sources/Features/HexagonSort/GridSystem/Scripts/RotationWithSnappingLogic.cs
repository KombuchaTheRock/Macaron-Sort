using System;
using Sources.Common.CodeBase.Infrastructure.Utilities;
using UnityEngine;

namespace Sources.Features.HexagonSort.GridSystem.Scripts
{
    public class RotationWithSnappingLogic
    {
        public event Action<float> OnAngleChanged;
        public event Action<float> SnapToNextAngle;
        public event Action<float> ReturnToPreviousAngle;

        private Vector2 _previousCursorPosition;
        private float _currentAngle;
        private float _targetAngle;
        private float _previousAngle;
        private float _rotationSensitivity;
        private bool _isRotating;
        
        private readonly float _snapThreshold;
        private readonly float _snapAngle;
        private readonly int _rotationDirection;

        public float CurrentAngle => _currentAngle;

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
            _isRotating = true;
            _previousCursorPosition = cursorPosition;
            _previousAngle = _currentAngle;
        }

        public void ActivateSnapping() => 
            Snapping();

        public void UpdateRotation(Vector2 cursorPosition)
        {
            if (_isRotating)
                RotateToCursor(cursorPosition);
        }

        private void RotateToCursor(Vector2 cursorPosition)
        {
            Vector2 delta = cursorPosition - _previousCursorPosition;
            float angleDelta = delta.x * _rotationSensitivity * Time.fixedDeltaTime * _rotationDirection;
            
            _currentAngle += angleDelta;
            _currentAngle = AngleUtils.NormalizeAngle(_currentAngle);

            _previousCursorPosition = cursorPosition;

            OnAngleChanged?.Invoke(_currentAngle);
        }

        private void Snapping()
        {
            _isRotating = false;

            float targetSnapAngle = AngleUtils.FindClosestSnapAngle(_currentAngle, _snapAngle);
            float angleDiff = AngleUtils.GetAngleDifference(_currentAngle, targetSnapAngle);

            if (angleDiff <= _snapThreshold)
            {
                _currentAngle = targetSnapAngle;
                SnapToNextAngle?.Invoke(_currentAngle);
            }
            else
            {
                _currentAngle = _previousAngle;
                ReturnToPreviousAngle?.Invoke(_currentAngle);
            }
        }
    }
}