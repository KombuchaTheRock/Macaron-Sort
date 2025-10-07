using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Sources.Common.CodeBase.Infrastructure.Extensions;
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
            
            TweenerCore<Vector3, Vector3, VectorOptions> startAnimation = MoveAnimation(startPosition,
                initialPosition,
                0.5f,
                Ease.Unset,
                () => FinishMoveAnimation());

            startAnimation.Play();
        }

        public Tween RemoveAnimation(Action onCompleted = null, Ease ease = Ease.Unset)
        {
            Vector3 pointOutsideScreen = Camera.main.ViewportToWorldPoint(new Vector3(-0.1f, 0.6f, 0));
            Vector3 endPosition = new(pointOutsideScreen.x, transform.position.y, pointOutsideScreen.z);

            TweenerCore<Vector3, Vector3, VectorOptions> removeAnimation = MoveAnimation(transform.position,
                endPosition,
                0.5f,
                ease,
                () => onCompleted?.Invoke());
            
            removeAnimation.Play();
            
            return removeAnimation;
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
            float duration, Ease ease = Ease.Linear, Action onComplete = null)
        {
            return transform
                .DOMove(to, duration)
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