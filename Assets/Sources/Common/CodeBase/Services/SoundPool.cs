using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace Sources.Common.CodeBase.Services
{
    public class SoundPool : MonoBehaviour
    {
        [SerializeField] private Transform _audioSourceParent;

        private List<SoundSource> _freeAudioSources;
        private List<SoundSource> _occupiedAudioSources;
        private IAudioFactory _audioFactory;

        [Inject]
        private void Construct(IAudioFactory audioFactory)
        {
            _audioFactory = audioFactory;
        }
        
        public SoundSource GetFreeSoundSource()
        {
            if (_freeAudioSources.Count > 0)
            {
                SoundSource audioSource = _freeAudioSources.First();

                _occupiedAudioSources.Add(audioSource);
                _freeAudioSources.Remove(audioSource);

                audioSource.gameObject.SetActive(true);
                
                return audioSource;
            }
            else
            {
                SoundSource audioSource = _audioFactory.CreateAudioSource(_audioSourceParent);

                _occupiedAudioSources.Add(audioSource);

                return audioSource;
            }
        }

        public void ReturnToPool(SoundSource source)
        {
            if (_occupiedAudioSources.Contains(source))
                _occupiedAudioSources.Remove(source);

            source.gameObject.SetActive(false);
            
            _freeAudioSources.Add(source);
        }
    }
}