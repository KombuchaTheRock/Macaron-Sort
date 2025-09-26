using UnityEngine.Audio;

namespace Sources.Common.CodeBase.Services.SoundService
{
    public class SoundService : ISoundService
    {
        private const string MasterMixerName = "Master";
        private AudioMixer _mixer;

        private readonly SoundPool _soundPool;

        public SoundService(SoundPool soundPool) =>
            _soundPool = soundPool;

        public void Mute() =>
            _mixer.SetFloat(MasterMixerName, 0);

        public void UnMute() =>
            _mixer.SetFloat(MasterMixerName, 80f);

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