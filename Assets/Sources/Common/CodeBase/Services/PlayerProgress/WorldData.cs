using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sources.Features.HexagonSort.GridSystem.GridGenerator.Scripts;
using Sources.Features.HexagonSort.HexagonTile.Scripts;
using UnityEngine;

namespace Sources.Common.CodeBase.Services.PlayerProgress
{
    [Serializable]
    public class WorldData
    {
        [field: SerializeField] public GridData GridData { get; private set; }
    }

    [Serializable]
    public class GridData
    {
        [field: SerializeField] public List<PlacedStack> StacksOnGrid { get; private set; }

        public void UpdateData(List<GridCell> cells)
        {
            StacksOnGrid.Clear();

            foreach (GridCell cell in cells)
            {
                if (cell.IsOccupied)
                {
                    HexagonTileType[] tiles = cell.Stack.Hexagons.Select(x => x.TileType).ToArray();
                    PlacedStack placedStack = new(tiles, cell.PositionOnGrid);
                    StacksOnGrid.Add(placedStack);
                }
            }
            
            StringBuilder builder = new();

            foreach (PlacedStack placedStack in StacksOnGrid) 
                builder.AppendLine($"StackCount: {placedStack.Tiles.Length} Position: {placedStack.PositionOnGrid}");
            
            Debug.Log(builder.ToString());
            Debug.Log(StacksOnGrid.Count);
        }
    }
}