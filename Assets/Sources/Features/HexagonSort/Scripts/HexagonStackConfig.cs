using UnityEngine;

namespace Sources.Features.HexagonSort.Scripts
{
    [CreateAssetMenu(menuName = "Hexagon/HexagonStackConfig", fileName = "HexagonStackConfig", order = 0)]
    public class HexagonStackConfig : ScriptableObject
    {
        [field: SerializeField] public HexagonStackTemplate Template { get; private set; }
        [Space(5)]
        public float HexagonHeight;
        [Range(1, 10)] public int MaxStackSize;
        [Range(1, 10)] public int MinStackSize;
        [Space(5)] public Color[] Colors;

        private void OnValidate()
        {
            if (MaxStackSize < MinStackSize)
                MaxStackSize = MinStackSize;
        }
    }
}