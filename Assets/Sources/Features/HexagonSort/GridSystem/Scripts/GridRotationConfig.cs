using UnityEngine;

namespace Sources.Features.HexagonSort.GridSystem.Scripts
{
    [CreateAssetMenu(menuName = "StaticData/GridRotationConfig", fileName = "GridRotationConfig", order = 0)]
    public class GridRotationConfig : ScriptableObject
    {
        [field: SerializeField] public float SnapAnchorAngle { get; private set; }
        [field: SerializeField] public float SnapThreshold { get; private set; }
        [field: SerializeField] public float SnapSpeed { get; private set; }
        [field: SerializeField] public float ReturnSpeed { get; private set; }
        [field: SerializeField] public float RotationSensitivity { get; private set; }
        [field: SerializeField] public bool ClockwiseRotation { get; private set; }
    }
}