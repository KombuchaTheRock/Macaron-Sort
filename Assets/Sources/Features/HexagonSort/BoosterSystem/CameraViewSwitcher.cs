using DG.Tweening;
using UnityEngine;

namespace Sources.Features.HexagonSort.BoosterSystem
{
    public class CameraViewSwitcher : MonoBehaviour
    {
        [SerializeField] private float _boosterViewTransitionDuration = 0.5f;
        [SerializeField] private float _defaultViewTransitionDuration = 0.5f;
        [SerializeField] private Ease _boosterViewEase;
        [SerializeField] private Ease _defaultViewEase;
    
        [SerializeField] private Transform _defaultCameraTransform;
        [SerializeField] private Transform _boosterCameraTransform;

        public void ToDefaultTransform()
        {
            Sequence transitionAnim = TransitionAnim(_defaultCameraTransform, _defaultViewTransitionDuration, _defaultViewEase);
            transitionAnim.Play();
        }

        public void ToBoosterTransform()
        {
            Sequence transitionAnim = TransitionAnim(_boosterCameraTransform, _boosterViewTransitionDuration, _boosterViewEase);
            transitionAnim.Play();
        }

        private Sequence TransitionAnim(Transform to, float duration, Ease ease)
        {
            Sequence sequence = DOTween.Sequence();
            sequence.SetLink(gameObject);

            sequence.Join(transform.DOMove(to.transform.position, duration).SetEase(ease))
                .Join(transform.DORotate(to.transform.eulerAngles, duration).SetEase(ease));
        
            return sequence;
        }
    }
}
