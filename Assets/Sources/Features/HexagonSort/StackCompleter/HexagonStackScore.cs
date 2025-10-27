using Sources.Features.HexagonSort.HexagonTile.Scripts;

namespace Sources.Features.HexagonSort.StackCompleter
{
    public struct HexagonStackScore
    {
        public HexagonTileType TileType;
        public readonly int Score;

        public HexagonStackScore(HexagonTileType tileType, int score)
        {
            TileType = tileType;
            Score = score;
        }
    }
}