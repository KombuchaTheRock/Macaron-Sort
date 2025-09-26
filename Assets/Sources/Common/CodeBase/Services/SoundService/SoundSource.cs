using System;
using System.Collections;
using UnityEngine;

namespace Sources.Common.CodeBase.Services.SoundService
{
    public class SoundSource : MonoBehaviour
    {
        [SerializeField] private AudioSource _audioSource;
        private Coroutine _coroutine;
        public AudioClip PlayingClip { get; private set; }
        
        public void PlayWithCallback(AudioClip clip, Action onSoundPlayed)
        {
            PlayingClip = clip;
            _coroutine = StartCoroutine(PlayWithCallbackRoutine(clip, onSoundPlayed));
        }

        public void Stop()
        {
            if (_coroutine != null) 
                StopCoroutine(_coroutine);
            
            _audioSource.Stop();
        }
        
        private IEnumerator PlayWithCallbackRoutine(AudioClip clip, Action onSoundPlayed)
        {
            _audioSource.PlayOneShot(clip);
            yield return new WaitForSeconds(clip.length);
            
            onSoundPlayed?.Invoke();
        }
    }
}