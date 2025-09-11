using Sources.Features.HexagonSort.StackGenerator.Scripts;
using UnityEngine;

namespace Sources.Features.HexagonSort.GridGenerator.Scripts
{
    public class GridCell : MonoBehaviour
    {
        private HexagonStack _stack;
        
        public bool IsOccupied 
        { get => _stack != null;
            private set { }
        }
    }
}
