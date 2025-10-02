using Sources.Common.CodeBase.Services.InputService;

namespace Sources.Features.HexagonSort.GridSystem.GridRotator.Scripts
{
    public class GridRotationInputHandler
    {
        private readonly RotationWithSnappingLogic _rotation;
        private readonly IInputService _input;

        public bool IsRotating { get; private set; }

        public GridRotationInputHandler(RotationWithSnappingLogic rotation, IInputService input)
        {
            _rotation = rotation;
            _input = input;
        }

        public void Initialize()
        {
            _input.CursorUp += OnCursorUp;
            _input.CursorDown += OnCursorDown;
        }

        public void CleanUp()
        {
            _input.CursorUp -= OnCursorUp;
            _input.CursorDown -= OnCursorDown;
        }

        private void OnCursorDown()
        {
            IsRotating = true;
            _rotation.ActivateRotation(_input.CursorPosition);
        }

        private void OnCursorUp()
        {
            IsRotating = false;
            _rotation.ActivateSnapping();
        }
    }
}