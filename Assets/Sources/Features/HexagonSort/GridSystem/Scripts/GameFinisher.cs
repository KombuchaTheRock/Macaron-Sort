using System.Linq;
using Sources.Common.CodeBase.Infrastructure.StateMachine;
using Sources.Common.CodeBase.Infrastructure.StateMachine.States;
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
        private IGameStateMachine _stateMachine;

        [Inject]
        private void Construct(IGameStateMachine stateMachine) => 
            _stateMachine = stateMachine;

        public void Initialize(MergeSystem mergeSystem)
        {
            _mergeSystem = mergeSystem;
            _gameOverScreen.gameObject.SetActive(false);
            
            SubscribeUpdates();
        }

        private void OnDestroy() => 
            CleanUp();

        private void SubscribeUpdates()
        {
            _mergeSystem.MergeFinished += OnMergeFinished;
            _gameOverScreen.ToControlPointButton.onClick.AddListener(ResetProgressToControlPoint);
        }

        private void CleanUp()
        {
            _mergeSystem.MergeFinished -= OnMergeFinished;
            _gameOverScreen.ToControlPointButton.onClick.RemoveListener(ResetProgressToControlPoint);
        }

        private void OnMergeFinished()
        {
            int occupiedCellsCount = _hexagonGrid.Cells.Where(x => x.IsOccupied).ToList().Count;

            if (occupiedCellsCount >= _hexagonGrid.Cells.Count) 
                _gameOverScreen.gameObject.SetActive(true);
        }

        private void ResetProgressToControlPoint()
        {
            _stateMachine.Enter<ResetProgressState, bool>(false);
            _gameOverScreen.gameObject.SetActive(false);
        }
    }
}