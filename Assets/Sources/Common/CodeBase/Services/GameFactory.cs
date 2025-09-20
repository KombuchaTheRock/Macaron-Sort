using System.Collections.Generic;
using Sources.Common.CodeBase.Paths;
using Sources.Features.HexagonSort.GridSystem.GridGenerator.Scripts;
using Sources.Features.HexagonSort.GridSystem.Scripts;
using Sources.Features.HexagonSort.HexagonStackSystem.StackGenerator.Scripts;
using Sources.Features.HexagonSort.HexagonStackSystem.StackMover.Scripts;
using Sources.Features.HexagonSort.HexagonTile.Scripts;
using UnityEngine;
using Zenject;

namespace Sources.Common.CodeBase.Services
{
    public class GameFactory : IGameFactory
    {
        private const string InstanceRootName = "GameFactoryRoot";
        private const string StacksRootName = "Stacks";
        
        
        public StackMover StackMover { get; private set; }
        public GridRotator GridRotator { get; private set; }
        public List<HexagonStack> Stacks { get; private set; }
        public List<GridCell> GridCells { get; private set; }

        private Transform _instanceRoot;

        private readonly IInstantiator _instantiator;
        private readonly IResourceLoader _resourceLoader;
        private readonly IStaticDataService _staticData;

        public GameFactory(IInstantiator instantiator, IResourceLoader resourceLoader, IStaticDataService staticData)
        {
            _instantiator = instantiator;
            _resourceLoader = resourceLoader;
            _staticData = staticData;
            
            Stacks = new List<HexagonStack>();
            GridCells = new List<GridCell>();
        }

        public void CreateInstanceRoot() => 
            _instanceRoot = CreateRootObject(InstanceRootName);

        public HexagonGrid CreateHexagonGrid(Grid grid)
        {
            GridRotator gridRotator = Instantiate<GridRotator>(AssetsPaths.GridRootPrefab, Vector3.zero, _instanceRoot);;
            gridRotator.Initialize(_staticData.GameConfig.GridRotation);
            gridRotator.gameObject.name = "Grid";
            GridRotator = gridRotator;
            
            HexagonGrid hexagonGrid = gridRotator.GetComponent<HexagonGrid>();
            hexagonGrid.Initialize(grid);
            
            return hexagonGrid;
        }

        public MergeSystem CreateMergeSystem(StackMover stackMover, HexagonGrid hexagonGrid)
        {
            MergeSystem mergeSystem = Instantiate<MergeSystem>(AssetsPaths.MergeSystemPrefab, Vector3.zero, _instanceRoot);
            mergeSystem.Initialize(stackMover, hexagonGrid);
            
            return mergeSystem;
        }
        
        public StackMover CreateStackMover()
        {
            StackMover stackMover = Instantiate<StackMover>(AssetsPaths.StackMoverPrefab, Vector3.zero, _instanceRoot);
            StackMover = stackMover;
            
            return stackMover;
        }

        public Transform CreateStacksRoot()
        {
            Transform rootObject = CreateRootObject(StacksRootName);
            rootObject.SetParent(_instanceRoot);
            
            return rootObject;
        }

        public HexagonStack CreateHexagonStack(Vector3 position, Transform parent, float offsetBetweenTiles)
        {
            HexagonStack hexagonStack = Instantiate<HexagonStack>(AssetsPaths.StackPrefab, position, parent);
            hexagonStack.SetInitialPosition(position);
            hexagonStack.SetOffsetBetweenTiles(offsetBetweenTiles);
            
            Stacks.Add(hexagonStack);
            return hexagonStack;
        }

        public GridCell CreateGridCell(Vector3 position, Vector2Int positionOnGrid, Transform parent, Color normalColor, Color highlightColor)
        {
            GridCell gridCell = Instantiate<GridCell>(AssetsPaths.GridCellPrefab, position, parent);
            gridCell.InitializeColors(normalColor, highlightColor);
            gridCell.InitializeGridPosition(positionOnGrid);
            
            GridCells.Add(gridCell);
            return gridCell;
        }

        public Hexagon CreateHexagon(Vector3 position, HexagonTileType tileType, Transform parent)
        {
            Hexagon hexagonPrefab = _staticData.ForHexagonTle(tileType).HexagonPrefab;
            Hexagon hexagon = Instantiate<Hexagon>(hexagonPrefab.gameObject, position, parent);
            hexagon.SetTileType(tileType);
            
            return hexagon;
        }

        private Transform CreateRootObject(string rootObjectName) => 
            new GameObject(rootObjectName).transform;

        private GameObject Instantiate(GameObject prefab, Vector3 at, Transform parent = null) => 
            _instantiator.InstantiatePrefab(prefab, at, Quaternion.identity, parent);

        private T Instantiate<T>(string assetPath, Vector3 at, Transform parent = null) where T : Component
        {
            GameObject prefab = _resourceLoader.LoadAsset<GameObject>(assetPath);
            return _instantiator.InstantiatePrefabForComponent<T>(prefab, at, Quaternion.identity, parent);
        }
        
        private T Instantiate<T>(GameObject prefab, Vector3 at, Transform parent = null) where T : Component => 
            _instantiator.InstantiatePrefabForComponent<T>(prefab, at, Quaternion.identity, parent);
    }
}