using Sources.Common.CodeBase.Infrastructure.StateMachine.States;
using Sources.Features.HexagonSort.GridSystem.GridGenerator.Scripts;
using Sources.Features.HexagonSort.HexagonStackSystem.StackGenerator.Scripts;
using Sources.Features.HexagonSort.HexagonStackSystem.StackMover.Scripts;
using Sources.Features.HexagonSort.Merge.Scripts;
using Sources.Features.HexagonSort.StackSelector;
using Zenject;

namespace Sources.Common.CodeBase.Infrastructure.Installers
{
    public class GameplayLogicInstaller : MonoInstaller, ICoroutineRunner
    {
        public override void InstallBindings()
        {
            BindGridGenerator();
            BindStackGenerator();
            BindStackMover();
            BindStacksSpawner();
            BindStackMerger();
            BindStackCompleter();
            BindBoosterCounter();
            BindBoosterActivator();
        }

        private void BindBoosterCounter()
        {
            Container.BindInterfacesTo<BoosterCounter>()
                .AsSingle();
        }

        private void BindStackCompleter()
        {
            Container.BindInterfacesTo<StackCompletionLogic>()
                .AsSingle();
            
            Container.BindInterfacesTo<StackCompleter>()
                .AsSingle()
                .WithArguments(this);
        }

        private void BindBoosterActivator()
        {
            Container.BindInterfacesTo<BoosterActivator>()
                .AsSingle();
        }

        private void BindStackMerger()
        {
            Container.BindInterfacesTo<StackMerger>()
                .AsSingle()
                .WithArguments(this);
        }

        private void BindGridGenerator()
        {
            Container.BindInterfacesTo<GridGenerator>()
                .AsSingle();
        }
        
        private void BindStackGenerator()
        {
            Container.BindInterfacesTo<StackGenerator>()
                .AsSingle();
        }
        
        private void BindStackMover()
        {
            Container.BindInterfacesTo<StackPlacementLogic>()
                .AsSingle();

            Container.BindInterfacesTo<StackDraggingLogic>()
                .AsSingle();

            Container.BindInterfacesTo<StackSelectionLogic>()
                .AsSingle();

            Container.BindInterfacesTo<StackMover>()
                .AsSingle();
        }

        private void BindStacksSpawner()
        {
            Container.BindInterfacesTo<StackSpawner>()
                .AsSingle()
                .WithArguments(this);
        }
    }
}