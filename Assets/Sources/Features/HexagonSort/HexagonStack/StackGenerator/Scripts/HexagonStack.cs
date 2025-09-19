using System;
using System.Collections.Generic;
using Sources.Features.HexagonSort.HexagonStack.HexagonTile.Scripts;
using Sources.Features.HexagonSort.HexagonStack.StackMover.Scripts;
using UnityEngine;

namespace Sources.Features.HexagonSort.HexagonStack.StackGenerator.Scripts
{
    public class HexagonStack : MonoBehaviour
    {
        [SerializeField] private StackMovement _movement;

        private List<Hexagon> _hexagons = new();
        
        public Hexagon FirstHexagon => _hexagons[^1];
        public Vector3 InitialPosition { get; private set; }
        public bool CanMove => _movement.CanMove;

        private void Awake() =>
            _movement.StartAnimation(transform.position);

        public void SetInitialPosition(Vector3 position) =>
            InitialPosition = position;

        public void DisableMovement() =>
            _movement.DisableMovement();

        public void EnableMovement() =>
            _movement.EnableMovement();

        public void FollowingTarget(Vector3 target, float speed) =>
            _movement.FollowingTarget(target, speed);

        public void MoveToTarget(Vector3 targetPosition, float speed, Action onComplete = null) =>
            _movement.MoveToTarget(targetPosition, speed, onComplete);

        public void Add(Hexagon hexagon) =>
            _hexagons.Add(hexagon);
    }
}