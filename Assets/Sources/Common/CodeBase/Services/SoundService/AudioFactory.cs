using Sources.Common.CodeBase.Paths;
using UnityEngine;
using Zenject;

namespace Sources.Common.CodeBase.Services.SoundService
{
    public class AudioFactory : BaseFactory, IAudioFactory
    {
        public AudioFactory(IInstantiator instantiator, IResourceLoader resourceLoader) : base(instantiator,
            resourceLoader)
        {
        }

        public SoundSource CreateAudioSource(Transform parent) => 
            Instantiate<SoundSource>(AssetsPaths.SoundSource, parent.position, parent);
    }
}