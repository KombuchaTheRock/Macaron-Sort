using System;
using UnityEngine;

namespace Sources.Common.CodeBase.Services.SoundService
{
    [Serializable]
    public class Sound
    {
        [field: SerializeField] public AudioClip AudioClip { get; private set; }
        [field: SerializeField] public bool CanBeReplaceableBySame { get; private set; }
    }
}