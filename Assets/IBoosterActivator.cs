using Sources.Features.HexagonSort.GridSystem.Scripts;

public interface IBoosterActivator
{
    void Initialize(BoosterPicker boosterPicker, HexagonGrid hexagonGrid);
    void Reset();
}