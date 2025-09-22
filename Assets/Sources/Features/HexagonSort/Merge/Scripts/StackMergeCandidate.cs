using System;
using Sources.Features.HexagonSort.GridSystem.GridGenerator.Scripts;
using Sources.Features.HexagonSort.HexagonStackSystem.StackGenerator.Scripts;

namespace Sources.Features.HexagonSort.Merge.Scripts
{
    public class StackMergeCandidate : IComparable<StackMergeCandidate>
    {
        private static int _nextId = 1;
        
        private int _sameHexagonCount;
        private readonly int _uniqueId;
        
        public HexagonStack Stack { get; private set; }
        public GridCell Cell { get; private set; }
        public bool IsMonoType { get; private set; }

        public StackMergeCandidate(int sameHexagonCount, HexagonStack stack, bool isMonoType, GridCell cell)
        {
            _uniqueId = _nextId++;
            _sameHexagonCount = sameHexagonCount;
            
            Cell = cell;
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

            int hexagonCountCompression = other._sameHexagonCount.CompareTo(_sameHexagonCount);
            if (hexagonCountCompression != 0) return hexagonCountCompression;

            return _uniqueId.CompareTo(other._uniqueId);
        }
    }
}