using UnityEngine;

namespace QQ
{
    public class Singleton<T> where T : class, new()
    {
        private static T instance = null;
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new T();
                }

                return instance;
            }
        }

        protected Singleton()
        {
        }

        ~Singleton()
        {
            instance = null;
        }

        public static bool isValid() => instance != null;        
    }

    public class DontDestroySingleton<T> : MonoBehaviour where T : DontDestroySingleton<T>
    {
        private static T instance = null;
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    var go = GameObject.Find(typeof(T).Name);
                    if (go != null)
                    {
                        instance = go.GetComponent<T>();
                    }
                    else
                    {
#if UNITY_EDITOR
                        LogHelper.LogError($"Creat DonDestroySingleTone : {typeof(T)}");
#endif
                        var name = typeof(T).Name;
                        instance = new GameObject(string.Concat("SingletonOf", typeof(T).Name), typeof(T)).GetComponent<T>();
                    }
                }

                return instance;
            }
        }

        public static bool IsValid() => instance != null;

        protected virtual void Awake()
        {
            if (instance != null)
                DestroyImmediate(instance.gameObject);

            instance = this as T;
            DontDestroyOnLoad(instance.gameObject);
        }

        protected virtual void OnDestroy()
        {
            if (IsValid())
            {
                instance = null;
            }
        }
    }
}