using NaughtyAttributes;
using UnityEngine;

namespace Sources.Features.HexagonSort.GridGenerator.Scripts
{
    [CreateAssetMenu(menuName = "StaticData/GridConfig", fileName = "GridConfig", order = 0)]
    public class GridConfig : ScriptableObject
    {
        [field: Expandable, SerializeField] public CellConfig CellConfig { get; private set; }
        [field: Space (5), SerializeField] public Grid Grid { get; private set; }
        [field: SerializeField] public int Size { get; private set; }
    }
}