namespace Sources.Features.HexagonSort.Grid.GridGenerator.Scripts
{
    public interface IGridGenerator
    {
        public void GenerateGrid(UnityEngine.Grid grid, int gridSize, CellConfig cellConfig);
    }
}