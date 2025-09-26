namespace Sources.Common.CodeBase.Services
{
    public interface ISoundService
    {
        void Mute();
        void UnMute();

        void Play(Sound sound);
    }
}