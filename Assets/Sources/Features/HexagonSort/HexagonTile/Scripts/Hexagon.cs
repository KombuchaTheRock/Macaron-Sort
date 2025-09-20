using UnityEngine;

namespace Sources.Features.HexagonSort.HexagonTile.Scripts
{
    public class Hexagon : MonoBehaviour
    {
        public HexagonTileType TileType { get; private set; }

        public void SetTileType(HexagonTileType tileType) => 
            TileType = tileType;
    }
}
