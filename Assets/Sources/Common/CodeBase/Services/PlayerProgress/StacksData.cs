using System;
using System.Collections.Generic;
using System.Linq;
using Sources.Features.HexagonSort.GridSystem.GridGenerator.Scripts;
using Sources.Features.HexagonSort.HexagonStackSystem.Scripts;
using Sources.Features.HexagonSort.HexagonTile.Scripts;
using UnityEngine;

namespace Sources.Common.CodeBase.Services.PlayerProgress
{
    [Serializable]
    public class StacksData
    {
        [field: SerializeField] public List<PlacedStack> StacksOnGrid { get; private set; }
        [field: SerializeField] public List<FreeStack> FreeStacks { get; private set; }

        public StacksData()
        {
            StacksOnGrid = new List<PlacedStack>();
            FreeStacks = new List<FreeStack>();
        }

        public StacksData(List<PlacedStack> stacksOnGrid, List<FreeStack> freeStacks)
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
                PlacedStack placedStack = new(tiles, cell.PositionOnGrid);
                
                StacksOnGrid.Add(placedStack);
            }
        }

        public void UpdateFreeStacksData(List<HexagonStack> stacks)
        {
            FreeStacks.Clear();

            foreach (HexagonStack stack in stacks)
            {
                HexagonTileType[] tiles = stack.Hexagons.Select(x => x.TileType).ToArray();
                FreeStack freeStack = new(tiles,stack.InitialPosition);
                
                FreeStacks.Add(freeStack);
            }
        }
    }
}