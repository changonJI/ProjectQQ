using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace QQ
{
    public class PoolManager : DontDestroySingleton<PoolManager>
    {
        // Ǯ ������Ʈ ������
        [SerializeField] private Transform actorRoot;
        [SerializeField] private Transform monsterRoot;
        [SerializeField] private Transform npcRoot;
        [SerializeField] private Transform buildingRoot;
        [SerializeField] private Transform itemRoot;
        [SerializeField] private Transform projectileRoot;
        [SerializeField] private Transform sfxRoot;


        /// <summary> ������Ʈ Ǯ </summary>
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

        public GameObject Get(ObjectType type, int id)
        {
            BaseGameObject baseObj = null;
            // ������
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

            // ���� ���ӿ�����Ʈ ���
            GameObject gameObject = baseObj.gameObject;
            gameObject.SetActive(true);

            // ������ ����
            baseObj.SetData(id);

            return gameObject;
        }

        public void Release(BaseGameObject releaseObject)
        {
            releaseObject.gameObject.SetActive(false);

            // Ǯ�� ��ȯ
            if (objectPools.TryGetValue(releaseObject.Type, out var pool))
            {
                pool.Release(releaseObject);
            }
            else
            {
                return;
            }
        }

        public void Release(GameObject releaseObject)
        {
            Release(releaseObject.GetComponent<BaseGameObject>());
        }

        public void DestroyAll()
        {
            foreach (var pool in objectPools.Values)
            {
                pool.Clear();
            }
        }

        /// <summary>
        /// ������ƮǮ �����ÿ� ����� Create �Լ�
        /// </summary>
        /// <param name="objectType"></param>
        /// <returns></returns>
        private Func<BaseGameObject> CreateFunc(ObjectType objectType)
        {
            return () =>
            {
                Type componentType = GetComponentType(objectType);

                GameObject obj = ResManager.InstantiateSync(PrefabType.Object, componentType);
                obj.SetActive(false);
#if UNITY_EDITOR
                obj.name = StringBuilderPool.Get(objectType.ToString(), GetAllObjectCount(objectType).ToString());
#endif

                // �θ� �����ϱ�
                obj.transform.SetParent(GetParentTransform(objectType));

                // BaseGameObject ���� Ȯ��
                BaseGameObject baseGameObject = obj.GetComponent<BaseGameObject>();
                if (null == baseGameObject)
                {
                    baseGameObject = obj.AddComponent(componentType) as BaseGameObject;
                }

                return baseGameObject;
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
                Debug.LogWarning($"PoolManager.GetComponentType {objectType} Ÿ�� ���� �ȵ�");
            }
#endif
            return componentType;

        }

        /// <summary>
        /// ������Ʈ Ÿ�Ժ� �θ� ��ü ���
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
                Debug.LogWarning($"PoolManager.GetParentTransform {objectType} Ÿ�� �θ� ���� �ȵ�");
            }
#endif
            return parentTransform;
        }
    }
}