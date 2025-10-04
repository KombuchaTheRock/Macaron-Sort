using System.Collections.Generic;
using System.Linq;
using Sources.Features.HexagonSort.HexagonStackSystem.Scripts;
using Sources.Features.HexagonSort.HexagonTile.Scripts;

namespace Sources.Features.HexagonSort.Merge.Scripts
{
    public static class HexagonStackUtils
    {
        public static List<Hexagon> GetSimilarHexagons(HexagonStack stack, HexagonTileType topTileType)
        {
            List<Hexagon> similarHexagons = new();

            for (int i = stack.Hexagons.Count - 1; i >= 0; i--)
            {
                Hexagon hexagon = stack.Hexagons[i];

                if (hexagon.TileType != topTileType)
                    break;

                similarHexagons.Add(hexagon);
            }

            return similarHexagons;
        }

        public static bool CheckForMonoType(HexagonStack stack, HexagonTileType topTileType)
        {
            bool isMonoType = true;

            for (int i = stack.Hexagons.Count - 1; i >= 0; i--)
            {
                Hexagon hexagon = stack.Hexagons[i];

                if (hexagon.TileType == topTileType)
                    continue;

                isMonoType = false;
                break;
            }

            return isMonoType;
        }
        
        public static int CalculateScore(List<Hexagon> hexagons) =>
            hexagons.Sum(hexagon => hexagon.Score);
    }
}