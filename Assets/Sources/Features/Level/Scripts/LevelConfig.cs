using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Sources.Features.Level.Scripts
{
    [CreateAssetMenu(menuName = "StaticData/LevelConfig", fileName = "LevelConfig", order = 0)]
    public class LevelConfig : ScriptableObject
    {
        private const string StackSpawnPositionsTag = "StackPosition";
        
        [field: SerializeField] public string LevelName {get; private set; }
        [field: SerializeField] public List<Vector3> StackSpawnPoints {get; private set; }
        
        [Button("Save LevelStaticData")]
        private void SaveLevelStaticData()
        {
            LevelName = SceneManager.GetActiveScene().name;
            StackSpawnPoints = GetAllTaggedObjectPositions(StackSpawnPositionsTag);
        }
        
        private List<Vector3> GetAllTaggedObjectPositions(string tag)
        {
            return GameObject.FindGameObjectsWithTag(tag)
                .Select(go => go.transform.position)
                .ToList();
        }
    }
}
