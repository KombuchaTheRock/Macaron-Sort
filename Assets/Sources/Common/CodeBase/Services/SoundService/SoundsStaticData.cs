using UnityEngine;

namespace Sources.Common.CodeBase.Services.SoundService
{
    [CreateAssetMenu(menuName = "StaticData/SoundsStaticData", fileName = "SoundsStaticData", order = 0)]
    public class SoundsStaticData : ScriptableObject
    {
        [field: SerializeField] public Sound StackStartDraggingSound { get; private set; }
        [field: SerializeField] public Sound StackPlacedSound { get; private set; }
        [field: SerializeField] public Sound MergeSound { get; private set; }
        [field: SerializeField] public Sound HexagonDeleteSound { get; private set; }
    }
}