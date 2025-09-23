using System;
using System.Linq;
using Sources.Common.CodeBase.Infrastructure.Extensions;
using Sources.Features.HexagonSort.HexagonTile.Scripts;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Sources.Features.HexagonSort.HexagonStackSystem.StackGenerator.Scripts
{
    public static class TileShuffler
    {
        public static HexagonTileType[] GetRandomTileTypes(int amount, int maxTileChanges)
        {
            HexagonTileType[] tileTypes = new HexagonTileType[amount];
            int numberOfIndexes = Random.Range(1, maxTileChanges);
            
            int[] indexes = GetRandomDividingIndices(amount, numberOfIndexes);

            HexagonTileType[] randomTiles = EnumExtensions.GetRandomValues<HexagonTileType>(maxTileChanges);
            HexagonTileType currentTileType = randomTiles[0];

            if (amount == 1)
                return tileTypes;
            
            for (int i = 0, j = 0; i < tileTypes.Length; i++)
            {
                if (i == indexes[j])
                {
                    currentTileType = randomTiles[Random.Range(0, randomTiles.Length)];

                    if (j < indexes.Length - 1)
                        j++;
                }

                tileTypes[i] = currentTileType;
            }

            return tileTypes;
        }

        private static int[] GetRandomDividingIndices(int arrayLength, int numberOfIndices)
        {
            if (arrayLength <= 0 || numberOfIndices <= 0)
                return Array.Empty<int>();

            numberOfIndices = Mathf.Min(numberOfIndices, arrayLength - 1);

            int minDistance = arrayLength / (numberOfIndices + 1);

            int[] indices = new int[numberOfIndices];
            int currentPosition = minDistance;

            for (int i = 0; i < numberOfIndices; i++)
            {
                int maxOffset = Mathf.Min(minDistance / 2, arrayLength - currentPosition - 1);
                int offset = Random.Range(-maxOffset, maxOffset + 1);

                indices[i] = Mathf.Clamp(currentPosition + offset, 0, arrayLength - 1);
                currentPosition += minDistance;
            }

            return indices.Distinct().OrderBy(x => x).ToArray();
        }
    }
}