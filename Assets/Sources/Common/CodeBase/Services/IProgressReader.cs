using Sources.Common.CodeBase.Services.PlayerProgress;

namespace Sources.Common.CodeBase.Services
{
    public interface IProgressReader
    {
        void ApplyProgress(GameProgress progress);
    }
}