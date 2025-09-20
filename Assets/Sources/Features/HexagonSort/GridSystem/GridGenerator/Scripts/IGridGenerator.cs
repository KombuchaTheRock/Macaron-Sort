using Sources.Features.HexagonSort.GridSystem.Scripts;
using UnityEngine;

namespace Sources.Features.HexagonSort.GridSystem.GridGenerator.Scripts
{
    public interface IGridGenerator
    {
        public HexagonGrid GenerateGrid(Grid grid, int gridSize, CellConfig cellConfig);
    }
}