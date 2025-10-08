using System;
using Sources.Common.CodeBase.Services.PlayerProgress.Data;
using Sources.Features.HexagonSort.GridSystem.Scripts;
using UnityEngine;
using UnityEngine.UI;

public class BoosterPicker : MonoBehaviour
{
    public event Action<BoosterType> BoosterPicked;

    [SerializeField] private Button _rocketBoosterActivator;
    [SerializeField] private Button _arrowBoosterActivator;
    [SerializeField] private Button _reverseBoosterActivator;

    private HexagonGridSaveLoader _gridSaveLoader;
    private StacksData _stacksData;

    public void Initialize(HexagonGrid hexagonGrid)
    {
        _gridSaveLoader = hexagonGrid.GetComponent<HexagonGridSaveLoader>();

        SubscribeUpdates();
    }

    private void SubscribeUpdates()
    {
        _gridSaveLoader.GridDataUpdated += OnGridDataUpdated;
        SubscribeButtons();
    }

    private void OnGridDataUpdated(int stacksOnGridCount)
    {
        if (stacksOnGridCount <= 0)
        {
            _rocketBoosterActivator.interactable = false;
            _arrowBoosterActivator.interactable = false;
        }
        else
        {
            _rocketBoosterActivator.interactable = true;
            _arrowBoosterActivator.interactable = true;
        }
    }

    private void SubscribeButtons()
    {
        _reverseBoosterActivator.onClick.AddListener(() => SwitchBoosterTo(BoosterType.ReverseBooster));
        _rocketBoosterActivator.onClick.AddListener(() => SwitchBoosterTo(BoosterType.RocketBooster));
        _arrowBoosterActivator.onClick.AddListener(() => SwitchBoosterTo(BoosterType.ArrowBooster));
    }

    public void EnableInteractable()
    {
        _rocketBoosterActivator.interactable = true;
        _arrowBoosterActivator.interactable = true;
        _reverseBoosterActivator.interactable = true;
    }

    public void DisableInteractable()
    {
        _rocketBoosterActivator.interactable = false;
        _arrowBoosterActivator.interactable = false;
        _reverseBoosterActivator.interactable = false;
    }

    private void SwitchBoosterTo(BoosterType boosterType)
    {
        switch (boosterType)
        {
            case BoosterType.RocketBooster:
                BoosterPicked?.Invoke(BoosterType.RocketBooster);
                break;
            case BoosterType.ArrowBooster:
                BoosterPicked?.Invoke(BoosterType.ArrowBooster);
                break;
            case BoosterType.ReverseBooster:
                BoosterPicked?.Invoke(BoosterType.ReverseBooster);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(boosterType), boosterType, null);
        }
    }
}