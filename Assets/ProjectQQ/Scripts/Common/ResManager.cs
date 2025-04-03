using Cysharp.Threading.Tasks;
using UnityEngine;

namespace QQ
{
    public static class ResManager
    {
        private const string uiPath = "Prefabs/UI/Remote/";

        public static async UniTask<GameObject> Instantiate(System.Type type)
        {
            var resource = await Resources.LoadAsync<GameObject>(uiPath + type.Name);
#if UNITY_EDITOR
            if (resource == null)
            {
                Debug.LogError($"{nameof(type)} null");
                return null;
            }
#endif
            return GameObject.Instantiate(resource as GameObject);
        }
    }
}
