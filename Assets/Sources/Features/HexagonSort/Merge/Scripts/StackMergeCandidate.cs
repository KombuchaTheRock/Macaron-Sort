using System;
using Sources.Features.HexagonSort.GridSystem.GridGenerator.Scripts;
using Sources.Features.HexagonSort.HexagonStackSystem.StackGenerator.Scripts;

namespace Sources.Features.HexagonSort.Merge.Scripts
{
    public class StackMergeCandidate : IComparable<StackMergeCandidate>
    {
        public HexagonStack Stack { get; private set; }
        public bool IsMonoType { get; private set; }
        public int SameHexagonCount { get; private set; }
        public GridCell Cell { get; private set; }
        
        private static int _nextId = 1;
        private readonly int _uniqueId;

        public StackMergeCandidate(int sameHexagonCount, HexagonStack stack, bool isMonoType, GridCell cell)
        {
            _uniqueId = _nextId++;

            Cell = cell;
            SameHexagonCount = sameHexagonCount;
            Stack = stack;
            IsMonoType = isMonoType;
        }

        public int CompareTo(StackMergeCandidate other)
        {
            if (ReferenceEquals(this, other))
                return 0;

            if (other is null)
                return 1;

            if (IsMonoType && other.IsMonoType == false) return -1;
            if (IsMonoType == false && other.IsMonoType) return 1;

            int hexagonCountCompression = other.SameHexagonCount.CompareTo(SameHexagonCount);
            if (hexagonCountCompression != 0) return hexagonCountCompression;

            return _uniqueId.CompareTo(other._uniqueId);
        }
    }
}