using System.Collections.Generic;
using Sources.Common.CodeBase.Paths;
using Sources.Common.CodeBase.Services.PlayerProgress;
using Sources.Common.CodeBase.Services.ResourceLoader;
using Sources.Common.CodeBase.Services.StaticData;
using Sources.Features.HexagonSort.GridSystem.GridGenerator.Scripts;
using Sources.Features.HexagonSort.GridSystem.Scripts;
using Sources.Features.HexagonSort.HexagonStackSystem.StackMover.Scripts;
using Sources.Features.HexagonSort.Merge.Scripts;
using UnityEngine;
using Zenject;

namespace Sources.Common.CodeBase.Services.Factories.GameFactory
{
    public class GameFactory : BaseFactory, IGameFactory
    {
        private const string InstanceRootName = "GameFactoryRoot";

        private readonly IStaticDataService _staticData;
        private Transform _instanceRoot;

        public List<IProgressReader> ProgressReaders { get; private set; }
        public MergeSystem MergeSystem { get; private set; }
        public StackMover StackMover { get; private set; }
        public GridRotator GridRotator { get; private set; }

        public List<GridCell> GridCells { get; private set; }

        public GameFactory(IInstantiator instantiator, IResourceLoader resourceLoader, IStaticDataService staticData) :
            base(instantiator, resourceLoader)
        {
            _staticData = staticData;

            GridCells = new List<GridCell>();
            ProgressReaders = new List<IProgressReader>();
        }

        public void CreateInstanceRoot() =>
            _instanceRoot = CreateRootObject(InstanceRootName);

        public GameObject CreateHUD() =>
            Instantiate(AssetsPaths.HUD, Vector3.zero, _instanceRoot);

        public HexagonGrid CreateHexagonGrid(Grid grid)
        {
            GridRotator gridRotator = Instantiate<GridRotator>(AssetsPaths.GridRootPrefab, Vector3.zero, _instanceRoot);
            gridRotator.Initialize(_staticData.GameConfig.GridRotation);
            gridRotator.gameObject.name = "Grid";
            GridRotator = gridRotator;

            HexagonGrid hexagonGrid = gridRotator.GetComponent<HexagonGrid>();
            RegisterProgressReaders(hexagonGrid.gameObject);

            return hexagonGrid;
        }

        public MergeSystem CreateMergeSystem(StackMover stackMover, HexagonGrid hexagonGrid)
        {
            MergeSystem mergeSystem =
                Instantiate<MergeSystem>(AssetsPaths.MergeSystemPrefab, Vector3.zero, _instanceRoot);
            mergeSystem.Initialize(stackMover, hexagonGrid);
            MergeSystem = mergeSystem;

            return mergeSystem;
        }

        public StackMover CreateStackMover()
        {
            StackMover stackMover = Instantiate<StackMover>(AssetsPaths.StackMoverPrefab, Vector3.zero, _instanceRoot);
            StackMover = stackMover;

            return stackMover;
        }

        public GridCell CreateGridCell(Vector3 position, Vector2Int positionOnGrid, Transform parent, Color normalColor,
            Color highlightColor)
        {
            GridCell gridCell = Instantiate<GridCell>(AssetsPaths.GridCellPrefab, position, parent);
            gridCell.InitializeColors(normalColor, highlightColor);
            gridCell.InitializeGridPosition(positionOnGrid);

            GridCells.Add(gridCell);
            return gridCell;
        }

        private Transform CreateRootObject(string rootObjectName) =>
            new GameObject(rootObjectName).transform;

        private void RegisterProgressReaders(GameObject gameObject)
        {
            foreach (IProgressReader progressReader in gameObject.GetComponentsInChildren<IProgressReader>())
                ProgressReaders.Add(progressReader);
        }
    }
}