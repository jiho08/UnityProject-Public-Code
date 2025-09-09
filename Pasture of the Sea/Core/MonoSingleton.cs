using UnityEngine;

namespace Code.Core
{
    public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
    {
        private static readonly object locker = new();
        private static T instance;

        public static T Instance
        {
            get
            {
                lock (locker)
                {
                    if (instance != null)
                        return instance;
                    
                    instance = FindAnyObjectByType<T>();

                    if (instance != null)
                        return instance;
                    
                    var obj = new GameObject($"@{typeof(T).Name}");
                    instance = obj.AddComponent<T>();
                }

                return instance;
            }
        }

        protected virtual void Awake()
        {
            if (instance == null)
            {
                instance = this as T;
                DontDestroyOnLoad(gameObject);
            }
            else if (instance != this)
                Destroy(gameObject);
        }
    }
}