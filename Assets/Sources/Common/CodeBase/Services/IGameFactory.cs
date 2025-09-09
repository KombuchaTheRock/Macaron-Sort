using UnityEngine;

namespace Sources.Common.CodeBase.Services
{
    public interface IGameFactory
    {
        GameObject CreateHexagon(Vector3 position, Transform parent);
        GameObject CreateHexagonStack(Vector3 position, Transform parent);
    }
}