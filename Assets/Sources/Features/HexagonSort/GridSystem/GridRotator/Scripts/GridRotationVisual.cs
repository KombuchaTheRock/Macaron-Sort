using Sources.Common.CodeBase.Infrastructure.Utilities;
using UnityEngine;

namespace Sources.Features.HexagonSort.GridSystem.GridRotator.Scripts
{
  public class GridRotationVisual
    {
        private readonly Transform _transform;
        
        private RotationWithSnappingLogic _rotation;
        private GridRotationConfig _config;
        
        private float _targetAngle;
        private float _currentVisualAngle;
        private bool _isSnapping;
        private bool _isReturning;

        public GridRotationVisual(Transform transform, RotationWithSnappingLogic rotation, GridRotationConfig config)
        {
            _transform = transform;
            _rotation = rotation;
            _config = config;
            
            SubscribeUpdates();
        }

        public void CleanUp()
        {
            _rotation.OnAngleChanged -= OnAngleChanged;
            _rotation.SnapToNextAngle -= OnSnapToNextAngle;
            _rotation.ReturnToPreviousAngle -= OnSnapToPreviousAngle;
        }

        private void SubscribeUpdates()
        {
            _rotation.OnAngleChanged += OnAngleChanged;
            _rotation.SnapToNextAngle += OnSnapToNextAngle;
            _rotation.ReturnToPreviousAngle += OnSnapToPreviousAngle;
        }

        public void HandleSnappingRotation()
        {
            if (_isSnapping || _isReturning)
            {
                if (_isSnapping)
                    SmoothSnapToTarget();
                else if (_isReturning)
                    SmoothReturnToPrevious();
            }
        }

        public void ApplyTargetAngleRotation()
        {
            _currentVisualAngle = _targetAngle;
            _transform.rotation = Quaternion.Euler(0f, _currentVisualAngle, 0f);
        }

        private void OnAngleChanged(float newAngle)
        {
            _targetAngle = newAngle;

            if (_isSnapping == false && _isReturning == false)
                ApplyTargetAngleRotation();
        }

        private void OnSnapToNextAngle(float snapAngle)
        {
            _isSnapping = true;
            _isReturning = false;
            _targetAngle = snapAngle;
        }

        private void OnSnapToPreviousAngle(float snapAngle)
        {
            _isReturning = true;
            _isSnapping = false;
            _targetAngle = snapAngle;
        }

        private void SmoothSnapToTarget()
        {
            RotateToTarget(_config.SnapSpeed);

            if (AngleUtils.IsAngleReached(_currentVisualAngle, _targetAngle))
            {
                ApplyTargetAngleRotation();
                _isSnapping = false;
            }
        }

        private void SmoothReturnToPrevious()
        {
            RotateToTarget(_config.ReturnSpeed);

            if (AngleUtils.IsAngleReached(_currentVisualAngle, _targetAngle))
            {
                ApplyTargetAngleRotation();
                _isReturning = false;
            }
        }

        private void RotateToTarget(float speed)
        {
            _currentVisualAngle = Mathf.LerpAngle(_currentVisualAngle, _targetAngle, speed * Time.deltaTime);
            _transform.rotation = Quaternion.Euler(0f, _currentVisualAngle, 0f);
        }
    }
}