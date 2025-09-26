using UnityEngine;

namespace Sources.Common.CodeBase.Services.SoundService
{
    public interface IAudioFactory
    {
        SoundSource CreateAudioSource(Transform parent);
    }
}