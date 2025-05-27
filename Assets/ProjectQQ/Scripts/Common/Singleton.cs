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

        public bool isValid()
        {
            return instance != null;
        }
    }

    public class SingletonMono<T> : MonoBehaviour where T : class, new()
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

        protected SingletonMono()
        {
        }

        ~SingletonMono()
        {
            instance = null;
        }

        public bool IsValid()
        {
            return instance != null;
        }
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
                        Debug.LogError($"___________DontDestroySIngleton_생성된 오브젝트 없음 : {typeof(T).Name}");
                    }
                }

                return instance;
            }
        }

        public bool IsValid()
        {
            return instance != null;
        }

        protected virtual void Awake()
        {
            if (instance != null)
                DestroyImmediate(instance.gameObject);

            instance = this as T;
            DontDestroyOnLoad(instance.gameObject);
        }
    }
}