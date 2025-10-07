using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace Sources.Features.HexagonSort.HexagonStackSystem.StackMover.Scripts
{
    public class StackMovement : MonoBehaviour
    {
        private TweenerCore<Vector3, Vector3, VectorOptions> _transitionToInitialAnim;

        public bool CanMove { get; private set; } = true;

        public void EnableMovement() =>
            CanMove = true;

        public void DisableMovement() =>
            CanMove = false;

        public void StartAnimation(Vector3 initialPosition)
        {
            if (CanMove == false)
                return;

            Vector3 pointOutsideScreen = Camera.main.ViewportToWorldPoint(new Vector3(1.1f, 0.5f, 0));
            Vector3 startPosition = new(pointOutsideScreen.x, transform.position.y, pointOutsideScreen.z);

            
            transform.DOMove(initialPosition, 0.5f)
                .From(startPosition)
                .Play()
                .OnComplete(() => FinishMoveAnimation())
                .SetLink(gameObject);
        }

        public void FollowingTarget(Vector3 target, float speed)
        {
            if (CanMove == false)
                return;

            transform.position =
                Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
        }

        public void MoveToTarget(Vector3 targetPosition, float speed, Action onComplete = null)
        {
            if (CanMove == false)
                return;

            float transitionDuration = (transform.position - targetPosition).magnitude / speed;
            _transitionToInitialAnim?.Complete();
            
            DisableMovement();
            
            _transitionToInitialAnim = MoveAnimation(transform.position, targetPosition, transitionDuration, Ease.OutQuad,
                () => FinishMoveAnimation(onComplete));
            _transitionToInitialAnim.Play();
        }

        private TweenerCore<Vector3, Vector3, VectorOptions> MoveAnimation(Vector3 from, Vector3 to,
            float transitionDuration, Ease ease = Ease.Linear, Action onComplete = null)
        {
            return transform
                .DOMove(to, transitionDuration)
                .From(from)
                .SetEase(ease)
                .OnComplete(() => onComplete?.Invoke())
                .SetLink(gameObject);
        }

        private void FinishMoveAnimation(Action onComplete = null)
        {
            EnableMovement();
            onComplete?.Invoke();
        }
    }
}