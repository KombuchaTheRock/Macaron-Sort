using System;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using Sources.Common.CodeBase.Infrastructure;
using Sources.Common.CodeBase.Services.PlayerProgress;
using Sources.Common.CodeBase.Services.StaticData;
using Sources.Features.HexagonSort.GridSystem.GridGenerator.Scripts;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Sources.Features.HexagonSort.GridSystem.Scripts
{
    public class GridModificator : MonoBehaviour, ICoroutineRunner
    {
        public event Action GridModified;

        [SerializeField] private HexagonGrid _hexagonGrid;
        [SerializeField] private GridCellUnlocker _gridCellUnlocker;
        [SerializeField, Min(0)] private int _cellsToAddCount;
        [SerializeField, Min(0)] private int _cellsToDeleteCount;

        private GridCellAddLogic _gridCellsAddLogic;
        private GridCellDeleteLogic _gridCellDeleteLogic;
        private GridCellLockLogic _gridCellsLockLogic;

        private IGridGenerator _gridGenerator;
        private IStaticDataService _staticData;
        private IPlayerLevel _playerLevel;
        private Dictionary<int, GridModifier> _gridModifiers;


        [Inject]
        private void Construct(IStaticDataService staticData, IGridGenerator gridGenerator, IPlayerLevel playerLevel)
        {
            _playerLevel = playerLevel;
            _staticData = staticData;
            _gridGenerator = gridGenerator;
        }

        private void Awake()
        {
            _gridModifiers =
                _staticData.GameConfig.GridModifierConfig.Modifiers
                    .ToDictionary(x => x.LevelForStart, x => x);

            _gridCellsAddLogic = new GridCellAddLogic(_gridGenerator, _staticData, this, _hexagonGrid);
            _gridCellDeleteLogic = new GridCellDeleteLogic(_hexagonGrid, this);
            _gridCellsLockLogic = new GridCellLockLogic(_hexagonGrid, this);

            SubscribeUpdates();
        }

        private void OnDestroy() =>
            CleanUp();

        private void SubscribeUpdates() =>
            _playerLevel.LevelChanged += OnLevelChanged;

        private void CleanUp() =>
            _playerLevel.LevelChanged -= OnLevelChanged;

        private void OnLevelChanged(int level)
        {
            Debug.Log("LevelChanged " + level);
            
            if (_gridModifiers.TryGetValue(level, out GridModifier modifier))
            {
                if (modifier.AddNewCellsAdder)
                {
                    int count = Random.Range(modifier.MinMaxAddedCellsCount.x, modifier.MinMaxAddedCellsCount.y);
                    AddCells(count);
                }

                if (modifier.AddCellsDeleter)
                {
                    int freeCellsCount = _hexagonGrid.Cells.Count(x => x is { IsOccupied: false, IsLocked: false });

                    if (freeCellsCount > 3)
                    {
                        int count = Random.Range(modifier.MinMaxDeletedCellsCount.x,
                            Mathf.Min(modifier.MinMaxDeletedCellsCount.y, freeCellsCount - 3));

                        RemoveCells(count);
                    }
                }

                if (modifier.AddSimpleBlocker)
                {
                    int freeCellsCount = _hexagonGrid.Cells.Count(x => x is { IsOccupied: false, IsLocked: false });

                    if (freeCellsCount > 3)
                    {
                        int count = Random.Range(modifier.MinMaxSimpleBlockerCount.x,
                            Mathf.Min(modifier.MinMaxSimpleBlockerCount.y, freeCellsCount - 3));
                        
                        BlockCellsSimple(count);
                    }
                }
            }
        }

        private void AddCells(int count)
        {
            _gridCellsAddLogic.AddCellsToRandomPositions(count,
                () => GridModified?.Invoke());
        }

        private void RemoveCells(int count)
        {
            _gridCellDeleteLogic.DeleteRandomEdgeFreeCells(count,
                () => GridModified?.Invoke());
        }
        
        [Button("BlockCellSimple")]
        private void BlockCellsSimple(int count = 1)
        {
            _gridCellsLockLogic.AddSimpleBlockers(count, simpleCellLocks =>
            {
                if (simpleCellLocks.Count <= 0)
                    return;

                _gridCellUnlocker.AddRangeSimpleLockedCell(simpleCellLocks);
                GridModified?.Invoke();
            });
        }
    }
}