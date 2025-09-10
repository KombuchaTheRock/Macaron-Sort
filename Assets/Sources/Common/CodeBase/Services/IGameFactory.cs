using Sources.Features.HexagonSort.Scripts;
using UnityEngine;

namespace Sources.Common.CodeBase.Services
{
    public interface IGameFactory
    {
        Hexagon CreateHexagon(Vector3 position, Transform parent);
        GameObject CreateHexagonStack(Vector3 position, Transform parent);
    }
}