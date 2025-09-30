using System.Collections.Generic;
using Sources.Common.CodeBase.Paths;
using Sources.Common.CodeBase.Services.Factories;
using Sources.Common.CodeBase.Services.ResourceLoader;
using Sources.Common.CodeBase.Services.Settings;
using Sources.Common.CodeBase.Services.StaticData;
using UnityEngine;
using Zenject;

namespace Sources.Common.CodeBase.Services.WindowService
{
    public class UIFactory : BaseFactory, IUIFactory
    {
        private readonly IStaticDataService _staticData;
        private Transform _uiRoot;

        public List<ISettingsReader> SettingsReaders { get; }
        
        public UIFactory(IInstantiator instantiator, IResourceLoader resourceLoader, IStaticDataService staticData) : base(instantiator,
            resourceLoader)
        {
            _staticData = staticData;

            SettingsReaders = new List<ISettingsReader>();
        }

        public void CreateUIRoot()
        {
            GameObject tempParent = new();
            _uiRoot = Instantiate(AssetsPaths.UIRoot, Vector3.zero, tempParent.transform).transform;
            _uiRoot.parent = null;
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
        
        private void RegisterSettingsReader(GameObject gameObject)
        {
            foreach (ISettingsReader progressReader in gameObject.GetComponentsInChildren<ISettingsReader>())
                SettingsReaders.Add(progressReader);
        }
    }
}