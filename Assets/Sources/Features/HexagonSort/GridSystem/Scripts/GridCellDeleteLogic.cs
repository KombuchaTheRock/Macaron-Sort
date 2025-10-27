using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sources.Common.CodeBase.Infrastructure;
using Sources.Features.HexagonSort.GridSystem.GridGenerator.Scripts;
using Sources.Features.HexagonSort.Merge.Scripts;
using UnityEngine;

namespace Sources.Features.HexagonSort.GridSystem.Scripts
{
    public class GridCellDeleteLogic
    {
        private HexagonGrid _hexagonGrid;
        private ICoroutineRunner _coroutineRunner;
        private Coroutine _deleteCellsRoutine;

        public GridCellDeleteLogic(HexagonGrid hexagonGrid, ICoroutineRunner coroutineRunner)
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

                yield return new WaitForSeconds(0.05f);
            }

            onCompleted?.Invoke();
        }

        private void DeleteRandomEdgeFreeCell()
        {
            List<Vector2Int> edgePositions = GridCellsUtility.GetEdgePositions(_hexagonGrid);

            Vector2Int cellWithMaxMagnitude = edgePositions
                .Where(pos =>
                {
                    Vector3 worldPos = _hexagonGrid.GridComponent.GetCellCenterWorld(new Vector3Int(pos.x, pos.y, 0));
                    return worldPos.magnitude > 1f;
                })
                .OrderByDescending(pos =>
                {
                    Vector3 worldPos = _hexagonGrid.GridComponent.GetCellCenterWorld(new Vector3Int(pos.x, pos.y, 0));
                    return worldPos.magnitude;
                })
                .FirstOrDefault();

            if (cellWithMaxMagnitude == default) 
                return;
            
            GridCell gridCell = _hexagonGrid.Cells.FirstOrDefault(x => x.PositionOnGrid == cellWithMaxMagnitude);
            _hexagonGrid.RemoveCell(gridCell);
        }
    }
}