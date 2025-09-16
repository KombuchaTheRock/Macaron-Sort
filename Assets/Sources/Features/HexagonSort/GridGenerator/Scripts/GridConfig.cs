using UnityEngine;

namespace Sources.Features.HexagonSort.GridGenerator.Scripts
{
    [CreateAssetMenu(menuName = "StaticData/GridConfig", fileName = "GridConfig", order = 0)]
    public class GridConfig : ScriptableObject
    {
        [field: Space (5), SerializeField] public Grid Grid { get; private set; }
        [field: SerializeField] public Color CellColor { get; private set; }
        [field: SerializeField] public Color CellHighlightColor { get; set; }
        [field: SerializeField] public float CellSize { get; private set; }
        [field: SerializeField] public int GridRadius { get; private set; }
    }
}