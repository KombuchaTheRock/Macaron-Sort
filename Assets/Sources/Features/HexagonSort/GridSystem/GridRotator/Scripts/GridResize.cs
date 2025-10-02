using UnityEngine;

namespace Sources.Features.HexagonSort.GridSystem.GridRotator.Scripts
{
    public class GridResize
    {
        private Vector3 _targetScale = Vector3.one;
        
        private readonly Transform _transform;
        private GridRotationConfig _config;

        public GridResize(Transform transform, GridRotationConfig config)
        {
            _config = config;
            _transform = transform;
        }

        public void HandleGridResizing(bool isCursorHold, float currentAngle)
        {
            if (isCursorHold)
            {
                float angleRemainder = currentAngle % _config.SnapAnchorAngle;
                bool isSnapAngleDeadZone = angleRemainder is > 15 and < 45;

                _targetScale = isSnapAngleDeadZone ? Vector3.one * 0.85f : Vector3.one;
            }
            else
            {
                _targetScale = Vector3.one;
            }

            ResizeGrid(_config.RotationSensitivity);
        }

        public void ResetScale()
        {
            _transform.localScale = Vector3.one;
            _targetScale = Vector3.one;
        }

        private void ResizeGrid(float speed)
        {
            if (_transform.localScale.magnitude <= Vector3.one.magnitude)
            {
                _transform.localScale = Vector3.Lerp(
                    _transform.localScale,
                    _targetScale,
                    speed * Time.deltaTime
                );
            }
        }
    }
}