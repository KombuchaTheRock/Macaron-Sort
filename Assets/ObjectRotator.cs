using Sources.Common.CodeBase.Services;
using UnityEngine;
using Zenject;

public class ObjectRotator : MonoBehaviour
{
 [Header("Snapping Settings")] 
    [SerializeField] private float _snapAngle = 60f;
    [SerializeField] private float _snapThreshold = 15f;
    [SerializeField] private float _rotationSpeed = 5f;
    [SerializeField] private float _returnSpeed = 8f;
    [SerializeField] private float _rotationSensitivity = 0.07f;

    private RotationWithSnappingLogic _rotation;
    private IInputService _input;
    private float _targetAngle;
    private float _currentVisualAngle;
    private bool _isSnapping;
    private bool _isReturning;

    [Inject]
    private void Construct(IInputService input)
    {
        _input = input;
        _rotation = new RotationWithSnappingLogic(_input, _rotationSensitivity, _snapAngle, _snapThreshold);
        _rotation.OnAngleChanged += HandleAngleChanged;
        _rotation.SnapToNextAngle += OnSnapToNextAngle;
        _rotation.ReturnToPreviousAngle += OnSnapToPreviousAngle;
    }

    private void Update()
    {
        _rotation.Update();
        
        HandleSmoothRotation();
    }

    private void HandleAngleChanged(float newAngle)
    {
        _targetAngle = newAngle;
        
        if (_isSnapping == false && _isReturning == false)
        {
            _currentVisualAngle = _targetAngle;
            transform.rotation = Quaternion.Euler(0f, _currentVisualAngle, 0f);
        }
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

    private void HandleSmoothRotation()
    {
        if (_isSnapping)
            SmoothSnapToTarget();
        else if (_isReturning) 
            SmoothReturnToPrevious();
    }

    private void SmoothSnapToTarget()
    {
        _currentVisualAngle = Mathf.LerpAngle(_currentVisualAngle, _targetAngle, _rotationSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Euler(0f, _currentVisualAngle, 0f);

        if (Mathf.Abs(Mathf.DeltaAngle(_currentVisualAngle, _targetAngle)) < 0.1f)
        {
            _currentVisualAngle = _targetAngle;
            transform.rotation = Quaternion.Euler(0f, _targetAngle, 0f);
            _isSnapping = false;
        }
    }

    private void SmoothReturnToPrevious()
    {
        _currentVisualAngle = Mathf.LerpAngle(_currentVisualAngle, _targetAngle, _returnSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Euler(0f, _currentVisualAngle, 0f);

        if (Mathf.Abs(Mathf.DeltaAngle(_currentVisualAngle, _targetAngle)) < 0.1f)
        {
            _currentVisualAngle = _targetAngle;
            transform.rotation = Quaternion.Euler(0f, _targetAngle, 0f);
            _isReturning = false;
        }
    }

    private void OnDestroy()
    {
        if (_rotation != null)
        {
            _rotation.OnAngleChanged -= HandleAngleChanged;
            _rotation.SnapToNextAngle -= OnSnapToNextAngle;
            _rotation.ReturnToPreviousAngle -= OnSnapToPreviousAngle;
            _rotation.Dispose();
        }
    }
}