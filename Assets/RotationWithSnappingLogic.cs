using System;
using Sources.Common.CodeBase.Services;
using UnityEngine;

public class RotationWithSnappingLogic
{
    private readonly IInputService _input;

    private readonly float _snapAngle;
    private readonly float _snapThreshold;

    private float _currentAngle;
    private float _targetAngle;
    private float _previousAngle;
    private bool _isRotating;
    private Vector2 _previousCursorPosition;
    private float _rotationSensitivity;

    public event Action<float> OnAngleChanged;
    public event Action<float> SnapToNextAngle;
    public event Action<float> ReturnToPreviousAngle;

    public RotationWithSnappingLogic(IInputService input, float rotationSensitivity, float snapAngle = 60f,
        float snapThreshold = 15f)
    {
        _input = input;
        _snapAngle = snapAngle;
        _snapThreshold = snapThreshold;
        _rotationSensitivity = rotationSensitivity;

        _input.CursorDown += OnCursorDown;
        _input.CursorUp += OnCursorUp;
    }

    public void Update()
    {
        if (_isRotating && _input.IsCursorHold)
            HandleRotation();
        else if (_isRotating)
            Snap();
    }

    private void OnCursorDown()
    {
        _isRotating = true;
        _previousCursorPosition = _input.CursorPosition;
        _previousAngle = _currentAngle;
    }

    private void OnCursorUp()
    {
        if (_isRotating)
            Snap();
    }

    private void HandleRotation()
    {
        Vector2 currentCursorPos = _input.CursorPosition;
        Vector2 delta = currentCursorPos - _previousCursorPosition;

        float angleDelta = delta.x * _rotationSensitivity;

        _currentAngle += angleDelta;
        _currentAngle = NormalizeAngle(_currentAngle);

        _previousCursorPosition = currentCursorPos;

        OnAngleChanged?.Invoke(_currentAngle);
    }

    private void Snap()
    {
        _isRotating = false;

        int snapIndex = Mathf.RoundToInt(_currentAngle / _snapAngle);
        float targetSnapAngle = snapIndex * _snapAngle;

        float angleDiff = Mathf.Abs(Mathf.DeltaAngle(_currentAngle, targetSnapAngle));

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

    private float NormalizeAngle(float angle)
    {
        angle %= 360f;
        if (angle < 0f)
            angle += 360f;
        return angle;
    }

    public float GetCurrentAngle() => _currentAngle;

    public void Dispose()
    {
        if (_input != null)
        {
            _input.CursorDown -= OnCursorDown;
            _input.CursorUp -= OnCursorUp;
        }
    }
}