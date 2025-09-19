using NaughtyAttributes;
using Sources.Features.HexagonSort.Grid.GridGenerator.Scripts;
using Sources.Features.HexagonSort.Grid.Scripts;
using Sources.Features.HexagonSort.HexagonStack.StackMover.Scripts;
using Sources.Features.Level.Scripts;
using UnityEngine;

namespace Sources.Common.CodeBase.Infrastructure
{
    [CreateAssetMenu(menuName = "StaticData/GameConfig", fileName = "GameConfig", order = 0)]
    public class GameConfig : ScriptableObject
    {
        [field: SerializeField, Expandable] public GridConfig GridConfig { get; private set; }
        [field: SerializeField, Expandable] public GridRotationConfig GridRotation { get; private set; }
        [field: SerializeField, Expandable] public LevelConfig LevelConfig { get; private set; }
        [field: SerializeField, Expandable] public StackMoverConfig StackMoverConfig { get; private set; }
    }
}