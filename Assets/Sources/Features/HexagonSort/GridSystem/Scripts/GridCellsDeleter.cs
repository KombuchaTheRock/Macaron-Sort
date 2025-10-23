using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sources.Common.CodeBase.Infrastructure;
using Sources.Features.HexagonSort.GridSystem.GridGenerator.Scripts;
using Sources.Features.HexagonSort.Merge.Scripts;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Sources.Features.HexagonSort.GridSystem.Scripts
{
    public class GridCellsDeleter
    {
        private HexagonGrid _hexagonGrid;
        private ICoroutineRunner _coroutineRunner;
        private Coroutine _deleteCellsRoutine;

        public GridCellsDeleter(HexagonGrid hexagonGrid, ICoroutineRunner coroutineRunner)
        {
            _coroutineRunner = coroutineRunner;
            _hexagonGrid = hexagonGrid;
        }

        public void DeleteRandomEdgeFreeCells(int count, Action onCompleted = null)
        {
            if (_deleteCellsRoutine != null)
                _coroutineRunner.StopCoroutine(_deleteCellsRoutine);

            _deleteCellsRoutine = _coroutineRunner.StartCoroutine(DeleteRandomEdgeFreeCellsRoutine(count, onCompleted));
        }

        private IEnumerator DeleteRandomEdgeFreeCellsRoutine(int count, Action onCompleted)
        {
            for (int i = 0; i < count; i++)
            {
                if (_hexagonGrid.Cells.Count > 0)
                    DeleteRandomEdgeFreeCell();
                else
                    break;

                yield return new WaitForSeconds(0.1f);
            }

            onCompleted?.Invoke();
        }

        private void DeleteRandomEdgeFreeCell()
        {
            List<Vector2Int> edgePositions = GridCellsUtility.GetEdgePositions(_hexagonGrid);

            GridCell[] freeCells = _hexagonGrid.Cells
                .Where(cell => IsFreeEdgeCell(cell, edgePositions))
                .ToArray();

            if (freeCells.Length <= 0)
                return;

            int i = 0;
            while (i < freeCells.Length - 1)
            {
                GridCell randomEdgeCell = freeCells[Random.Range(0, edgePositions.Count - 1)];

                Vector3Int positionOnGrid = new(randomEdgeCell.PositionOnGrid.x, randomEdgeCell.PositionOnGrid.y, 0);
                if (_hexagonGrid.GridComponent.CellToWorld(positionOnGrid).magnitude <= 2)
                {
                    i++;
                    continue;
                }

                _hexagonGrid.RemoveCell(randomEdgeCell);
                break;
            }
        }

        private static bool IsFreeEdgeCell(GridCell cell, List<Vector2Int> edgePositions) =>
            cell.IsOccupied == false && edgePositions.Contains(cell.PositionOnGrid);
    }
}