using System;
using System.Collections.Generic;
using System.Linq;
using Sources.Features.HexagonSort.GridSystem.GridGenerator.Scripts;
using Sources.Features.HexagonSort.HexagonStackSystem.Scripts;
using Sources.Features.HexagonSort.HexagonTile.Scripts;
using UnityEngine;

namespace Sources.Common.CodeBase.Services.PlayerProgress.Data
{
    [Serializable]
    public class StacksData
    {
        [field: SerializeField] public List<PlacedStackData> StacksOnGrid { get; private set; } 
        [field: SerializeField] public List<FreeStackDataData> FreeStacks { get; private set; }

        public StacksData(List<PlacedStackData> stacksOnGrid, List<FreeStackDataData> freeStacks)
        {
            StacksOnGrid = stacksOnGrid;
            FreeStacks = freeStacks;
        }
        
        public void UpdateStacksOnGridData(List<GridCell> cells)
        {
            StacksOnGrid.Clear();

            foreach (GridCell cell in cells)
            {
                if (cell.IsOccupied == false) 
                    continue;
                
                HexagonTileType[] tiles = cell.Stack.Hexagons.Select(x => x.TileType).ToArray();
                PlacedStackData placedStackData = new(tiles, cell.PositionOnGrid);
                
                StacksOnGrid.Add(placedStackData);
            }
        }

        public void UpdateFreeStacksData(List<HexagonStack> stacks)
        {
            FreeStacks.Clear();

            foreach (HexagonStack stack in stacks)
            {
                HexagonTileType[] tiles = stack.Hexagons.Select(x => x.TileType).ToArray();
                FreeStackDataData freeStackDataData = new(tiles,stack.InitialPosition);
                
                FreeStacks.Add(freeStackDataData);
            }
        }
    }
}