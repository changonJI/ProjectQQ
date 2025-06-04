using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace QQ
{
    public class PoolManager : DontDestroySingleton<PoolManager>
    {
        [SerializeField] private Transform monsterRoot;
        [SerializeField] private Transform itemRoot;
        [SerializeField] private Transform sfxRoot;

        // List: all Objects created by PoolManager
        // Queue: inactive Objects that are ready for reuse
        protected readonly Dictionary<string, (List<BaseGameObject> list, Queue<BaseGameObject> queue)> monsterPools = new Dictionary<string, (List<BaseGameObject>, Queue<BaseGameObject> queue)>(dicCapacity);
        protected readonly Dictionary<string, (List<BaseGameObject> list, Queue<BaseGameObject> queue)> itemPools = new Dictionary<string, (List<BaseGameObject>, Queue<BaseGameObject>)>(dicCapacity);
        protected readonly Dictionary<string, (List<BaseGameObject> list, Queue<BaseGameObject> queue)> sfxPools = new Dictionary<string, (List<BaseGameObject>, Queue<BaseGameObject>)>(dicCapacity);

        private const int dicCapacity = 256;
        private const int poolCapacity = 2048;

        private async UniTask<BaseGameObject> CreateBaseGameObject(ObjectType objType, string prefabName)
        {
            GameObject obj = await ResManager.Instantiate(ResType.Object, prefabName);
            BaseGameObject baseGameObj = obj.GetComponent<BaseGameObject>();

            if (baseGameObj == null)
            {
                baseGameObj = AddComponenetBaseGameObject(ref obj, objType);
                LogHelper.LogError($"오브젝트 프리팹 {prefabName}에 Object 스크립트 컴포넌트 추가 필요!!!");
            }

            baseGameObj.Init();

            return baseGameObj;
        }

        public async UniTask<GameObject> GetObject(ObjectType objType, string prefabName)
        {
            Dictionary<string, (List<BaseGameObject>, Queue<BaseGameObject>)> pool = GetPoolByType(objType);
            GameObject obj = null;


            if (!pool.TryGetValue(prefabName, out (List<BaseGameObject> list, Queue<BaseGameObject> queue) poolPair))
            {
                poolPair = (new List<BaseGameObject>(poolCapacity), new Queue<BaseGameObject>(poolCapacity));
                pool.Add(prefabName, poolPair);
            }

            Queue<BaseGameObject> queue = poolPair.Item2;

            if (0 == poolPair.queue.Count && poolCapacity > poolPair.list.Count)
            {
                // Create Object Instance
                BaseGameObject baseGameObj = await CreateBaseGameObject(objType, prefabName);
                obj = baseGameObj.gameObject;
                obj.name = prefabName;
                SetParent(obj.transform, objType);

                poolPair.list.Add(baseGameObj);
            }
            else
            {
                obj = poolPair.queue.Dequeue().gameObject;
            }

            obj?.SetActive(true);
            return obj;
        }

        public void ReleaseObject(GameObject obj)
        {
            obj.SetActive(false);
            BaseGameObject baseGameObj = obj.GetComponent<BaseGameObject>();
            Dictionary<string, (List<BaseGameObject>, Queue<BaseGameObject>)> pool = GetPoolByType(baseGameObj.Type);
            if (pool.TryGetValue(obj.name, out (List<BaseGameObject> list, Queue<BaseGameObject> queue) poolPair))
            {
                poolPair.queue.Enqueue(baseGameObj);
            }
        }

        Dictionary<string, (List<BaseGameObject>, Queue<BaseGameObject>)> GetPoolByType(ObjectType objType)
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
            BaseGameObject baseGameObj = objType switch
            {
                ObjectType.Monster => obj.AddComponent<Character>(),
                ObjectType.Item => obj.AddComponent<Item>(),
                ObjectType.SFX => obj.AddComponent<SFX>(),
                _ => null
            };

            if (null == baseGameObj)
            {
                LogHelper.LogError("objType에 맞는 BaseGameObject 클래스가 없음");
            }

            return baseGameObj;
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

        public void DestroyAll()
        {
            foreach (var poolPair in monsterPools.Values)
            {
                // Destroy All Object in List
                foreach (var obj in poolPair.list)
                {
                    if (obj != null)
                    {
                        Destroy(obj);
                    }
                }
                poolPair.list.Clear();
                // Clear queue
                poolPair.queue.Clear();
            }

            foreach (var poolPair in itemPools.Values)
            {
                foreach (var obj in poolPair.list)
                {
                    if (obj != null)
                    {
                        Destroy(obj);
                    }
                }
                poolPair.list.Clear();
                poolPair.queue.Clear();
            }

            foreach (var poolPair in sfxPools.Values)
            {
                foreach (var obj in poolPair.list)
                {
                    if (obj != null)
                    {
                        Destroy(obj);
                    }
                }
                poolPair.list.Clear();
                poolPair.queue.Clear();
            }
        }
    }
}