using UnityEngine;

namespace Sources.Common.CodeBase.Services.PlayerProgress
{
    [CreateAssetMenu(menuName = "StaticData/PlayerLevelConfig", fileName = "PlayerLevelConfig", order = 0)]
    public class PlayerLevelConfig : ScriptableObject
    {
        [field: SerializeField] public float BaseMaxScoreForLevel { get; private set; }
        [field: SerializeField] public float MaxScoreGrowthFactor { get; private set; }
        [field: SerializeField] public int ControlPointSaveInterval { get; private set; }
    }
}