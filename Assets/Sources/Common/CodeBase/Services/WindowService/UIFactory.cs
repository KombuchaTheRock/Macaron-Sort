using Sources.Common.CodeBase.Paths;
using Sources.Common.CodeBase.Services.Factories;
using Sources.Common.CodeBase.Services.ResourceLoader;
using Sources.Common.CodeBase.Services.StaticData;
using UnityEngine;
using Zenject;

namespace Sources.Common.CodeBase.Services.WindowService
{
    public class UIFactory : BaseFactory, IUIFactory
    {
        private readonly IStaticDataService _staticData;
        private Transform _uiRoot;

        public UIFactory(IInstantiator instantiator, IResourceLoader resourceLoader, IStaticDataService staticData) : base(instantiator,
            resourceLoader)
        {
            _staticData = staticData;
        }

        public void CreateUIRoot()
        {
            GameObject tempParent = new();
            
            _uiRoot = Instantiate(AssetsPaths.UIRoot, Vector3.zero).transform;
            
            _uiRoot.SetParent(tempParent.transform,false);
            _uiRoot.SetParent(null, false);
            
            Object.Destroy(tempParent);
        }

        public void CreateGameOverWindow()
        {
            WindowConfig config = _staticData.ForWindow(WindowID.GameOver);
            Instantiate(config.Prefab.gameObject, _uiRoot );
        }

        public void CreatePauseWindow()
        {
            WindowConfig config = _staticData.ForWindow(WindowID.Pause);
            Instantiate(config.Prefab.gameObject, _uiRoot );
        }
        
        public void CreateRocketBoosterWindow()
        {
            WindowConfig config = _staticData.ForWindow(WindowID.RocketBooster);
            Instantiate(config.Prefab.gameObject, _uiRoot );
        }
        
        public void CreateArrowBoosterWindow()
        {
            WindowConfig config = _staticData.ForWindow(WindowID.ArrowBooster);
            Instantiate(config.Prefab.gameObject, _uiRoot );
        }
    }
}