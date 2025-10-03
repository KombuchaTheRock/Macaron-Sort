using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

public class StackCountPopUp : MonoBehaviour
{
    [SerializeField] private float _lifeTime;
    [SerializeField] private float _animationDuration;
    [SerializeField] private Ease _animationEase;

    private void Awake()
    {
        AppearAnimation();
        DisappearAnimation();
    }

    private void Update() =>
        RotateText();

    private void RotateText()
    {
        if (transform.rotation.y != 0)
            transform.rotation = Quaternion.Euler(90, 0, 0);
    }

    private void AppearAnimation()
    {
        transform.DOScale(1, _animationDuration)
            .From(0)
            .SetLink(gameObject)
            .SetEase(_animationEase)
            .Play();
    }
    
    private void DisappearAnimation()
    {
        Sequence sequence = DOTween.Sequence();

        TweenerCore<Vector3, Vector3, VectorOptions> scaleAnimation = transform.DOScale(0f, _animationDuration)
            .SetDelay(_lifeTime)
            .SetLink(gameObject)
            .SetEase(_animationEase);

        float yOffset = 2f;
        Vector3 targetPosition = transform.position + Vector3.up * yOffset;
        TweenerCore<Vector3, Vector3, VectorOptions> moveAnimation = transform.DOMove(targetPosition, _animationDuration)
            .SetEase(_animationEase);

        sequence.Append(moveAnimation)
            .Join(scaleAnimation)
            .OnComplete(() => Destroy(gameObject, _lifeTime))
            .Play();
    }
}
