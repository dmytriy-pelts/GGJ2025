using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.SceneManagement;

namespace GumFly.Utils
{
    public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance)
                {
                    return _instance;
                }

                for (int i = 0; i < SceneManager.sceneCount; i++)
                {
                    var goList = ListPool<GameObject>.Get();
                    try
                    {
                        var scene = SceneManager.GetSceneAt(i);
                        if (scene.isLoaded)
                        {
                            scene.GetRootGameObjects(goList);
                            foreach (var go in goList)
                            {
                                _instance = go.GetComponentInChildren<T>();
                                if (_instance)
                                {
                                    return _instance;
                                }
                            }
                        }
                    }
                    finally
                    {
                        ListPool<GameObject>.Release(goList);
                    }
                }
                
                Debug.LogError($"Singleton object not found: {typeof(T).FullName}");
                return null;
            }
        }
    }
}