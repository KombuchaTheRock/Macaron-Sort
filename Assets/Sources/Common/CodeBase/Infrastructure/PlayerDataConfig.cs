using UnityEngine;

namespace Sources.Common.CodeBase.Infrastructure
{
    [CreateAssetMenu(menuName = "StaticData/PlayerDataConfig", fileName = "PlayerDataConfig", order = 0)]
    public class PlayerDataConfig : ScriptableObject
    {
        [field: SerializeField] public int InitialLevel { get; private set; }
        [field: SerializeField] public int InitialScore { get; private set; }
    }
}