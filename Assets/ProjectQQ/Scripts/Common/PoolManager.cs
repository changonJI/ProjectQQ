using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace QQ
{
    using ObjectPool = Dictionary<string, Queue<BaseGameObject>>;

    public class PoolManager : DontDestroySingleton<PoolManager>
    {
        [SerializeField] private Transform monsterRoot;
        [SerializeField] private Transform itemRoot;
        [SerializeField] private Transform sfxRoot;

        protected ObjectPool monsterPools = new();
        protected ObjectPool itemPools = new();
        protected ObjectPool sfxPools = new();

        private const int defaultCapacity = 2048;

        private async UniTask<BaseGameObject> CreateBaseGameObject(ObjectType objType, string prefabName)
        {
            GameObject obj = await ResManager.Instantiate(ResType.Object, prefabName);
            BaseGameObject baseGameObj = obj.GetComponent<BaseGameObject>();

            if (baseGameObj == null)
            {
                baseGameObj = AddComponenetBaseGameObject(ref obj, objType);
            }

            return baseGameObj;
        }


        public async UniTask<GameObject> GetObject(ObjectType objType, string prefabName)
        {
            ObjectPool pool = GetPoolByType(objType);
            GameObject obj = null;
            if (!pool.TryGetValue(prefabName, out Queue<BaseGameObject> queue))
            {
                queue = new Queue<BaseGameObject>();
                pool.Add(prefabName, queue);
            }

            if (0 == queue.Count)
            {
                BaseGameObject baseGameObj = await CreateBaseGameObject(objType, prefabName);
                obj = baseGameObj.gameObject;
                obj.name = prefabName;
                SetParent(obj.transform, objType);
            }
            else
            {
                obj = queue.Dequeue().gameObject;
            }

            obj?.SetActive(true);
            return obj;
        }

        public void ReturnObject(GameObject obj)
        {
            obj.SetActive(false);
            BaseGameObject baseGameObj = obj.GetComponent<BaseGameObject>();
            ObjectPool pool = GetPoolByType(baseGameObj.Type);
            if (pool.TryGetValue(obj.name, out Queue<BaseGameObject> queue))
            {
                queue.Enqueue(baseGameObj);
            }
        }

        ObjectPool GetPoolByType(ObjectType objType)
        {
            return objType switch
            {
                ObjectType.Monster => monsterPools,
                ObjectType.Item => itemPools,
                ObjectType.SFX => sfxPools,
                _ => null
            };
        }

        BaseGameObject AddComponenetBaseGameObject(ref GameObject obj, ObjectType objType)
        {
            return objType switch
            {
                ObjectType.Monster => obj.AddComponent<Character>(),
                ObjectType.Item => obj.AddComponent<Item>(),
                ObjectType.SFX => obj.AddComponent<SFX>(),
                _ => throw new ArgumentException("Max Size must be greater than 0", "maxSize")
            };
        }

        private void SetParent(Transform obj, ObjectType objType)
        {
            switch (objType)
            {
                case ObjectType.Monster:
                    obj.SetParent(monsterRoot);
                    break;

                case ObjectType.SFX:
                    obj.SetParent(sfxRoot);
                    break;

                case ObjectType.Item:
                    obj.SetParent(itemRoot);
                    break;
            }
        }
    }
}