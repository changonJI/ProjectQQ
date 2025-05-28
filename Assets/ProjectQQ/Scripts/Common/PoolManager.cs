using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace QQ
{
    public class PoolManager : DontDestroySingleton<PoolManager>
    {
        // 풀 오브젝트 보관함
        [SerializeField] private Transform actorRoot;
        [SerializeField] private Transform monsterRoot;
        [SerializeField] private Transform npcRoot;
        [SerializeField] private Transform buildingRoot;
        [SerializeField] private Transform itemRoot;
        [SerializeField] private Transform projectileRoot;
        [SerializeField] private Transform sfxRoot;


        /// <summary> 오브젝트 풀 </summary>
        protected Dictionary<ObjectType, ObjectPool<BaseGameObject>> objectPools = new();

        private const int defaultCapacity = 2048;

        public int PoolCount => objectPools.Count;
        public bool IsContainsPool(ObjectType type) { return objectPools.ContainsKey(type); }

        public PoolManager()
        {
            foreach (ObjectType objectType in Enum.GetValues(typeof(ObjectType)))
            {
                if (ObjectType.Default == objectType)
                {
                    continue;
                }

                Func<BaseGameObject> createFunc = CreateFunc(objectType);
                if (null == createFunc)
                {
                    continue;
                }

                ObjectPool<BaseGameObject> pool = new ObjectPool<BaseGameObject>(
                    createFunc: createFunc,
                    actionOnGet: obj => obj.GetFromPool(),
                    actionOnRelease: obj => obj.ReturnToPool(),
                    actionOnDestroy: obj => obj.DestroyFromPool(),
                    defaultCapacity: defaultCapacity
                    );

                objectPools.Add(objectType, pool);
            }
        }

        public int GetAllObjectCount(ObjectType type)
        {
            if (objectPools.TryGetValue(type, out var pool))
            {
                return pool.CountAll;
            }

            return 0;
        }

        public int GetInactiveObjectCount(ObjectType type)
        {
            if (objectPools.TryGetValue(type, out var pool))
            {
                return pool.CountInactive;
            }

            return 0;
        }

        public int GetActiveObjectCount(ObjectType type)
        {
            if (objectPools.TryGetValue(type, out var pool))
            {
                return pool.CountActive;
            }

            return 0;
        }

        public GameObject Get(ObjectType type)
        {
            BaseGameObject baseObj = null;
            // 꺼내기
            if (objectPools.TryGetValue(type, out var pool))
            {
                baseObj = pool.Get();
                if (null == baseObj)
                {
                    return null;
                }
            }
            else
            {
                return null;
            }

            // 연결 게임오브젝트 얻기
            GameObject gameObject = baseObj.gameObject;
            gameObject.SetActive(true);

            // 데이터 세팅

            return gameObject;
        }

        public void Release(BaseGameObject releaseObject)
        {
            releaseObject.gameObject.SetActive(false);

            // 풀에 반환
            if (objectPools.TryGetValue(releaseObject.Type, out var pool))
            {
                pool.Release(releaseObject);
            }
            else
            {
                return;
            }
        }

        public void DestroyAll()
        {
            foreach (var pool in objectPools.Values)
            {
                pool.Clear();
            }
        }

        /// <summary>
        /// 오브젝트풀 생성시에 등록할 Create 함수
        /// </summary>
        /// <param name="objectType"></param>
        /// <returns></returns>
        private Func<BaseGameObject> CreateFunc(ObjectType objectType)
        {
            return () =>
            {
                GameObject obj = new GameObject(StringBuilderPool.Get(objectType.ToString(), GetAllObjectCount(objectType).ToString()));
                obj.SetActive(false);

                // 부모 설정하기
                obj.transform.SetParent(GetParentTransform(objectType));

                Type componentType = GetComponentType(objectType);
                return (BaseGameObject)obj.AddComponent(componentType);
            };
        }

        private Type GetComponentType(ObjectType objectType)
        {
            Type componentType = objectType switch
            {
                ObjectType.Actor => typeof(Character),
                ObjectType.Monster => typeof(Character),
                ObjectType.Npc => typeof(Character),
                ObjectType.Building => typeof(Building),
                ObjectType.Item => typeof(Item),
                ObjectType.Projectile => typeof(Projectile),
                ObjectType.SFX => typeof(SFX),
                _ => null
            };

            // default case
#if UNITY_EDITOR
            if (null == componentType)
            {
                Debug.LogWarning($"PoolManager.GetComponentType {objectType} 타입 정의 안됨");
            }
#endif
            return componentType;

        }

        /// <summary>
        /// 오브젝트 타입별 부모 객체 얻기
        /// </summary>
        /// <param name="objectType"></param>
        /// <param name="isActive"></param>
        /// <returns></returns>
        private Transform GetParentTransform(ObjectType objectType)
        {
            Transform parentTransform = objectType switch
            {
                ObjectType.Actor => actorRoot,
                ObjectType.Monster => monsterRoot,
                ObjectType.Npc => npcRoot,
                ObjectType.Building => buildingRoot,
                ObjectType.Item => itemRoot,
                ObjectType.Projectile => projectileRoot,
                ObjectType.SFX => sfxRoot,
                _ => null
            };

            // default case
#if UNITY_EDITOR
            if (null == parentTransform)
            {
                Debug.LogWarning($"PoolManager.GetParentTransform {objectType} 타입 부모 정의 안됨");
            }
#endif
            return parentTransform;
        }
    }
}