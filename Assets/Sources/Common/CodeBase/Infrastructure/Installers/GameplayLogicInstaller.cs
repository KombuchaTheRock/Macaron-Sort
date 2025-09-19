using Sources.Features.HexagonSort.HexagonStack.StackMover.Scripts;
using Zenject;

namespace Sources.Common.CodeBase.Infrastructure.Installers
{
    public class GameplayLogicInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindStackSelectionLogic();
            BindStackDraggingLogic();
            BindStackPlacementLogic();
        }

        private void BindStackPlacementLogic()
        {
            Container.BindInterfacesTo<StackPlacementLogic>()
                .AsSingle();
        }

        private void BindStackDraggingLogic()
        {
            Container.BindInterfacesTo<StackDraggingLogic>()
                .AsSingle();
        }

        private void BindStackSelectionLogic()
        {
            Container.BindInterfacesTo<StackSelectionLogic>()
                .AsSingle();
        }
    }
}
