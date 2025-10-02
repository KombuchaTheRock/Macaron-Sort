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
            
            _audioSource.PlayOneShot(clip);
            _coroutine = StartCoroutine(PlayWithCallbackRoutine(clip.length, onSoundPlayed));
        }

        public void StopCallbackRoutine()
        {
            if (_coroutine != null) 
                StopCoroutine(_coroutine);
        }
        
        private IEnumerator PlayWithCallbackRoutine(float clipLength, Action onSoundPlayed)
        {
            yield return new WaitForSeconds(clipLength);
            
            onSoundPlayed?.Invoke();
        }
    }
}