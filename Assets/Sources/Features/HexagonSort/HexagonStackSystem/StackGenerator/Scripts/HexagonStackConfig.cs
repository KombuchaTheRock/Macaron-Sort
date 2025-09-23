using UnityEngine;

namespace Sources.Features.HexagonSort.HexagonStackSystem.StackGenerator.Scripts
{
    [CreateAssetMenu(menuName = "Hexagon/HexagonStackConfig", fileName = "HexagonStackConfig", order = 0)]
    public class HexagonStackConfig : ScriptableObject
    {
        [field: SerializeField] public HexagonStackTemplate Template { get; private set; }
        [field: Space(5), SerializeField] public float OffsetBetweenTiles { get; private set; }
        [field: Range(1, 10), SerializeField] public int MaxStackSize { get; private set; }
        [field: Range(1, 10), SerializeField] public int MinStackSize { get; private set; }
        [field: Range(1, 6), SerializeField] public int MaxTileChanges { get; private set; }

        private void OnValidate()
        {
            if (MaxStackSize < MinStackSize)
                MaxStackSize = MinStackSize;
        }
    }
}