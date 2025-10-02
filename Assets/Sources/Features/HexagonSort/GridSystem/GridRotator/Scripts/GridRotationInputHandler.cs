using Sources.Common.CodeBase.Services.InputService;

namespace Sources.Features.HexagonSort.GridSystem.GridRotator.Scripts
{
    public class GridRotationInputHandler
    {
        private readonly GridRotationLogic _gridRotation;
        private readonly IInputService _input;

        public bool IsRotating { get; private set; }

        public GridRotationInputHandler(GridRotationLogic gridRotation, IInputService input)
        {
            _gridRotation = gridRotation;
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
            _gridRotation.ActivateRotation(_input.CursorPosition);
        }

        private void OnCursorUp()
        {
            IsRotating = false;
            _gridRotation.ActivateSnapping();
        }
    }
}