using System;
using NaughtyAttributes;
using UnityEngine;

namespace Sources.Features.HexagonSort.GridSystem.GridModificator.Scripts
{
    [Serializable]
    public class GridModifierRule
    {
        [field: SerializeField] public ModifierType Modifier { get; private set; }

        [field: SerializeField, Header("Conditions"), MinMaxSlider(0, 1f)] 
        public Vector2 FreeCellsPercentToApplyRange { get; private set; }
    
        [SerializeField, Header("Value"), MinMaxSlider(0, 1f)]
        private Vector2 _modifiedCellsPercentRange;
    
        public float MinModifiedCellsPercent => _modifiedCellsPercentRange.x;
        public float MaxModifiedCellsPercent => _modifiedCellsPercentRange.y;

        public bool IsConditionMet(float freeCellsPercent)
        {
            return freeCellsPercent >= FreeCellsPercentToApplyRange.x &&
                   freeCellsPercent <= FreeCellsPercentToApplyRange.y;
        }
    }
}