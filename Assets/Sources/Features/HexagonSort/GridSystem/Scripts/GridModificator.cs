using NaughtyAttributes;
using Sources.Common.CodeBase.Infrastructure;
using Sources.Common.CodeBase.Services.StaticData;
using Sources.Features.HexagonSort.GridSystem.GridGenerator.Scripts;
using UnityEngine;
using Zenject;

namespace Sources.Features.HexagonSort.GridSystem.Scripts
{
    public class GridModificator : MonoBehaviour, ICoroutineRunner
    {
        [SerializeField] private HexagonGrid _hexagonGrid;
        [SerializeField, Range(0f, 0.5f)] private float _gizmosEdgeCellRadius;
        [SerializeField] private int _cellsToAddCount;
        
        private IGridGenerator _gridGenerator;
        private IStaticDataService _staticData;
        private GridCellAdder _gridCellsAdder;
        private GridCellsDeleter _gridCellsDeleter;

        [Inject]
        private void Construct(IStaticDataService staticData, IGridGenerator gridGenerator)
        {
            _staticData = staticData;
            _gridGenerator = gridGenerator;
        }
        
        private void Awake()
        {
            _gridCellsAdder = new GridCellAdder(_gridGenerator, _staticData, this, _hexagonGrid);
            _gridCellsDeleter = new GridCellsDeleter(_hexagonGrid);
        }

        [Button("AddCells")]
        private void AddCells() => 
            _gridCellsAdder.AddCellsToRandomPositions(_cellsToAddCount);

        [Button("RemoveCell")]
        private void RemoveCell() => 
            _gridCellsDeleter.DeleteRandomFreeCell();
    }
}