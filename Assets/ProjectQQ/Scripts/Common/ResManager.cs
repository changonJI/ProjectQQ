using Cysharp.Threading.Tasks;
using UnityEngine;

namespace QQ
{
    public static class ResManager
    {
        // Local
        private const string uiLocalPath = "Prefabs/UI/Remote/";
        private const string soundLocalPath = "Sounds/";
        private const string objectLocalPath = "Prefabs/object/";

        public static AudioClip soundLoad(string fileName)
        {
            var resource = Resources.Load<AudioClip>(StringBuilderPool.Get(soundLocalPath, fileName));

            return resource;
        }

        public static GameObject InstantiateSync(PrefabType prefabType, System.Type type)
        {
            var resource = prefabType switch
            {
                PrefabType.Object => Resources.Load<GameObject>(StringBuilderPool.Get(objectLocalPath, type.Name)),
                _ => null
            };

            return GameObject.Instantiate(resource);
        }

        public static async UniTask<GameObject> Instantiate(System.Type type)
        {
            var resource = await Resources.LoadAsync<GameObject>(StringBuilderPool.Get(uiLocalPath, type.Name));

            return GameObject.Instantiate(resource as GameObject);
        }
    }
}
