using System;
using System.Collections.Generic;
using System.Linq;
using Sources.Common.CodeBase.Services.PlayerProgress;
using Sources.Common.CodeBase.Services.PlayerProgress.Data;
using Sources.Features.HexagonSort.BoosterSystem.Counter;
using Sources.Features.HexagonSort.GridSystem.Scripts;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Sources.Features.HexagonSort.BoosterSystem.Activation
{
    public class BoosterPicker : MonoBehaviour
    {
        public event Action<BoosterType> BoosterPicked;

        [SerializeField] private Button _rocketBoosterActivator;
        [SerializeField] private Button _arrowBoosterActivator;
        [SerializeField] private Button _reverseBoosterActivator;

        private Dictionary<BoosterType, Button> _boosterButtons;
        private Dictionary<BoosterType, bool> _buttonsInteractable = new();
        
        private HexagonStacksSaveLoader _stacksSaveLoader;
        private StacksData _stacksData;
        private IGameProgressService _progressService;
        private IBoosterCounter _boosterCounter;
        private Dictionary<BoosterType, int> _boosterCounts;

        [Inject]
        private void Construct(IGameProgressService progressService, IBoosterCounter boosterCounter)
        {
            _boosterCounter = boosterCounter;
            _progressService = progressService;
        }
    
        public void Initialize(HexagonGrid hexagonGrid)
        {
            _stacksSaveLoader = hexagonGrid.GetComponent<HexagonStacksSaveLoader>();

            _buttonsInteractable = new Dictionary<BoosterType, bool>
            {
                [BoosterType.ArrowBooster] = _arrowBoosterActivator.interactable,
                [BoosterType.RocketBooster] = _rocketBoosterActivator.interactable,
                [BoosterType.ReverseBooster] = _reverseBoosterActivator.interactable,
            };
            
            _boosterButtons = new Dictionary<BoosterType, Button>
            {
                [BoosterType.ArrowBooster] = _arrowBoosterActivator,
                [BoosterType.RocketBooster] = _rocketBoosterActivator,
                [BoosterType.ReverseBooster] = _reverseBoosterActivator,
            };
            
            _boosterCounts = _boosterCounter.BoostersCount;
            
            UpdateButtonEnabled();
            
            foreach (KeyValuePair<BoosterType, int> boosterCount in _boosterCounts) 
                _boosterButtons[boosterCount.Key].interactable = boosterCount.Value > 0;
            
            SubscribeUpdates();
        }

        private void SubscribeUpdates()
        {
            _boosterCounter.BoosterCountChanged += OnBoosterCountChanged;
            _stacksSaveLoader.GridDataUpdated += UpdateButtonEnabled;
            SubscribeButtons();
        }

        private void OnBoosterCountChanged(Dictionary<BoosterType, int> boosterCounts)
        {
            foreach (KeyValuePair<BoosterType, int> boosterCount in _boosterCounts) 
                _boosterButtons[boosterCount.Key].interactable = boosterCount.Value > 0;
        }

        private void UpdateButtonEnabled()
        {
            if (ZeroStacksOnGrid)
            {
                _rocketBoosterActivator.interactable = false;
                _arrowBoosterActivator.interactable = false;
            }
            else
            {
                _rocketBoosterActivator.interactable = _boosterCounts[BoosterType.RocketBooster] > 0;
                _arrowBoosterActivator.interactable = _boosterCounts[BoosterType.ArrowBooster] > 0;
            }
        }

        private bool ZeroStacksOnGrid => _progressService.GameProgress.PersistentProgressData.WorldData.StacksData.StacksOnGrid.Count <= 0;

        public void DisableButtons()
        {
            SaveButtonsInteractable();
            
            foreach (Button button in _boosterButtons.Values) 
                button.interactable = false;
        }

        public void LoadButtonsInteractable()
        {
            foreach (KeyValuePair<BoosterType, Button> button in _boosterButtons) 
                button.Value.interactable = _buttonsInteractable[button.Key];

            foreach (KeyValuePair<BoosterType, int> boosterCount in _boosterCounts) 
                _boosterButtons[boosterCount.Key].interactable = boosterCount.Value > 0;
            
            UpdateButtonEnabled();
        }
        
        private void SaveButtonsInteractable()
        {
            foreach (KeyValuePair<BoosterType, Button> boosterButton in _boosterButtons) 
                _buttonsInteractable[boosterButton.Key] = boosterButton.Value.interactable;
        }
        
        private void SubscribeButtons()
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
}