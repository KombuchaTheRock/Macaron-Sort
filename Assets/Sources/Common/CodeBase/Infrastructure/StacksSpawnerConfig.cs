using UnityEngine;

namespace Sources.Common.CodeBase.Infrastructure
{
    [CreateAssetMenu(menuName = "StaticData/StacksSpawnerConfig", fileName = "StacksSpawnerConfig", order = 0)]
    public class StacksSpawnerConfig : ScriptableObject
    {
        [field: SerializeField] public float DelayBetweenStacks { get; private set; }

        private void OnValidate()
        {
            if (DelayBetweenStacks < 0) 
                DelayBetweenStacks = 0;
        }
    }
}