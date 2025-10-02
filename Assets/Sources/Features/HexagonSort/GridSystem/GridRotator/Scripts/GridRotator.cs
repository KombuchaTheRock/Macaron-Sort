using Sources.Common.CodeBase.Services.InputService;
using UnityEngine;
using Zenject;

namespace Sources.Features.HexagonSort.GridSystem.GridRotator.Scripts
{
    public class GridRotator : MonoBehaviour
    {
        private IInputService _input;

        private GridRotationConfig _config;
        private GridRotationLogic _gridRotation;
        private GridResize _gridResize;
        private GridRotationVisual _gridRotationVisual;
        private float _targetAngle;
        private float _currentVisualAngle;
        private bool _isSnapping;
        private bool _isReturning;
        private bool _isRotating;
        private bool CanRotate => _input.IsCursorHold && _isRotating;

        [Inject]
        private void Construct(IInputService input) =>
            _input = input;

        public void Initialize(GridRotationConfig config)
        {
            _config = config;
            _gridRotation = new GridRotationLogic(_config.RotationSensitivity,
                _config.SnapAnchorAngle,
                _config.SnapThreshold,
                _config.ClockwiseRotation);

            _gridResize = new GridResize(transform, _config);
            _gridRotationVisual = new GridRotationVisual(transform, _gridRotation, _config);
            
            SubscribeUpdates();
        }

        private void Update()
        {
            _gridRotationVisual.HandleSnappingRotation();
            _gridResize.HandleGridResizing(_input.IsCursorHold, _gridRotation.CurrentAngle);
        }

        private void FixedUpdate()
        {
            if (CanRotate) 
                _gridRotation.RotateToCursor(_input.CursorPosition);
        }

        private void OnDisable()
        {
            _gridRotationVisual.ApplyTargetAngleRotation();
            _gridResize.ResetScale();
        }

        private void OnDestroy() =>
            CleanUp();

        private void OnCursorDown()
        {
             _isRotating = true;
            _gridRotation.ActivateRotation(_input.CursorPosition);
        }

        private void OnCursorUp()
        {
            _isRotating = false;
            _gridRotation.ActivateSnapping();
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