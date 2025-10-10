using System.Collections.Generic;
using Sources.Features.HexagonSort.BoosterSystem.Activation;
using TMPro;
using UnityEngine;
using Zenject;

namespace Sources.Features.HexagonSort.BoosterSystem.Counter
{
    public class BoosterCountView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _rocketBoosterCount;
        [SerializeField] private TextMeshProUGUI _reverseBoosterCount;
        [SerializeField] private TextMeshProUGUI _arrowBoosterCount;
    
        private IBoosterCounter _boosterCounter;

        [Inject]
        private void Construct(IBoosterCounter boosterCounter) => 
            _boosterCounter = boosterCounter;

        private void Awake()
        {
            SubscribeUpdates();
            UpdateBoostersCount(_boosterCounter.BoostersCount);
        }

        private void OnDestroy() => 
            CleanUp();

        private void OnBoosterCountChanged(Dictionary<BoosterType, int> boostersCount)
        {
            UpdateBoostersCount(boostersCount);
        }

        private void UpdateBoostersCount(Dictionary<BoosterType, int> boostersCount)
        {
            _reverseBoosterCount.text = boostersCount[BoosterType.ReverseBooster].ToString();
            _rocketBoosterCount.text = boostersCount[BoosterType.RocketBooster].ToString();
            _arrowBoosterCount.text = boostersCount[BoosterType.ArrowBooster].ToString();
        }

        private void SubscribeUpdates() => 
            _boosterCounter.BoosterCountChanged += OnBoosterCountChanged;

        private void CleanUp() => 
            _boosterCounter.BoosterCountChanged -= OnBoosterCountChanged;
    }
}
