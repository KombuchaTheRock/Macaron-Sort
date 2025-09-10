using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Sources.Features.Level.Scripts
{
    [CreateAssetMenu(menuName = "Level/LevelStaticData", fileName = "LevelStaticData", order = 0)]
    public class LevelConfig : ScriptableObject
    {
        private const string StackSpawnPositionsTag = "StackPosition";
        
        public string LevelName;
        public List<Vector3> StackSpawnPoints;
        
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
