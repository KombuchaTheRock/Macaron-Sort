using System.Linq;
using Cysharp.Threading.Tasks;
using Sources.Common.CodeBase.Infrastructure.StateMachine;
using Sources.Common.CodeBase.Services.PlayerProgress;
using Sources.Features.HexagonSort.Merge.Scripts;
using UnityEngine;
using Zenject;

namespace Sources.Features.HexagonSort.GridSystem.Scripts
{
    public class GameFinisher : MonoBehaviour
    {
        [SerializeField] private HexagonGrid _hexagonGrid;
        [SerializeField] private GameOverScreen _gameOverScreen;
        
        private MergeSystem _mergeSystem;
        private IGameProgressService _progressService;
        private IGameStateMachine _stateMachine;

        [Inject]
        private void Construct(IGameProgressService progressService, IGameStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
            _progressService = progressService;
        }

        public void Initialize(MergeSystem mergeSystem)
        {
            _mergeSystem = mergeSystem;
            _mergeSystem.MergeFinished += OnMergeFinished;
            
            _gameOverScreen.ToControlPointButton.onClick.AddListener(ResetProgressToControlPoint);
        }

        private void OnDestroy() => 
            _mergeSystem.MergeFinished -= OnMergeFinished;

        private void OnMergeFinished()
        {
            int occupiedCellsCount = _hexagonGrid.Cells.Where(x => x.IsOccupied).ToList().Count;

            if (occupiedCellsCount >= _hexagonGrid.Cells.Count)
            {
                _gameOverScreen.gameObject.SetActive(true);
            }
        }

        private void ResetProgressToControlPoint()
        {
            

            _stateMachine.Enter<ResetState>();
        }
    }
}