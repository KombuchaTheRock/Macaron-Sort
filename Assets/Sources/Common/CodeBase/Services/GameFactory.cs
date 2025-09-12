using System.Collections.Generic;
using Sources.Common.CodeBase.Paths;
using Sources.Features.HexagonSort.GridGenerator.Scripts;
using Sources.Features.HexagonSort.HexagonTile.Scripts;
using Sources.Features.HexagonSort.Scripts;
using Sources.Features.HexagonSort.StackGenerator.Scripts;
using UnityEngine;
using Zenject;

namespace Sources.Common.CodeBase.Services
{
    public class GameFactory : IGameFactory
    {
        private const string InstanceRootName = "InstanceRoot";
        
        public List<HexagonStack> Stacks { get; private set; }
        public StackGenerator StackGenerator { get; private set; }

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
        }

        public void CreateInstanceRoot() => 
            _instanceRoot = new GameObject(InstanceRootName).transform;

        public StackGenerator CreateStackGenerator(HexagonStackTemplate template, string levelName, Vector3 at)
        {
            StackGenerator stackGenerator = Instantiate<StackGenerator>(AssetsPaths.StackGeneratorPrefab, at, _instanceRoot);
            
            stackGenerator.Initialize(_staticData.ForHexagonStack(template), 
                _staticData.ForLevel(levelName).StackSpawnPoints);

            StackGenerator = stackGenerator;
            
            return stackGenerator;
        }
        
        public GridGenerator CreateGridGenerator(GridTemplate template, Vector3 at)
        {
            GridGenerator gridGenerator = Instantiate<GridGenerator>(AssetsPaths.GridGeneratorPrefab, at, _instanceRoot);
            gridGenerator.Initialize(_staticData.ForGrid(template));

            return gridGenerator;
        }

        
        public HexagonStack CreateHexagonStack(Vector3 position, Transform parent)
        {
            HexagonStack hexagonStack = Instantiate<HexagonStack>(AssetsPaths.StackPrefab, position, parent);
            Stacks.Add(hexagonStack);
            return hexagonStack;
        }

        public GridCell CreateGridCell(Vector3 position, Transform parent) =>
            Instantiate<GridCell>(AssetsPaths.GridCellPrefab, position, parent);
        
        public Hexagon CreateHexagon(Vector3 position, Transform parent) =>
            Instantiate<Hexagon>(AssetsPaths.HexagonPrefab, position, parent);

        private GameObject Instantiate(string assetPath, Vector3 at, Transform parent = null)
        {
            GameObject prefab = _resourceLoader.LoadAsset<GameObject>(assetPath);
            return _instantiator.InstantiatePrefab(prefab, at, Quaternion.identity, parent);
        }

        private T Instantiate<T>(string assetPath, Vector3 at, Transform parent = null) where T : Component
        {
            GameObject prefab = _resourceLoader.LoadAsset<GameObject>(assetPath);
            return _instantiator.InstantiatePrefabForComponent<T>(prefab, at, Quaternion.identity, parent);
        }
    }
}