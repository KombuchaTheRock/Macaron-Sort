using System;

namespace Sources.Common.CodeBase.Infrastructure.Extensions
{
    public static class EnumExtensions
    {
        public static T GetRandomValue<T>() where T : Enum
        {
            Array values = Enum.GetValues(typeof(T));
            return (T)values.GetValue(UnityEngine.Random.Range(0, values.Length));
        }
    }
}