using Cysharp.Threading.Tasks;
using UnityEngine;

namespace QQ
{
    public static class ResManager
    {
        // Local
        private const string uiLocalPath = "Prefabs/UI/Remote/";
        private const string soundLocalPath = "Sounds/";
        private const string stageLocalPath = "Prefabs/Stage/";
        private const string objectLocalPath = "Prefabs/Object/";
        private const string textureLocalPath = "Image/UI/";

        /// <summary>
        /// Load a resource from the Resources folder In General
        /// </summary>
        public static T LoadResource<T>(ResType type, string fileName) where T : Object
        { 
            string path = GetResourcePath(type, fileName);

            var resource = Resources.Load<T>(path);

            if (resource == null)
            {
                LogHelper.LogError($"Resource not found : {fileName}");
            }
            return resource;
        }

        /// <summary>
        /// UI Prefab load and instantiate
        /// </summary>
        public static async UniTask<GameObject> Instantiate(System.Type type)
        {
            var resource = await Resources.LoadAsync<GameObject>(StringBuilderPool.Get(uiLocalPath, type.Name));

            return GameObject.Instantiate(resource as GameObject);
        }

        public static async UniTask<GameObject> Instantiate(ResType type, string name)
        {
            string path = GetResourcePath(type, name);

            var resource = await Resources.LoadAsync<GameObject>(path);

            return GameObject.Instantiate(resource as GameObject);
        }

        private static string GetResourcePath(ResType type, string name)
        {
            switch (type)
            {
                case ResType.UI:
                    return StringBuilderPool.Get(uiLocalPath, name);
                case ResType.Sound:
                    return StringBuilderPool.Get(soundLocalPath, name);
                case ResType.Object:
                    return StringBuilderPool.Get(objectLocalPath, name);
                case ResType.Stage:
                    return StringBuilderPool.Get(stageLocalPath, name);
                case ResType.Texture:
                    return StringBuilderPool.Get(textureLocalPath, name);
                default:
                    LogHelper.LogError($"Unknown resource type: {type}");
                    return null;
            }
        }
    }
}
