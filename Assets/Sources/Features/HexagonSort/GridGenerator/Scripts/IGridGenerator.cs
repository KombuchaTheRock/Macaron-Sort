using UnityEngine;

namespace Sources.Features.HexagonSort.GridGenerator.Scripts
{
    public interface IGridGenerator
    {
        public void GenerateGrid(Grid grid, int gridSize, CellConfig cellConfig);
    }
}