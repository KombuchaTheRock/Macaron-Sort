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

        public WindowBase CreateGameOverWindow()
        {
            WindowConfig config = _staticData.ForWindow(WindowID.GameOver);
            return Instantiate<WindowBase>(config.Prefab.gameObject, _uiRoot );
        }

        public WindowBase CreatePauseWindow()
        {
            WindowConfig config = _staticData.ForWindow(WindowID.Pause);
            return Instantiate<WindowBase>(config.Prefab.gameObject, _uiRoot );
        }
        
        public WindowBase CreateRocketBoosterWindow()
        {
            WindowConfig config = _staticData.ForWindow(WindowID.RocketBooster);
            return Instantiate<WindowBase>(config.Prefab.gameObject, _uiRoot );
        }
        
        public WindowBase CreateArrowBoosterWindow()
        {
            WindowConfig config = _staticData.ForWindow(WindowID.ArrowBooster);
            return Instantiate<WindowBase>(config.Prefab.gameObject, _uiRoot );
        }
    }
}