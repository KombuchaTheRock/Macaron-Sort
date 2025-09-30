using Sources.Common.CodeBase.Paths;
using Sources.Common.CodeBase.Services.ResourceLoader;
using Sources.Common.CodeBase.Services.WindowService;
using UnityEngine;
using UnityEngine.Audio;

namespace Sources.Common.CodeBase.Services.SoundService
{
    public class SoundService : ISoundService
    {
        private const string MasterMixerVolume = "MasterVolume";
        private AudioMixer _mixer;

        private readonly SoundPool _soundPool;
        private readonly IPauseService _pauseService;

        public SoundService(SoundPool soundPool, IResourceLoader resourceLoader, IPauseService pauseService)
        {
            _soundPool = soundPool;
            _pauseService = pauseService;
            _mixer = resourceLoader.LoadAsset<AudioMixer>(AssetsPaths.MasterMixer);
        }

        public void Mute() => 
            _mixer.SetFloat(MasterMixerVolume, -80f);

        public void UnMute() => 
            _mixer.SetFloat(MasterMixerVolume, 0f);

        public void Play(Sound sound)
        {
            if (_pauseService.IsPaused)
                return;
            
            SoundSource soundSource = sound.CanBeReplaceableBySame
                ? TryGetSoundSourceBySound(sound)
                : _soundPool.GetFreeSoundSource();

            soundSource.PlayWithCallback(sound.AudioClip, () => Stop(soundSource));
        }

        private SoundSource TryGetSoundSourceBySound(Sound sound)
        {
            SoundSource soundSource;
            
            if (_soundPool.TryGetSoundSourceBySound(sound, out SoundSource foundedSource))
            {
                foundedSource.Stop();
                soundSource = foundedSource;
            }
            else
            {
                soundSource = _soundPool.GetFreeSoundSource();
            }

            return soundSource;
        }

        private void Stop(SoundSource soundSource) =>
            _soundPool.ReturnToPool(soundSource);
    }
}