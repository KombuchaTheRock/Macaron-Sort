using UnityEngine;

namespace Sources.Features.HexagonSort.GridGenerator.Scripts
{
    [CreateAssetMenu(menuName = "HexagonSort/GridConfig", fileName = "GridConfig", order = 0)]
    public class GridConfig : ScriptableObject
    {
        public GridTemplate Template;
        [Space]
        public Grid Grid;
        public Color GridColor;
        public float CellSize;
        public int GridRadius;
    }
}