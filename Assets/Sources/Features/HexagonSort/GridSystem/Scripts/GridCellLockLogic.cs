using System;
using System.Collections;
using System.Collections.Generic;
using Sources.Common.CodeBase.Infrastructure;
using Sources.Common.CodeBase.Infrastructure.Extensions;
using Sources.Features.HexagonSort.GridSystem.GridGenerator.Scripts;
using Sources.Features.HexagonSort.HexagonTile.Scripts;
using Sources.Features.HexagonSort.Merge.Scripts;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Sources.Features.HexagonSort.GridSystem.Scripts
{
    public class GridCellLockLogic
    {
        private const int CompletedStacksToUnlock = 3;

        private HexagonGrid _hexagonGrid;
        private ICoroutineRunner _coroutineRunner;
        private Coroutine _addSimpleBlockersRoutine;
        private Coroutine _addTileScoreLocksRoutine;

        public GridCellLockLogic(HexagonGrid hexagonGrid, ICoroutineRunner coroutineRunner)
        {
            _coroutineRunner = coroutineRunner;
            _hexagonGrid = hexagonGrid;
        }

        public void AddSimpleLocks(int count, Action<List<SimpleCellLock>> onCompleted)
        {
            if (_addSimpleBlockersRoutine != null)
                _coroutineRunner.StopCoroutine(_addSimpleBlockersRoutine);

            _addSimpleBlockersRoutine = _coroutineRunner.StartCoroutine(AddSimpleBlockersRoutine(count, onCompleted));
        }

        public void AddTileScoreLocks(int count, Action<List<TileScoreCellLock>> onCompleted)
        {
            if (_addTileScoreLocksRoutine != null)
                _coroutineRunner.StopCoroutine(_addTileScoreLocksRoutine);

            _addTileScoreLocksRoutine = _coroutineRunner.StartCoroutine(AddTileScoreCellLocksRoutine(count, onCompleted));
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

        private IEnumerator AddTileScoreCellLocksRoutine(int count, Action<List<TileScoreCellLock>> onCompleted)
        {
            List<TileScoreCellLock> lockedCells = new();

            for (int i = 0; i < count; i++)
            {
                if (GridCellsUtility.TryGetRandomFreeCell(_hexagonGrid, out GridCell cell))
                {
                    TileScoreCellLock cellLock = new(EnumExtensions.GetRandomValue<HexagonTileType>(),
                        Random.Range(10, 30));
                    
                    cell.Lock(cellLock);
                    lockedCells.Add(cellLock);
                }

                yield return new WaitForSeconds(0.05f);
            }

            onCompleted.Invoke(lockedCells);
        }
        
        public void AddTileScoreCellLock()
        {
            if (GridCellsUtility.TryGetRandomFreeCell(_hexagonGrid, out GridCell cell))
            {
                TileScoreCellLock cellLock = new(EnumExtensions.GetRandomValue<HexagonTileType>(),
                    Random.Range(150, 300));
                    
                cell.Lock(cellLock);
            }
        }
    }
}