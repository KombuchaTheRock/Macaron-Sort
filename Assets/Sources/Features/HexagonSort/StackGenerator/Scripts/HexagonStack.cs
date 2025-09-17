using System;
using System.Collections.Generic;
using Sources.Features.HexagonSort.HexagonTile.Scripts;
using Sources.Features.HexagonSort.StackMover.Scripts;
using UnityEngine;

namespace Sources.Features.HexagonSort.StackGenerator.Scripts
{
    public class HexagonStack : MonoBehaviour
    {
        [SerializeField] private StackMovement _movement;

        private List<Hexagon> _hexagons = new();
        public Hexagon FirstHexagon => _hexagons[^1];
        public bool CanMove => _movement.CanMove;
        
        public void DisableMovement() =>
            _movement.DisableMovement();

        public void FollowingTarget(Vector3 target, float speed) =>
            _movement.FollowingTarget(target, speed);

        public void MoveToTarget(Vector3 targetPosition, float speed, Action onComplete = null) =>
            _movement.MoveToTarget(targetPosition, speed, onComplete);

        public void Add(Hexagon hexagon) =>
            _hexagons.Add(hexagon);
    }
}