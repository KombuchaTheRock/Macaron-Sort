using System.Collections.Generic;
using Sources.Common.CodeBase.Paths;
using Sources.Features.HexagonSort.GridGenerator.Scripts;
using Sources.Features.HexagonSort.HexagonTile.Scripts;
using Sources.Features.HexagonSort.StackGenerator.Scripts;
using UnityEngine;
using Zenject;

namespace Sources.Common.CodeBase.Services
{
    public class GameFactory : IGameFactory
    {
        private const string InstanceRootName = "GameFactoryRoot";
        private const string GridRootName = "Grid";
        private const string StacksRootName = "Stacks";
        
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
            _instanceRoot = CreateRootObject(InstanceRootName);

        public Transform CreateGridRoot()
        {
            Transform rootObject = CreateRootObject(GridRootName);
            rootObject.SetParent(_instanceRoot);
            
            return rootObject;
        }

        public Transform CreateStacksRoot()
        {
            Transform rootObject = CreateRootObject(StacksRootName);
            rootObject.SetParent(_instanceRoot);
            
            return rootObject;
        }

        public HexagonStack CreateHexagonStack(Vector3 position, Transform parent)
        {
            HexagonStack hexagonStack = Instantiate<HexagonStack>(AssetsPaths.StackPrefab, position, parent);
            
            Stacks.Add(hexagonStack);
            return hexagonStack;
        }

        public GridCell CreateGridCell(Vector3 position, Transform parent, Color normalColor, Color highlightColor)
        {
            GridCell gridCell = Instantiate<GridCell>(AssetsPaths.GridCellPrefab, position, parent);
            gridCell.SetColors(normalColor, highlightColor);
            gridCell.GetComponent<MeshColor>().Set(normalColor);
            
            return gridCell;
        }

        public Hexagon CreateHexagon(Vector3 position, Transform parent, Color color)
        {
            Hexagon hexagon = Instantiate<Hexagon>(AssetsPaths.HexagonPrefab, position, parent);
            hexagon.GetComponent<MeshColor>().Set(color);
            
            return hexagon;
        }

        private Transform CreateRootObject(string rootObjectName) => 
            new GameObject(rootObjectName).transform;

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