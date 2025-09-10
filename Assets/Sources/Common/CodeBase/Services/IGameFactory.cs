using Sources.Features.HexagonSort.GridGenerator.Scripts;
using Sources.Features.HexagonSort.Scripts;
using UnityEngine;

namespace Sources.Common.CodeBase.Services
{
    public interface IGameFactory
    {
        Hexagon CreateHexagon(Vector3 position, Transform parent);
        GameObject CreateHexagonStack(Vector3 position, Transform parent);
        GridGenerator CreateGridGenerator(GridTemplate template, Vector3 at);
        void CreateInstanceRoot();
        StackGenerator CreateStackGenerator(HexagonStackTemplate template, string levelName, Vector3 at);
    }
}