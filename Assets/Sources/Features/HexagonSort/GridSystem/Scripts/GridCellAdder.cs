using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sources.Common.CodeBase.Infrastructure;
using Sources.Common.CodeBase.Services.StaticData;
using Sources.Features.HexagonSort.GridSystem.GridGenerator.Scripts;
using Sources.Features.HexagonSort.Merge.Scripts;
using UnityEngine;

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

        public void AddCellsToRandomPositions(int cellsToAddCount)
        {
            if (_addCellsRoutine != null) 
                _coroutineRunner.StopCoroutine(_addCellsRoutine);
            
            _addCellsRoutine = _coroutineRunner.StartCoroutine(AddCellsToRandomPositionsRoutine(cellsToAddCount));
        }

        private IEnumerator AddCellsToRandomPositionsRoutine(int cellsToAddCount)
        {
            for (int i = 0; i < cellsToAddCount; i++)
            {
                AddCellToRandomPosition();
                yield return new WaitForSeconds(0.2f);
            }
        }

        private void AddCellToRandomPosition()
        {
            Vector2Int randomPositionOnEdge = GetRandomPositionOnEdge();

            Vector3 worldPosition =
                _hexagonGrid.GridComponent.CellToWorld(
                    new Vector3Int(randomPositionOnEdge.x, randomPositionOnEdge.y, 0));

            GridCell gridCell = _gridGenerator.GenerateGridCell(randomPositionOnEdge, worldPosition,
                _staticData.GameConfig.GridConfig.CellConfig);

            _hexagonGrid.AddCell(gridCell.PositionOnGrid, gridCell);
        }

        private List<Vector2Int> GetEdgePositions()
        {
            List<Vector2Int> edgePositions = new();

            foreach (Vector2Int positionOnGrid in _hexagonGrid.Cells.Select(cell => cell.PositionOnGrid))
            {
                Vector2Int[] neighbours = NeighbourCellsFindingUtility.GetNeighbourPositions(positionOnGrid);

                if (IsAllCellsOnGrid(neighbours) == false)
                    edgePositions.Add(positionOnGrid);
            }

            return edgePositions;
        }

        private Vector2Int GetRandomPositionOnEdge()
        {
            _edgePositions = GetEdgePositions();
            List<Vector2Int> positionsOnEdge = new();

            foreach (Vector2Int edgePosition in _edgePositions)
            {
                Vector2Int[] neighboursOnEdge = NeighbourCellsFindingUtility
                    .GetNeighbourPositions(edgePosition)
                    .Where(cell => _hexagonGrid.IsCellOnGrid(cell) == false)
                    .ToArray();

                if (neighboursOnEdge.Length <= 3)
                    positionsOnEdge.AddRange(neighboursOnEdge);
            }

            return positionsOnEdge[Random.Range(0, positionsOnEdge.Count - 1)];
        }

        private bool IsAllCellsOnGrid(Vector2Int[] cells) =>
            cells.All(cell => _hexagonGrid.IsCellOnGrid(cell));
    }
}