using System;
using System.Collections;
using System.Collections.Generic;
using Sources.Common.CodeBase.Infrastructure;
using Sources.Features.HexagonSort.GridSystem.GridGenerator.Scripts;
using Sources.Features.HexagonSort.Merge.Scripts;
using UnityEngine;

namespace Sources.Features.HexagonSort.GridSystem.Scripts
{
    public class GridCellLockLogic
    {
        private const int CompletedStacksToUnlock = 3;
        
        private HexagonGrid _hexagonGrid;
        private ICoroutineRunner _coroutineRunner;
        private Coroutine _addSimpleBlockersRoutine;

        public GridCellLockLogic(HexagonGrid hexagonGrid, ICoroutineRunner coroutineRunner)
        {
            _coroutineRunner = coroutineRunner;
            _hexagonGrid = hexagonGrid;
        }

        public void AddSimpleBlockers(int count, Action<List<SimpleCellLock>> onCompleted)
        {
            if (_addSimpleBlockersRoutine != null) 
                _coroutineRunner.StopCoroutine(_addSimpleBlockersRoutine);
            
            _addSimpleBlockersRoutine = _coroutineRunner.StartCoroutine(AddSimpleBlockersRoutine(count,onCompleted));
        }
        
        private IEnumerator AddSimpleBlockersRoutine(int count, Action<List<SimpleCellLock>> onCompleted)
        {
            List<SimpleCellLock> lockedCells = new();
            
            for (int i = 0; i < count; i++)
            {
                if (GridCellsUtility.TryGetRandomFreeCell(_hexagonGrid, out GridCell cell))
                {
                    SimpleCellLock simpleCellLock = new(CompletedStacksToUnlock);
                    
                    cell.Lock(simpleCellLock);        
                    lockedCells.Add(simpleCellLock);
                }

                yield return new WaitForSeconds(0.05f);
            }
            
            onCompleted.Invoke(lockedCells);
        }
    }
}