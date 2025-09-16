using NaughtyAttributes;
using UnityEngine;

namespace Sources.Features.HexagonSort.StackMover.Scripts
{
    [CreateAssetMenu(menuName = "StaticData/StackMoverConfig", fileName = "StackMoverConfig", order = 0)]
    public class StackMoverConfig : ScriptableObject
    {
        [field: SerializeField] public float MaxRaycastDistance { get; private set; }
        [field: SerializeField] public float DragVerticalOffset { get; private set; }
        [field: SerializeField] public float PlaceOffsetAboveGrid { get; private set; }
        
        [field: SerializeField, Layer] public int GroundLayer { get; private set; }
        [field: SerializeField, Layer] public int GridLayer { get; private set; }
        [field: SerializeField, Layer] public int HexagonLayer { get; private set; }
    }
}