using System;

namespace Sources.Features.HexagonSort.GridSystem.Scripts
{
    public class SimpleLockedCell : IComparable<SimpleLockedCell>
    {
        private static int _nextId = 1;
        private readonly int _uniqueId;

        public SimpleCellLock SimpleCellLock { get; }

        public SimpleLockedCell(SimpleCellLock simpleCellLock)
        {
            _uniqueId = _nextId++;
            SimpleCellLock = simpleCellLock;
        }

        public int CompareTo(SimpleLockedCell other)
        {
            if (ReferenceEquals(this, other))
                return 0;

            if (SimpleCellLock.CompletedStacksToUnlock == other.SimpleCellLock.CompletedStacksToUnlock)
                return _uniqueId.CompareTo(other._uniqueId);

            if (SimpleCellLock.CompletedStacksToUnlock < other.SimpleCellLock.CompletedStacksToUnlock)
                return -1;

            return 1;
        }
    }
}