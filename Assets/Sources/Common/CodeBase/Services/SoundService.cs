using UnityEngine.Audio;

namespace Sources.Common.CodeBase.Services
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
            SoundSource soundSource = _soundPool.GetFreeSoundSource();
            soundSource.PlayOneShotWithCallback(sound.AudioClip, () => Stop(soundSource));
        }

        private void Stop(SoundSource soundSource) => 
            _soundPool.ReturnToPool(soundSource);
    }
}