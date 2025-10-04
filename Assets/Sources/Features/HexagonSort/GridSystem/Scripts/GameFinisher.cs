using System;
using System.Linq;
using Sources.Common.CodeBase.Infrastructure.StateMachine;
using Sources.Common.CodeBase.Infrastructure.StateMachine.States;
using Sources.Features.HexagonSort.Merge.Scripts;
using Sources.Features.UI.Scripts;
using UnityEngine;
using Zenject;

namespace Sources.Features.HexagonSort.GridSystem.Scripts
{
    public class GameFinisher : MonoBehaviour
    {
        [SerializeField] private HexagonGrid _hexagonGrid;
        [SerializeField] private GameOverScreen _gameOverScreen;
        
        private IStackMerger _stackMerger;
        private IGameStateMachine _stateMachine;

        [Inject]
        private void Construct(IGameStateMachine stateMachine, IStackMerger stackMerger)
        {
            _stateMachine = stateMachine;
            _stackMerger = stackMerger;
        }

        private void Awake()
        {
            _gameOverScreen.gameObject.SetActive(false);
            SubscribeUpdates();
        }

        private void OnDestroy() => 
            CleanUp();

        private void SubscribeUpdates()
        {
            _stackMerger.MergeFinished += OnMergeFinished;
            _gameOverScreen.ToControlPointButton.onClick.AddListener(ResetProgressToControlPoint);
        }

        private void CleanUp()
        {
            _stackMerger.MergeFinished -= OnMergeFinished;
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