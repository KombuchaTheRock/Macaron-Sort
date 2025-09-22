using System;
using UnityEngine;

namespace Sources.Features.HexagonSort.GridSystem.Scripts
{
    public class RotationWithSnappingLogic
    {
        private const float RotationDirection = -1;
        
        private readonly float _snapAngle;
        private readonly float _snapThreshold;

        private Vector2 _previousCursorPosition;
        private float _currentAngle;
        private float _targetAngle;
        private float _previousAngle;
        private float _rotationSensitivity;
        private bool _isRotating;

        public event Action<float> OnAngleChanged;
        public event Action<float> SnapToNextAngle;
        public event Action<float> ReturnToPreviousAngle;

        public float CurrentAngle => _currentAngle;

        public RotationWithSnappingLogic(float rotationSensitivity, float snapAngle = 60f,
            float snapThreshold = 15f)
        {
            _snapAngle = snapAngle;
            _snapThreshold = snapThreshold;
            _rotationSensitivity = rotationSensitivity;
        }

        public void ActivateRotation(Vector2 cursorPosition)
        {
            _isRotating = true;
            _previousCursorPosition = cursorPosition;
            _previousAngle = _currentAngle;
        }

        public void ActivateSnapping() => 
            Snapping();

        public void Update(Vector2 cursorPosition)
        {
            if (_isRotating)
                FreeRotation(cursorPosition);
        }

        private void FreeRotation(Vector2 cursorPosition)
        {
            Vector2 delta = cursorPosition - _previousCursorPosition;
            float angleDelta = delta.x * _rotationSensitivity * Time.fixedDeltaTime * RotationDirection;
            
            _currentAngle += angleDelta;
            _currentAngle = NormalizeAngle(_currentAngle);

            _previousCursorPosition = cursorPosition;

            OnAngleChanged?.Invoke(_currentAngle);
        }

        private void Snapping()
        {
            _isRotating = false;

            float targetSnapAngle = FindClosestSnapAngle();
            float angleDiff = GetAngleDifference(targetSnapAngle);

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

        private float GetAngleDifference(float targetSnapAngle)
        {
            return Mathf.Abs(Mathf.DeltaAngle(_currentAngle, targetSnapAngle));
        }

        private float FindClosestSnapAngle()
        {
            int snapIndex = Mathf.RoundToInt(_currentAngle / _snapAngle);
            float targetSnapAngle = snapIndex * _snapAngle;

            return targetSnapAngle;
        }

        private float NormalizeAngle(float angle)
        {
            angle %= 360f;
            if (angle < 0f)
                angle += 360f;
            return angle;
        }
    }
}