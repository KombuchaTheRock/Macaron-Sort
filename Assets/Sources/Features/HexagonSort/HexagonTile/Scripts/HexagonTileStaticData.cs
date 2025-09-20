using System.Collections.Generic;
using UnityEngine;

namespace Sources.Features.HexagonSort.HexagonTile.Scripts
{
    [CreateAssetMenu(menuName = "StaticData/HexagonTileStaticData", fileName = "HexagonTileStaticData", order = 0)]
    public class HexagonTileStaticData : ScriptableObject
    {
        [field: SerializeField] public List<HexagonTileConfig> Configs { get; private set; }
    }
}