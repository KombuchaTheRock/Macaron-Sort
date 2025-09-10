using Sources.Features.HexagonSort.Scripts;
using UnityEngine;
using Zenject;

namespace Sources.Common.CodeBase.Services
{
    public class GameFactory : IGameFactory
    {
        private readonly IInstantiator _instantiator;

        public GameFactory(IInstantiator instantiator)
        {
            _instantiator = instantiator;
        }
        
        public GameObject CreateHexagonStack(Vector3 position, Transform parent) => 
            Instantiate(Assets.HexagonPrefab, position, parent);

        public Hexagon CreateHexagon(Vector3 position, Transform parent) => 
            Instantiate<Hexagon>(Assets.HexagonPrefab, position, parent);

        private T Instantiate<T>(string assetPath, Vector3 at, Transform parent = null) where T : Component
        {
            GameObject prefab = Resources.Load<GameObject>(assetPath);
            return _instantiator.InstantiatePrefabForComponent<T>(prefab, at, Quaternion.identity, parent);
        }
        
        private GameObject Instantiate(string assetPath, Vector3 at, Transform parent = null)
        {
            GameObject prefab = Resources.Load<GameObject>(assetPath);
            return _instantiator.InstantiatePrefab(prefab, at, Quaternion.identity, parent);
        }
    }
}