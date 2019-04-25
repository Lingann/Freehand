using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace Freehand.SceneManagement
{
    public delegate void LoadingAction(int progress);

    public class SceneBrain : MonoBehaviour
    {

        public int progress { get { return _progress; } }

        private int _progress;

        public LoadingAction onProgress;

        public static void LoadSceneAsync(string name)
        {

            Scene scene = SceneManager.GetSceneByName(name);
            int index = scene.buildIndex;
        }

        private IEnumerator WaitForLoadingScene(int i)
        {
            _progress = 0;

            int toProgress = 0;

            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(i);

            asyncOperation.allowSceneActivation = false;

            while (asyncOperation.progress < 0.9f)
            {
                toProgress = (int)asyncOperation.progress * 100;
                while (_progress < toProgress)
                {
                    ++_progress;
                    if (onProgress != null) onProgress(progress);
                    yield return new WaitForEndOfFrame();
                }
            }

            toProgress = 100;

            while (_progress < toProgress)
            {
                ++_progress;
                if (onProgress != null) onProgress(progress);
                yield return new WaitForEndOfFrame();
            }
            asyncOperation.allowSceneActivation = true;
        }

    }
}