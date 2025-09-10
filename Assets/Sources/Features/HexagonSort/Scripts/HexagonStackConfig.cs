using UnityEngine;

namespace Sources.Features.HexagonSort.Scripts
{
    [CreateAssetMenu(menuName = "Hexagon/HexagonStackConfig", fileName = "HexagonStackConfig", order = 0)]
    public class HexagonStackConfig : ScriptableObject
    {
        public float HexagonHeight;
        public int MaxStackSize;
        public int MinStackSize;
        [Space(5)] public Color[] Colors;
    }
}