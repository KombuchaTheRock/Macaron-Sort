using Sources.Common.CodeBase.Infrastructure.StateMachine.States;
using Sources.Common.CodeBase.Services.Factories.GameFactory;
using Sources.Common.CodeBase.Services.Factories.HexagonFactory;
using Sources.Common.CodeBase.Services.InputService;
using Sources.Common.CodeBase.Services.PlayerProgress;
using Sources.Common.CodeBase.Services.ResourceLoader;
using Sources.Common.CodeBase.Services.SaveService;
using Sources.Common.CodeBase.Services.Settings;
using Sources.Common.CodeBase.Services.SoundService;
using Sources.Common.CodeBase.Services.StaticData;
using Sources.Common.CodeBase.Services.WindowService;
using Sources.Features.HexagonSort.GridSystem.GridGenerator.Scripts;
using Sources.Features.HexagonSort.HexagonStackSystem.StackGenerator.Scripts;
using UnityEngine;
using Zenject;

namespace Sources.Common.CodeBase.Infrastructure.Installers
{
    public class InfrastructureInstaller : MonoInstaller
    {
        [SerializeField] private SoundPool _soundPool;
        
        public override void InstallBindings()
        {
            BindGameStateFactory();
            BindSceneLoader();
            BindResourceLoader();
            BindStaticDataService();
            
            
            BindGameFactory();
            BindHexagonFactory();
            BindAudioFactory();
            BindUIFactory();
            
            BindInputService();
            BindStackGenerator();
            BindGridGenerator();
            BindSaveDataFactory();
            BindGameProgress();
            BindPlayerLevel();
            BindSoundService();
            BindGameSettingsSaveLoader();
            BindGameSettings();
            BindWindowService();
        }

        private void BindWindowService()
        {
            Container.Bind<IWindowService>()
                .To<WindowService>()
                .AsSingle();
        }

        private void BindUIFactory()
        {
            Container.Bind<IUIFactory>()
                .To<UIFactory>()
                .AsSingle()
                .WithArguments(Container);
        }

        private void BindGameSettingsSaveLoader()
        {
            Container.Bind<ISettingsSaveLoader>()
                .To<SettingsSaveLoader>()
                .AsSingle();
        }

        private void BindGameSettings()
        {
            Container.Bind<IGameSettings>()
                .To<GameSettings>()
                .AsSingle();
        }

        private void BindSoundService()
        {
            Container.Bind<ISoundService>()
                .To<SoundService>()
                .AsSingle()
                .WithArguments(_soundPool);
        }

        private void BindAudioFactory()
        {
            Container.Bind<IAudioFactory>()
                .To<AudioFactory>()
                .AsSingle()
                .WithArguments(Container);
        }

        private void BindHexagonFactory()
        {
            Container.Bind<IHexagonFactory>()
                .To<HexagonFactory>()
                .AsSingle()
                .WithArguments(Container);
        }

        private void BindSaveDataFactory()
        {
            Container.Bind<ISaveDataFactory>()
                .To<SaveDataFactory>()
                .AsSingle();
        }

        private void BindPlayerLevel()
        {
            Container.BindInterfacesTo<PlayerLevel>()
                .AsSingle();
        }

        private void BindGameProgress()
        {
            Container.Bind<ISerializer>()
                .To<JsonUtilitySerializer>()
                .AsSingle();
        
            Container.Bind<ISaveSystem>()
                .To<SaveSystem>()
                .AsSingle();

            Container.Bind<IGameProgressService>()
                .To<GameProgressService>()
                .AsSingle();
        }

        private void BindGridGenerator()
        {
            Container.BindInterfacesTo<GridGenerator>()
                .AsSingle();
        }

        private void BindStackGenerator()
        {
            ICoroutineRunner coroutineRunner = CreateCoroutineRunner();
            
            Container.BindInterfacesTo<StackGenerator>()
                .AsSingle()
                .WithArguments(coroutineRunner);
        }

        private void BindInputService()
        {
            if (Application.isEditor)
                Container.BindInterfacesTo<StandaloneInput>()
                    .AsSingle();
            else
                Container.BindInterfacesTo<MobileInput>()
                    .AsSingle();
        }

        private void BindGameFactory()
        {
            Container.Bind<IGameFactory>()
                .To<GameFactory>()
                .AsSingle()
                .WithArguments(Container);
        }

        private void BindSceneLoader()
        {
            Container.Bind<SceneLoader>()
                .AsSingle()
                .WithArguments(this);
        }

        private void BindGameStateFactory()
        {
            Container.Bind<GameStateFactory>()
                .AsSingle();
        }

        private void BindResourceLoader()
        {
            Container.Bind<IResourceLoader>()
                .To<ResourceLoader>()
                .AsSingle();
        }

        private void BindStaticDataService()
        {
            Container.BindInterfacesTo<StaticDataService>()
                .AsSingle();
        }
        
        private ICoroutineRunner CreateCoroutineRunner()
        {
            GameObject go = new("CoroutineRunner");
            DontDestroyOnLoad(go);
            return go.AddComponent<CoroutineRunner>();
        }
    }
}