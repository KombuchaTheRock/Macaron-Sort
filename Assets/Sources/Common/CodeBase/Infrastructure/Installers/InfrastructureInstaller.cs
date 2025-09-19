using Sources.Common.CodeBase.Infrastructure.StateMachine.States;
using Sources.Common.CodeBase.Services;
using Sources.Features.HexagonSort.Grid.GridGenerator.Scripts;
using Sources.Features.HexagonSort.HexagonStack.StackGenerator.Scripts;
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