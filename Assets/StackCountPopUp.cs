using DG.Tweening;
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
        transform.DOScale(0f, _animationDuration)
            .SetDelay(_lifeTime)
            .SetLink(gameObject)
            .OnComplete(() => Destroy(gameObject, _lifeTime))
            .SetEase(_animationEase)
            .Play();
    }
}
