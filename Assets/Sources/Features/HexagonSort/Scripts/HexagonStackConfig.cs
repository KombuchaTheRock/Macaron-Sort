using UnityEngine;

namespace Sources.Features.HexagonSort.Scripts
{
    [CreateAssetMenu(menuName = "Hexagon/HexagonStackConfig", fileName = "HexagonStackConfig", order = 0)]
    public class HexagonStackConfig : ScriptableObject
    {
        [Space(5)] public HexagonStackTemplate Template;
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