using System;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Sources.Common.CodeBase.Infrastructure.Extensions
{
    public static class EnumExtensions
    {
        public static T GetRandomValue<T>() where T : Enum
        {
            Array values = Enum.GetValues(typeof(T));
            return (T)values.GetValue(Random.Range(0, values.Length));
        }

        public static T[] GetRandomValues<T>(int count) where T : Enum
        {
            T[] allValues = (T[])Enum.GetValues(typeof(T));
            count = Mathf.Min(count, allValues.Length);

            return allValues.OrderBy(x => Random.Range(0, int.MaxValue))
                .Take(count)
                .ToArray();
        }

        public static T[] GetAllValues<T>() where T : Enum =>
            (T[])Enum.GetValues(typeof(T));
    }
}