using Sources.Common.CodeBase.Paths;
using Sources.Common.CodeBase.Services.ResourceLoader;
using UnityEngine;
using UnityEngine.Audio;

namespace Sources.Common.CodeBase.Services.SoundService
{
    public class SoundService : ISoundService
    {
        private const string MasterMixerVolume = "MasterVolume";
        private AudioMixer _mixer;

        private readonly SoundPool _soundPool;

        public SoundService(SoundPool soundPool, IResourceLoader resourceLoader)
        {
            _soundPool = soundPool;
            _mixer = resourceLoader.LoadAsset<AudioMixer>(AssetsPaths.MasterMixer);
        }

        public void Mute()
        {
            Debug.Log("Mute");
            _mixer.SetFloat(MasterMixerVolume, -80f);
        }

        public void UnMute()
        {
            Debug.Log("UnMute");
            _mixer.SetFloat(MasterMixerVolume, 0f);
        }

        public void Play(Sound sound)
        {
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