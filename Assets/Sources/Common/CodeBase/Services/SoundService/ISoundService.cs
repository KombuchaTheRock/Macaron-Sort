namespace Sources.Common.CodeBase.Services.SoundService
{
    public interface ISoundService
    {
        void Mute();
        void UnMute();

        void Play(Sound sound);
    }
}