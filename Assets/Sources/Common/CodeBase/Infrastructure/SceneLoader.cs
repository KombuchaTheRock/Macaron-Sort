using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Sources.Common.CodeBase.Infrastructure
{
    public class SceneLoader
    {
        private readonly MonoBehaviour _coroutineRunner;

        public SceneLoader(MonoBehaviour coroutineRunner) => 
            _coroutineRunner = coroutineRunner;

        public void Load(string name, Action onLoaded = null) =>
            _coroutineRunner.StartCoroutine(LoadScene(name, onLoaded));
    
        private IEnumerator LoadScene(string nextScene, Action onLoaded = null)
        {
            if (SceneManager.GetActiveScene().name == nextScene)
            {
                onLoaded?.Invoke();
                yield break;
            }
      
            AsyncOperation waitNextScene = SceneManager.LoadSceneAsync(nextScene);

            while (waitNextScene.isDone == false)
                yield return null;
      
            onLoaded?.Invoke();
        }
    }
}