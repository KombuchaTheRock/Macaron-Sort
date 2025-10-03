using System.Collections.Generic;
using Sources.Features.HexagonSort.GridSystem.GridGenerator.Scripts;

namespace Sources.Features.HexagonSort.Merge.Scripts
{
    public class MergePriority
    {
        public (MergeCandidate from, MergeCandidate to) GetMergePair(MergeCandidate placed, MergeCandidate neighbour,
            int neighbourCount)
        {
            SortedSet<MergeCandidate> mergePair = new() { placed, neighbour };

            if (neighbourCount > 1)
                return (neighbour, placed);
    
            return mergePair.Max.IsMonoType 
                ? (mergePair.Min, mergePair.Max) 
                : (mergePair.Max, mergePair.Min);
        }

        public SortedSet<MergeCandidate> GetMergeCandidates(List<GridCell> neighboursCells)
        {
            SortedSet<MergeCandidate> mergeCandidates = new();

            foreach (GridCell cell in neighboursCells)
                mergeCandidates.Add(new MergeCandidate(cell));

            return mergeCandidates;
        }
    }
}