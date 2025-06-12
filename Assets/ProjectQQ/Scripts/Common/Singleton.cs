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

    public class SingletonMono<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T instance = null;

        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    var objects = FindObjectsOfType(typeof(T)) as T[];
                    if (objects.Length == 1)
                    {
                        instance = objects[0];
                    }
                    else if (objects.Length == 0)
                    {
                        LogHelper.LogError($"{typeof(T).Name} ���� �Ŵ����� ����");
                    }
                    else
                    {
                        instance = null;
                        LogHelper.LogError($"{typeof(T).Name} ���� ������ �Ŵ����� ���� �� ����");
                    }
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
                        LogHelper.LogError($"___________DontDestroySIngleton_������ ������Ʈ ���� : {typeof(T).Name}");
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