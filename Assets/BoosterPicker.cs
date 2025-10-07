using System;
using UnityEngine;
using UnityEngine.UI;

public class BoosterPicker : MonoBehaviour
{
    public event Action<BoosterType> BoosterPicked;
    
    [SerializeField] private Button _rocketBoosterActivator; 
    [SerializeField] private Button _arrowBoosterActivator; 
    [SerializeField] private Button _reverseBoosterActivator;

    private void Awake()
    {
        _reverseBoosterActivator.onClick.AddListener(() => SwitchBoosterTo(BoosterType.ReverseBooster));
        _rocketBoosterActivator.onClick.AddListener(() => SwitchBoosterTo(BoosterType.RocketBooster));
        _arrowBoosterActivator.onClick.AddListener(() => SwitchBoosterTo(BoosterType.ArrowBooster));
    }

    private void SwitchBoosterTo(BoosterType boosterType)
    {
        switch (boosterType)
        {
            case BoosterType.RocketBooster:
                BoosterPicked?.Invoke(BoosterType.RocketBooster);
                Debug.Log("RocketBooster");
                break;
            case BoosterType.ArrowBooster:
                BoosterPicked?.Invoke(BoosterType.ArrowBooster);
                Debug.Log("ArrowBooster");
                break;
            case BoosterType.ReverseBooster:
                BoosterPicked?.Invoke(BoosterType.ReverseBooster);
                Debug.Log("ReverseBooster");
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(boosterType), boosterType, null);
        }
    }
}