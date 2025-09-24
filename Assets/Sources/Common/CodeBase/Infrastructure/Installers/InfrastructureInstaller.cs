using Sources.Common.CodeBase.Infrastructure.StateMachine.States;
using Sources.Common.CodeBase.Services;
using Sources.Common.CodeBase.Services.PlayerProgress;
using Sources.Common.CodeBase.Services.SaveService;
using Sources.Features.HexagonSort.GridSystem.GridGenerator.Scripts;
using Sources.Features.HexagonSort.HexagonStackSystem.StackGenerator.Scripts;
using UnityEngine;
using Zenject;

namespace Sources.Common.CodeBase.Infrastructure.Installers
{
    public class InfrastructureInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindGameStateFactory();
            BindSceneLoader();
            BindResourceLoader();
            BindStaticDataService();
            BindGameFactory();
            BindInputService();
            BindStackGenerator();
            BindGridGenerator();
            BindSaveDataFactory();
            BindGameProgress();
            BindPlayerLevel();
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