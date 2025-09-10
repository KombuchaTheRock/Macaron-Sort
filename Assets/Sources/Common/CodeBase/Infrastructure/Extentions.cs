using System;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

namespace Sources.Common.CodeBase.Infrastructure
{
    public static class Extensions
    {
        public static int[] GetUniqueRandomIndexes(this Array array, int count)
        {
            if (array.Length <= 0 || count <= 0)
                return Array.Empty<int>();

            if (count > array.Length)
                throw new ArgumentException("Count cannot be greater than array length");

            List<int> allIndexes = Enumerable.Range(0, array.Length).ToList();
            int[] result = new int[count];

            for (int i = 0; i < count; i++)
            {
                int randomIndex = Random.Range(0, allIndexes.Count);
                result[i] = allIndexes[randomIndex];
                allIndexes.RemoveAt(randomIndex);
            }

            return result;
        }
    }
}