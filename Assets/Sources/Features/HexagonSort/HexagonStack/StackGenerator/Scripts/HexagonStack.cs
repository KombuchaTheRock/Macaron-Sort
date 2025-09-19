using System;
using System.Collections.Generic;
using DG.Tweening;
using Sources.Features.HexagonSort.HexagonStack.HexagonTile.Scripts;
using Sources.Features.HexagonSort.HexagonStack.StackMover.Scripts;
using UnityEngine;

namespace Sources.Features.HexagonSort.HexagonStack.StackGenerator.Scripts
{
    public class HexagonStack : MonoBehaviour
    {
        [SerializeField] private StackMovement _movement;

        private List<Hexagon> _hexagons = new();
        private Vector3 _initialPosition;
        public Hexagon FirstHexagon => _hexagons[^1];
        public bool CanMove => _movement.CanMove;

        private void Awake()
        {
            _initialPosition = transform.position;
            Vector3 pointOutsideScreen = Camera.main.ViewportToWorldPoint(new Vector3(1.1f, 0.5f, 0));
            Vector3 startPosition = new(pointOutsideScreen.x, transform.position.y, pointOutsideScreen.z);
            
            transform.DOMove(_initialPosition, 0.5f).From(startPosition).Play();
        }

        public void DisableMovement() =>
            _movement.DisableMovement();

        public void FollowingTarget(Vector3 target, float speed) =>
            _movement.FollowingTarget(target, speed);

        public void MoveToTarget(Vector3 targetPosition, float speed, Action onComplete = null) =>
            _movement.MoveToTarget(targetPosition, speed, onComplete);

        public void Add(Hexagon hexagon)
        {
           // hexagon.transform.DOScale(1f, 0.3f).From(0).SetEase(Ease.OutBounce).Play();
            _hexagons.Add(hexagon);
        }
    }
}