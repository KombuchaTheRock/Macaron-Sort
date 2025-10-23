using Sources.Common.CodeBase.Services.PlayerProgress;
using Sources.Common.CodeBase.Services.PlayerProgress.Data;
using UnityEngine;
using Zenject;

namespace Sources.Features.HexagonSort.GridSystem.Scripts
{
    public class HexagonGridSaver : MonoBehaviour
    {
        [SerializeField] private HexagonGrid _hexagonGrid;
        [SerializeField] private GridModificator _gridModificator;
        private IGameProgressService _progressService;
        private IPlayerLevel _playerLevel;

        private GridData PersistentGridData => _progressService.GameProgress.PersistentProgressData.WorldData.GridData;
        private GridData ControlPointGridData => _progressService.GameProgress.ControlPointProgressData.WorldData.GridData;
        
        [Inject]
        private void Construct(IGameProgressService progressService, IPlayerLevel playerLevel)
        {
            _playerLevel = playerLevel;
            _progressService = progressService;
        }

        private void Awake() => 
            SubscribeUpdates();

        private void OnDestroy() => 
            CleanUp();

        private void OnGridModified()
        {
            Debug.Log("OnGridModified");
            PersistentGridData.UpdateGridData(_hexagonGrid);
        }

        private void OnControlPointAchieved() => 
            ControlPointGridData.UpdateGridData(_hexagonGrid);

        private void SubscribeUpdates()
        {
            _playerLevel.ControlPointAchieved += OnControlPointAchieved;
            _gridModificator.GridModified += OnGridModified;
        }

        private void CleanUp()
        {
            _gridModificator.GridModified -= OnGridModified;
            _gridModificator.GridModified -= OnGridModified;
        }
    }
}