using System.Collections.Generic;
using Sources.Common.CodeBase.Services;
using Sources.Common.CodeBase.Services.PlayerProgress;
using Sources.Features.HexagonSort.GridSystem.GridGenerator.Scripts;
using Sources.Features.HexagonSort.HexagonStackSystem.StackGenerator.Scripts;
using Sources.Features.HexagonSort.HexagonStackSystem.StackMover.Scripts;
using Sources.Features.HexagonSort.Merge.Scripts;
using UnityEngine;
using Zenject;

namespace Sources.Features.HexagonSort.GridSystem.Scripts
{
    public class HexagonGridSaveLoader : MonoBehaviour, IProgressReader
    {
        [SerializeField] private HexagonGrid _hexagonGrid;
        
        private IGameProgressService _gameProgress;
        private StackMover _stackMover;
        private MergeSystem _mergeSystem;
        private List<GridCell> _cells;
        private IStackGenerator _stackGenerator;

        [Inject]
        private void Construct(IGameProgressService gameProgress, IStackGenerator stackGenerator)
        {
            _stackGenerator = stackGenerator;
            _gameProgress = gameProgress;
        }

        public void Initialize(MergeSystem mergeSystem)
        {
            _mergeSystem = mergeSystem;
            _cells = _hexagonGrid.Cells;
            
            SubscribeUpdates();
        }

        public void ApplyProgress(GameProgress progress)
        {
            
        }

        private void OnDestroy()
        {
            CleanUp();
        }

        private void SubscribeUpdates()
        {
            foreach (GridCell gridCell in _cells) 
                gridCell.StackRemoved += UpdateGridData;

            _mergeSystem.MergeFinished += UpdateGridData; 
        }

        private void CleanUp()
        {
            foreach (GridCell gridCell in _cells) 
                gridCell.StackRemoved -= UpdateGridData;

            _mergeSystem.MergeFinished -= UpdateGridData;
        }

        private void UpdateGridData()
        {
            GridData gridData = _gameProgress.GameProgress.PersistentProgressData.WorldData.GridData;
            gridData.UpdateData(_cells);
        }
    }
}