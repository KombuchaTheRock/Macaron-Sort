using System;
using System.Collections;
using Sources.Common.CodeBase.Services.WindowService;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class GameOverWindow : MonoBehaviour
{
    [SerializeField] private Image[] _animatedImages;
    [SerializeField] private TextMeshProUGUI[] _animatedTexts;
    [SerializeField] private WindowAnimation _windowAnimation;
    private PostProcessVolume _postProcessVolume;

    private void Awake()
    {
        _postProcessVolume = Camera.main.gameObject.GetComponent<PostProcessVolume>();
    }

    private void OnEnable()
    {
        _postProcessVolume.enabled = true;

        StartCoroutine(BlurRoutine(0, 1, 0.01f));
        
        Debug.Log("OnEnable");
        foreach (Image image in _animatedImages)
            _windowAnimation.ImageFadeAnimation(image, 0, image.color.a);

        foreach (TextMeshProUGUI text in _animatedTexts)
            _windowAnimation.TextFadeAnimation(text, 0, text.color.a);
    }
    
    private IEnumerator BlurRoutine(float from, float to, float timeBetweenSteps, Action onCompleted = null)
    {
        _postProcessVolume.weight = from;
            
        int sign = from > to ? -1 : 1;
            
        while (from <= to)
        {
            from += 0.001f * sign;
            _postProcessVolume.weight = from;
            yield return new WaitForSecondsRealtime(timeBetweenSteps);
        }
            
        onCompleted?.Invoke();
    }

    private void OnDisable() => 
        _postProcessVolume.enabled = false;
}