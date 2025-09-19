using NaughtyAttributes;
using UnityEngine;

namespace Sources.Features.HexagonSort.HexagonStack.StackMover.Scripts
{
    [CreateAssetMenu(menuName = "StaticData/StackMoverConfig", fileName = "StackMoverConfig", order = 0)]
    public class StackMoverConfig : ScriptableObject
    {
        [field: Range(200, 500), SerializeField] public float MaxRaycastDistance { get; private set; }
        [field: SerializeField] public float DraggingVerticalOffset { get; private set; }
        [field: SerializeField] public float DraggingHorizontalOffset { get; private set; }
        [field: SerializeField] public float DraggingSpeed { get; private set; }
        [field: SerializeField] public float ToInitialPositionSpeed { get; private set; }
        [field: SerializeField] public float PlaceOffsetAboveGrid { get; private set; }
        [field: SerializeField, Layer] public int GroundLayer { get; private set; }
        [field: SerializeField, Layer] public int GridLayer { get; private set; }
        [field: SerializeField, Layer] public int StackLayer { get; private set; }
    }
}