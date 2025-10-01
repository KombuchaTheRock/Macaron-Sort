using NaughtyAttributes;
using Sources.Common.CodeBase.Services.PlayerProgress;
using Sources.Features.HexagonSort.GridSystem.GridGenerator.Scripts;
using Sources.Features.HexagonSort.GridSystem.Scripts;
using Sources.Features.HexagonSort.HexagonStackSystem.StackMover.Scripts;
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
        [field: SerializeField, Expandable] public PlayerLevelConfig PlayerLevelConfig { get; private set; }
        [field: SerializeField, Expandable] public StacksSpawnerConfig StacksSpawnerConfig { get; private set; }
        
    }
}