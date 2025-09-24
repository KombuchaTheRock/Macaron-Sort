using System.Runtime.Serialization;

namespace Sources.Common.CodeBase.Services.PlayerProgress
{
    public class GameProgress
    {
        public PersistentProgressData PersistentProgressData { get; }
        public ControlPointProgressData ControlPointProgressData { get; }
        
        public GameProgress(PersistentProgressData persistentProgressData, ControlPointProgressData controlPointProgressData)
        {
            PersistentProgressData = persistentProgressData;
            ControlPointProgressData = controlPointProgressData;
        }
    }
}