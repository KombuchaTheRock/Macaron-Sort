using System;
using UnityEngine;

namespace Sources.Common.CodeBase.Services
{
    [Serializable]
    public class Sound
    {
        [field: SerializeField] public AudioClip AudioClip { get; private set; }
    }
}