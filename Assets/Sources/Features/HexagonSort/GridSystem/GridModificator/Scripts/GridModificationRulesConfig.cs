using System.Collections.Generic;
using UnityEngine;

namespace Sources.Features.HexagonSort.GridSystem.GridModificator.Scripts
{
    [CreateAssetMenu(menuName = "StaticData/GridModificationRulesConfig", fileName = "GridModificationRulesConfig",
        order = 0)]
    public class GridModificationRulesConfig : ScriptableObject
    {
        [SerializeField]
        private List<GridModifierRule> _modificationRules = new();

        public List<GridModifierRule> ModificationRules => _modificationRules; 
    }
}