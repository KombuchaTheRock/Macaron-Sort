using System;
using System.Collections.Generic;
using Sources.Features.HexagonSort.GridSystem.GridGenerator.Scripts;
using Sources.Features.HexagonSort.HexagonStackSystem.Scripts;
using Sources.Features.HexagonSort.HexagonTile.Scripts;

namespace Sources.Features.HexagonSort.Merge.Scripts
{
    public class MergeCandidate : IComparable<MergeCandidate>
    {
        private static int _nextId = 1;

        private readonly int _uniqueId;
        private readonly HexagonTileType _initialTopTileType;

        public HexagonStack Stack { get; }
        public GridCell Cell { get; }
        public bool IsMonoType { get; }
        private List<Hexagon> SimilarHexagons { get; }

        public MergeCandidate(GridCell cell)
        {
            _uniqueId = _nextId++;

            Cell = cell;
            Stack = cell.Stack;
            _initialTopTileType = Stack.TopHexagon.TileType;
            SimilarHexagons = GetSimilarHexagons();
            IsMonoType = CheckForMonoType();
        }

        public int CompareTo(MergeCandidate other)
        {
            if (ReferenceEquals(this, other))
                return 0;

            if (other is null)
                return 1;

            switch (IsMonoType)
            {
                case true when other.IsMonoType == false:
                    return -1;
                case false when other.IsMonoType:
                    return 1;
                default:
                {
                    int hexagonCountCompression = other.SimilarHexagons.Count.CompareTo(SimilarHexagons.Count);

                    return hexagonCountCompression != 0
                        ? hexagonCountCompression
                        : _uniqueId.CompareTo(other._uniqueId);
                }
            }
        }

        private List<Hexagon> GetSimilarHexagons()
        {
            List<Hexagon> similarHexagons = new();

            for (int i = Stack.Hexagons.Count - 1; i >= 0; i--)
            {
                Hexagon hexagon = Stack.Hexagons[i];

                if (hexagon.TileType != _initialTopTileType)
                    break;

                similarHexagons.Add(hexagon);
            }

            return similarHexagons;
        }

        private bool CheckForMonoType()
        {
            bool isMonoType = true;

            for (int i = Stack.Hexagons.Count - 1; i >= 0; i--)
            {
                Hexagon hexagon = Stack.Hexagons[i];

                if (hexagon.TileType == _initialTopTileType)
                    continue;

                isMonoType = false;
                break;
            }

            return isMonoType;
        }
    }
}