using Sources.Common.CodeBase.Infrastructure.StateMachine.States;
using Sources.Features.HexagonSort.GridSystem.GridGenerator.Scripts;
using Sources.Features.HexagonSort.HexagonStackSystem.StackGenerator.Scripts;
using Sources.Features.HexagonSort.HexagonStackSystem.StackMover.Scripts;
using Sources.Features.HexagonSort.Merge.Scripts;
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
            Container.BindInterfacesTo<StacksPlacer>()
                .AsSingle()
                .WithArguments(this);
        }
    }
}