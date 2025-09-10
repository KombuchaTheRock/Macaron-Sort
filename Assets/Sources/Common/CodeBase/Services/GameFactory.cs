using Sources.Features.HexagonSort.GridGenerator.Scripts;
using Sources.Features.HexagonSort.Scripts;
using UnityEngine;
using Zenject;

namespace Sources.Common.CodeBase.Services
{
    public class GameFactory : IGameFactory
    {
        private const string InstanceRootName = "InstanceRoot";
        
        private Transform _instanceRoot;

        private readonly IInstantiator _instantiator;
        private readonly IResourceLoader _resourceLoader;
        private readonly IStaticDataService _staticData;

        public GameFactory(IInstantiator instantiator, IResourceLoader resourceLoader, IStaticDataService staticData)
        {
            _instantiator = instantiator;
            _resourceLoader = resourceLoader;
            _staticData = staticData;
        }

        public void CreateInstanceRoot() => 
            _instanceRoot = new GameObject(InstanceRootName).transform;

        public StackGenerator CreateStackGenerator(HexagonStackTemplate template, string levelName, Vector3 at)
        {
            StackGenerator gridGenerator = Instantiate<StackGenerator>(AssetsPaths.StackGeneratorPrefab, at, _instanceRoot);
            
            gridGenerator.Initialize(_staticData.ForHexagonStack(template), 
                _staticData.ForLevel(levelName).StackSpawnPoints);

            return gridGenerator;
        }
        
        public GridGenerator CreateGridGenerator(GridTemplate template, Vector3 at)
        {
            GridGenerator gridGenerator = Instantiate<GridGenerator>(AssetsPaths.GridGeneratorPrefab, at, _instanceRoot);
            gridGenerator.Initialize(_staticData.ForGrid(template));

            return gridGenerator;
        }

        public GameObject CreateHexagonStack(Vector3 position, Transform parent) =>
            Instantiate(AssetsPaths.StackPrefab, position, parent);

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