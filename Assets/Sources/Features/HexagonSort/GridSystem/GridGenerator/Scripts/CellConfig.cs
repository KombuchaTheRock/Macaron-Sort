using UnityEngine;

namespace Sources.Features.HexagonSort.GridSystem.GridGenerator.Scripts
{
    [CreateAssetMenu(menuName = "StaticData/CellConfig", fileName = "CellConfig", order = 0)]
    public class CellConfig : ScriptableObject
    {
        [field: SerializeField] public Color CellColor { get; private set; }
        [field: SerializeField] public Color CellHighlightColor { get; set; }
        [field: SerializeField] public float CellSize { get; private set; }
    }
}