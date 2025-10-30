using System;
using System.Collections;
using System.Collections.Generic;
using Sources.Common.CodeBase.Infrastructure;
using Sources.Common.CodeBase.Services.StaticData;
using Sources.Features.HexagonSort.GridSystem.GridGenerator.Scripts;
using Sources.Features.HexagonSort.GridSystem.Scripts;
using Sources.Features.HexagonSort.Merge.Scripts;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Sources.Features.HexagonSort.GridSystem.GridModificator.Scripts
{
    public class GridCellAddLogic
    {
        private Coroutine _addCellsRoutine;

        private readonly IGridGenerator _gridGenerator;
        private readonly IStaticDataService _staticData;
        private readonly HexagonGrid _hexagonGrid;
        private readonly ICoroutineRunner _coroutineRunner;

        public GridCellAddLogic(IGridGenerator gridGenerator, IStaticDataService staticData,
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

            _addCellsRoutine =
                _coroutineRunner.StartCoroutine(AddCellsToRandomPositionsRoutine(cellsToAddCount, onCompleted));
        }

        private IEnumerator AddCellsToRandomPositionsRoutine(int cellsToAddCount, Action onCompleted = null)
        {
            for (int i = 0; i < cellsToAddCount; i++)
            {
                AddCellToRandomPosition();
                yield return new WaitForSeconds(0.05f);
            }

            onCompleted?.Invoke();
        }

        private void AddCellToRandomPosition()
        {
            List<Vector2Int> edgePositions = GridCellsUtility.GetRandomPositionOnEdge(_hexagonGrid, 3);

            if (edgePositions.Count == 0)
                return;
            
            Vector2Int randomPositionOnEdge = edgePositions[Random.Range(0, edgePositions.Count - 1)];
            AddNewCell(randomPositionOnEdge);
        }

        private void AddNewCell(Vector2Int randomPositionOnEdge)
        {
            GridCell gridCell = _gridGenerator.GenerateGridCell(randomPositionOnEdge,
                _staticData.GameConfig.GridConfig.CellConfig);

            _hexagonGrid.AddCell(gridCell.PositionOnGrid, gridCell);
        }
    }
}