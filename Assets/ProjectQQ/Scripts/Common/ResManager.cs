using Cysharp.Threading.Tasks;
using UnityEngine;

namespace QQ
{
    public static class ResManager
    {
        // Local
        private const string uiLocalPath = "Prefabs/UI/Remote/";

        public static async UniTask<GameObject> Instantiate(System.Type type)
        {
            var resource = await Resources.LoadAsync<GameObject>(StringBuilderPool.Get(uiLocalPath, type.Name));

            return GameObject.Instantiate(resource as GameObject);
        }
    }
}
