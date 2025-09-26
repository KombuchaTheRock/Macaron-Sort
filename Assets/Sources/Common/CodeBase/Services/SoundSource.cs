using System;
using System.Collections;
using UnityEngine;

namespace Sources.Common.CodeBase.Services
{
    public class SoundSource : MonoBehaviour
    {
        [SerializeField] private AudioSource _audioSource;
        public AudioClip PlayingClip { get; private set; }
        
        public void PlayOneShotWithCallback(AudioClip clip, Action onSoundPlayed)
        {
            PlayingClip = clip;
            StartCoroutine(PlayWithCallback(clip, onSoundPlayed));
        }

        private IEnumerator PlayWithCallback(AudioClip clip, Action onSoundPlayed)
        {
            _audioSource.PlayOneShot(clip);
            yield return new WaitForSeconds(clip.length);
            onSoundPlayed?.Invoke();
        }
    }
}