using Cysharp.Threading.Tasks;
using UnityEngine;

namespace QQ
{
    public static class ResManager
    {
        // Local
        private const string uiLocalPath = "Prefabs/UI/Remote/";
        private const string soundLocalPath = "Sounds/";

        public static AudioClip soundLoad(string fileName)
        {
            var resource = Resources.Load<AudioClip>(StringBuilderPool.Get(soundLocalPath, fileName));

            return resource;
        }

        public static async UniTask<GameObject> Instantiate(System.Type type)
        {
            var resource = await Resources.LoadAsync<GameObject>(StringBuilderPool.Get(uiLocalPath, type.Name));

            return GameObject.Instantiate(resource as GameObject);
        }
    }
}
