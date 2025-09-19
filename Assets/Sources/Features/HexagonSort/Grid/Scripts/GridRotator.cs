using System;
using Sources.Common.CodeBase.Services;
using UnityEngine;
using Zenject;

namespace Sources.Features.HexagonSort.Grid.Scripts
{
    public class GridRotator : MonoBehaviour
    {
        private IInputService _input;

        private RotationWithSnappingLogic _rotation;
        private GridRotationConfig _config;
        private float _targetAngle;
        private float _currentVisualAngle;
        private bool _isSnapping;
        private bool _isReturning;

        [Inject]
        private void Construct(IInputService input) =>
            _input = input;

        public void Initialize(GridRotationConfig config)
        {
            _config = config;

            _input.CursorUp += OnCursorUp;
            _input.CursorDown += OnCursorDown;

            _rotation = new RotationWithSnappingLogic(_config.RotationSensitivity, _config.SnapAnchorAngle,
                _config.SnapThreshold);

            _rotation.OnAngleChanged += OnAngleChanged;
            _rotation.SnapToNextAngle += OnSnapToNextAngle;
            _rotation.ReturnToPreviousAngle += OnSnapToPreviousAngle;
        }

        private void Update()
        {
            if (_isSnapping || _isReturning)
                HandleSnappingRotation();
            
            HandleGridResizing(_input.IsCursorHold);
        }

        private void FixedUpdate()
        {
            if (_input.IsCursorHold)
                _rotation.Update(_input.CursorPosition);
        }

        private void HandleGridResizing(bool isRotating)
        {
            if (isRotating)
            {
                float angleRemainder = _rotation.CurrentAngle % _config.SnapAnchorAngle;
                bool isSnapAngleDeadZone = angleRemainder is > 15 and < 45;
                
                if (isSnapAngleDeadZone)
                    ResizingGrid(Vector3.one * 0.85f, _config.RotationSensitivity);
                else
                    ResizingGrid(Vector3.one, _config.RotationSensitivity);
            }
            else
                ResizingGrid(Vector3.one, _config.RotationSensitivity);
        }

        private void ResizingGrid(Vector3 size, float speed)
        {
            if (transform.localScale.magnitude <= Vector3.one.magnitude)
                transform.localScale = Vector3.Lerp(transform.localScale, size, speed * Time.deltaTime);
        }

        private void OnCursorUp() =>
            _rotation.ActivateSnapping();

        private void OnCursorDown() =>
            _rotation.ActivateRotation(_input.CursorPosition);

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

        private void HandleSnappingRotation()
        {
            if (_isSnapping)
                SmoothSnapToTarget();
            else if (_isReturning)
                SmoothReturnToPrevious();
        }

        private void SmoothSnapToTarget()
        {
            RotateToTarget(_config.SnapSpeed);

            if (IsAngleReached() == false)
                return;

            ApplyTargetAngleRotation();
            _isSnapping = false;
        }

        private void SmoothReturnToPrevious()
        {
            RotateToTarget(_config.ReturnSpeed);

            if (IsAngleReached() == false)
                return;

            ApplyTargetAngleRotation();
            _isReturning = false;
        }

        private void ApplyTargetAngleRotation()
        {
            _currentVisualAngle = _targetAngle;
            transform.rotation = Quaternion.Euler(0f, _currentVisualAngle, 0f);
        }

        private void RotateToTarget(float speed)
        {
            _currentVisualAngle = Mathf.LerpAngle(_currentVisualAngle, _targetAngle, speed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(0f, _currentVisualAngle, 0f);
        }

        private bool IsAngleReached() =>
            Mathf.Abs(Mathf.DeltaAngle(_currentVisualAngle, _targetAngle)) < 0.1f;

        private void OnDisable() =>
            ApplyTargetAngleRotation();

        private void OnDestroy()
        {
            UnsubscribeFromRotationLogic();
            UnsubscribeFromInput();
        }

        private void UnsubscribeFromInput()
        {
            _input.CursorUp -= OnCursorUp;
            _input.CursorDown -= OnCursorDown;
        }

        private void UnsubscribeFromRotationLogic()
        {
            _rotation.OnAngleChanged -= OnAngleChanged;
            _rotation.SnapToNextAngle -= OnSnapToNextAngle;
            _rotation.ReturnToPreviousAngle -= OnSnapToPreviousAngle;
        }
    }
}