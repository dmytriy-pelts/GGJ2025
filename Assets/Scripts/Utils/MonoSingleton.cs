using UnityEngine;

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

                _instance = Object.FindObjectOfType<T>();
                if (!_instance)
                {
                    Debug.LogError($"Singleton object not found: {typeof(T).FullName}");
                }

                return _instance;
            }
        }
    }
}