using Sources.Common.CodeBase.Services.InputService;
using UnityEngine;
using Zenject;

namespace Sources.Features.HexagonSort.GridSystem.GridRotator.Scripts
{
    public class GridRotator : MonoBehaviour
    {
        private IInputService _input;
        private GridRotationConfig _config;

        private RotationWithSnappingLogic _rotation;
        private GridResizing _gridResizing;
        private GridRotationVisual _gridRotationVisual;
        private float _targetAngle;
        private float _currentVisualAngle;
        private bool _isSnapping;
        private bool _isReturning;
        private bool _isRotating;

        [Inject]
        private void Construct(IInputService input) =>
            _input = input;

        public void Initialize(GridRotationConfig config)
        {
            _config = config;
            _rotation = new RotationWithSnappingLogic(_config.RotationSensitivity,
                _config.SnapAnchorAngle,
                _config.SnapThreshold,
                _config.ClockwiseRotation);

            _gridResizing = new GridResizing(transform, _config);
            _gridRotationVisual = new GridRotationVisual(transform, _rotation, _config);
            
            SubscribeUpdates();
        }

        private void Update()
        {
            _gridRotationVisual.HandleSnappingRotation();
            _gridResizing.HandleGridResizing(_input.IsCursorHold, _rotation.CurrentAngle);
        }

        private void FixedUpdate()
        {
            if (_input.IsCursorHold && _isRotating) 
                _rotation.RotateToCursor(_input.CursorPosition);
        }

        private void OnDisable()
        {
            _gridRotationVisual.ApplyTargetAngleRotation();
            _gridResizing.ResetScale();
        }

        private void OnDestroy() =>
            CleanUp();

        private void OnCursorDown()
        {
             _isRotating = true;
            _rotation.ActivateRotation(_input.CursorPosition);
        }

        private void OnCursorUp()
        {
            _isRotating = false;
            _rotation.ActivateSnapping();
        }

        private void SubscribeUpdates()
        {
            _input.CursorUp += OnCursorUp;
            _input.CursorDown += OnCursorDown;
        }

        private void CleanUp()
        {
            _gridRotationVisual.CleanUp();
            
            _input.CursorUp -= OnCursorUp;
            _input.CursorDown -= OnCursorDown;
        }
    }
}