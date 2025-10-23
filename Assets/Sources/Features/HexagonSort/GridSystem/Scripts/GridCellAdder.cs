using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sources.Common.CodeBase.Infrastructure;
using Sources.Common.CodeBase.Services.StaticData;
using Sources.Features.HexagonSort.GridSystem.GridGenerator.Scripts;
using Sources.Features.HexagonSort.Merge.Scripts;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Sources.Features.HexagonSort.GridSystem.Scripts
{
    public class GridCellAdder
    {
        private List<Vector2Int> _edgePositions = new();
        private Coroutine _addCellsRoutine;
        
        private readonly IGridGenerator _gridGenerator;
        private readonly IStaticDataService _staticData;
        private readonly HexagonGrid _hexagonGrid;
        private readonly ICoroutineRunner _coroutineRunner;

        public GridCellAdder(IGridGenerator gridGenerator, IStaticDataService staticData,
            ICoroutineRunner coroutineRunner, HexagonGrid hexagonGrid)
        {
            _staticData = staticData;
            _coroutineRunner = coroutineRunner;
            _hexagonGrid = hexagonGrid;
            _gridGenerator = gridGenerator;
        }

        public void AddCellsToRandomPositions(int cellsToAddCount, Action onCompleted = null)
        {
            if (_addCellsRoutine != null) 
                _coroutineRunner.StopCoroutine(_addCellsRoutine);
            
            _addCellsRoutine = _coroutineRunner.StartCoroutine(AddCellsToRandomPositionsRoutine(cellsToAddCount, onCompleted));
        }

        private IEnumerator AddCellsToRandomPositionsRoutine(int cellsToAddCount, Action onCompleted = null)
        {
            for (int i = 0; i < cellsToAddCount; i++)
            {
                AddCellToRandomPosition();
                yield return new WaitForSeconds(0.1f);
            }
            
            onCompleted?.Invoke();
        }

        private void AddCellToRandomPosition()
        {
            Vector2Int randomPositionOnEdge = GetRandomPositionOnEdge();

            Vector3 worldPosition =
                _hexagonGrid.GridComponent.CellToWorld(
                    new Vector3Int(randomPositionOnEdge.x, randomPositionOnEdge.y, 0));

            if (worldPosition.magnitude > 3)
                return;
            
            GridCell gridCell = _gridGenerator.GenerateGridCell(randomPositionOnEdge, worldPosition,
                _staticData.GameConfig.GridConfig.CellConfig);

            _hexagonGrid.AddCell(gridCell.PositionOnGrid, gridCell);
        }

        private Vector2Int GetRandomPositionOnEdge()
        {
            _edgePositions = GridCellsUtility.GetEdgePositions(_hexagonGrid);
            List<Vector2Int> positionsOnEdge = new();

            foreach (Vector2Int edgePosition in _edgePositions)
            {
                Vector2Int[] neighboursOnEdge = GridCellsUtility
                    .GetNeighbourPositions(edgePosition)
                    .Where(cell => _hexagonGrid.IsCellOnGrid(cell) == false)
                    .ToArray();

                if (neighboursOnEdge.Length <= 3)
                    positionsOnEdge.AddRange(neighboursOnEdge);
            }

            return positionsOnEdge[Random.Range(0, positionsOnEdge.Count - 1)];
        }
    }
}