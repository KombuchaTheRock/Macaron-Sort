using Sources.Common.CodeBase.Infrastructure.StateMachine.States;
using Sources.Features.HexagonSort.HexagonStackSystem.StackMover.Scripts;
using Zenject;

namespace Sources.Common.CodeBase.Infrastructure.Installers
{
    public class GameplayLogicInstaller : MonoInstaller, ICoroutineRunner
    {
        public override void InstallBindings()
        {
            BindStackMover();
            BindStacksSpawner();
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
            Container.BindInterfacesTo<StacksSpawner>()
                .AsSingle()
                .WithArguments(this);
        }
    }
}