using Sources.Common.CodeBase.Services.PlayerProgress.Data;

namespace Sources.Common.CodeBase.Services.PlayerProgress
{
    public interface ISaveDataFactory
    {
        PersistentProgressData CreatePersistentProgressData();
        ControlPointProgressData CreateControlPointProgressData();
    }
}