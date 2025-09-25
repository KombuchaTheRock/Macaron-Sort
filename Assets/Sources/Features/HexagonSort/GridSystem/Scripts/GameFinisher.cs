using System.Linq;
using Sources.Features.HexagonSort.Merge.Scripts;
using UnityEngine;

namespace Sources.Features.HexagonSort.GridSystem.Scripts
{
    public class GameFinisher : MonoBehaviour
    {
        [SerializeField] private HexagonGrid _hexagonGrid;
        private MergeSystem _mergeSystem;

        public void Initialize(MergeSystem mergeSystem)
        {
            _mergeSystem = mergeSystem;
            _mergeSystem.MergeFinished += OnMergeFinished;
        }

        private void OnDestroy() => 
            _mergeSystem.MergeFinished -= OnMergeFinished;

        private void OnMergeFinished()
        {
            int occupiedCellsCount = _hexagonGrid.Cells.Where(x => x.IsOccupied).ToList().Count;

            if (occupiedCellsCount >= _hexagonGrid.Cells.Count) 
                Debug.LogWarning("Game finished");
        }
    }
}