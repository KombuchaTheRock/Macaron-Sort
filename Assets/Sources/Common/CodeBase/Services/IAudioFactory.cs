using UnityEngine;

namespace Sources.Common.CodeBase.Services
{
    public interface IAudioFactory
    {
        SoundSource CreateAudioSource(Transform parent);
    }
}