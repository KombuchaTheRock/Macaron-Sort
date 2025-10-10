using Sources.Features.HexagonSort.GridSystem.Scripts;

namespace Sources.Features.HexagonSort.BoosterSystem.Activation
{
    public interface IBoosterActivator
    {
        void Initialize(BoosterPicker boosterPicker, HexagonGrid hexagonGrid);
        void Reset();
    }
}