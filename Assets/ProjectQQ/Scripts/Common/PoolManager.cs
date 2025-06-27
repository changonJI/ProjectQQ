using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

namespace QQ
{
    public class PoolManager : DontDestroySingleton<PoolManager>
    {
        [SerializeField] private Transform ActorRoot;
        [SerializeField] private Transform monsterRoot;
        [SerializeField] private Transform itemRoot;
        [SerializeField] private Transform sfxRoot;

        // NOTE: 임시 테스트용
        public Actor actor;

        // List: all Objects created by PoolManager
        // Queue: inactive Objects that are ready for reuse
        protected readonly Dictionary<string, (List<BaseGameObject> list, Queue<BaseGameObject> queue)> monsterPools = new Dictionary<string, (List<BaseGameObject>, Queue<BaseGameObject> queue)>(dicCapacity);
        protected readonly Dictionary<string, (List<BaseGameObject> list, Queue<BaseGameObject> queue)> itemPools = new Dictionary<string, (List<BaseGameObject>, Queue<BaseGameObject>)>(dicCapacity);
        public readonly Dictionary<string, (List<BaseGameObject> list, Queue<BaseGameObject> queue)> sfxPools = new Dictionary<string, (List<BaseGameObject>, Queue<BaseGameObject>)>(dicCapacity);

        private const int dicCapacity = 256;
        private const int poolCapacity = 2048;

        public void Init() { }
        private async UniTask<BaseGameObject> CreateBaseGameObject(GameObjectType objType, string prefabName, int tableID)
        {
            GameObject obj = await ResManager.Instantiate(ObjTypeToResType(objType), prefabName);
            BaseGameObject baseGameObj = obj.GetComponent<BaseGameObject>();

            if (baseGameObj == null)
            {
                baseGameObj = AddComponenetBaseGameObject(ref obj, objType);
                LogHelper.LogError($"오브젝트 프리팹 {prefabName}에 Object 스크립트 컴포넌트 추가 필요!!!");
            }

            baseGameObj.SetData(tableID);

            return baseGameObj;
        }

        public async UniTask<GameObject> GetObject(GameObjectType objType, string prefabName, int tableID = 0)
        {
            GameObject obj = null;

            // 단일 객체는 Pool 안쓰도록
            if(objType == GameObjectType.Actor)
            {
                // Create Object Instance
                BaseGameObject baseGameObj = await CreateBaseGameObject(objType, prefabName, tableID);

                actor = baseGameObj as Actor;

                obj = baseGameObj.gameObject;
                obj.name = prefabName;
                SetParent(obj.transform, objType);

                obj?.SetActive(true);

                return obj;
            }

            Dictionary<string, (List<BaseGameObject>, Queue<BaseGameObject>)> pool = GetPoolByType(objType);


            if (!pool.TryGetValue(prefabName, out (List<BaseGameObject> list, Queue<BaseGameObject> queue) poolPair))
            {
                poolPair = (new List<BaseGameObject>(poolCapacity), new Queue<BaseGameObject>(poolCapacity));
                pool.Add(prefabName, poolPair);
            }

            Queue<BaseGameObject> queue = poolPair.queue;

            if (0 == poolPair.queue.Count && poolCapacity > poolPair.list.Count)
            {
                // Create Object Instance
                BaseGameObject baseGameObj = await CreateBaseGameObject(objType, prefabName, tableID);
                obj = baseGameObj.gameObject;
                obj.name = prefabName;
                SetParent(obj.transform, objType);

                poolPair.list.Add(baseGameObj);
            }
            else
            {
                BaseGameObject poolObject = poolPair.queue.Dequeue();
                poolObject.Init();

                obj = poolObject.gameObject;
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

        Dictionary<string, (List<BaseGameObject>, Queue<BaseGameObject>)> GetPoolByType(GameObjectType objType)
        {
            return objType switch
            {
                GameObjectType.Monster => monsterPools,
                GameObjectType.Item => itemPools,
                GameObjectType.SFX => sfxPools,
                _ => null
            };
        }

        BaseGameObject AddComponenetBaseGameObject(ref GameObject obj, GameObjectType objType)
        {
            BaseGameObject baseGameObj = objType switch
            {
                GameObjectType.Actor => obj.AddComponent<Actor>(),
                GameObjectType.Monster => obj.AddComponent<Monster>(),
                GameObjectType.Item => obj.AddComponent<Item>(),
                GameObjectType.SFX => obj.AddComponent<EffectSystem>(),
                _ => null
            };

            if (null == baseGameObj)
            {
                LogHelper.LogError("objType에 맞는 BaseGameObject 클래스가 없음");
            }

            return baseGameObj;
        }

        private void SetParent(Transform obj, GameObjectType objType)
        {
            switch (objType)
            {
                case GameObjectType.Actor:
                    obj.SetParent(ActorRoot);
                    break;

                case GameObjectType.Monster:
                    obj.SetParent(monsterRoot);
                    break;

                case GameObjectType.SFX:
                    obj.SetParent(sfxRoot);
                    break;

                case GameObjectType.Item:
                    obj.SetParent(itemRoot);
                    break;
            }
        }

        public void DestroyAll()
        {
            Destroy(actor);
            actor = null;

            foreach (var poolPair in monsterPools.Values)
            {
                // Destroy All Object in List
                foreach (var obj in poolPair.list)
                {
                    if (obj != null && obj.gameObject != null)
                    {
                        Destroy(obj.gameObject);
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
                    if (obj != null && obj.gameObject != null)
                    {
                        Destroy(obj.gameObject);
                    }
                }
                poolPair.list.Clear();
                poolPair.queue.Clear();
            }

            foreach (var poolPair in sfxPools.Values)
            {
                foreach (var obj in poolPair.list)
                {
                    if (obj != null && obj.gameObject != null)
                    {
                        Destroy(obj.gameObject);
                    }
                }
                poolPair.list.Clear();
                poolPair.queue.Clear();
            }
        }

        private ResType ObjTypeToResType(GameObjectType type)
        {
            switch (type)
            {
                case GameObjectType.Actor:
                case GameObjectType.Monster:
                case GameObjectType.Npc:
                case GameObjectType.Building:
                case GameObjectType.Item:
                    return ResType.Object;

                case GameObjectType.SFX:
                    return ResType.Effect;

                default:
                    return ResType.Object;
            }
        }
    }
}