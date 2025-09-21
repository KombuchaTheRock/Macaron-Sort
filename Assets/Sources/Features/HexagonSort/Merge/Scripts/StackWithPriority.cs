using System;
using Sources.Features.HexagonSort.HexagonStackSystem.StackGenerator.Scripts;

namespace Sources.Features.HexagonSort.Merge.Scripts
{
    public class StackWithPriority : IComparable<StackWithPriority>
    {
        public HexagonStack Stack { get; private set; }
        public bool IsMonoType { get; private set; }
        public int SameHexagonCount { get; private set; }

        private static int _nextId = 1;
        private readonly int _uniqueId;

        public StackWithPriority(int sameHexagonCount, HexagonStack stack, bool isMonoType)
        {
            _uniqueId = _nextId++;

            SameHexagonCount = sameHexagonCount;
            Stack = stack;
            IsMonoType = isMonoType;
        }

        public int CompareTo(StackWithPriority other)
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