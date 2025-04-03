using Cysharp.Threading.Tasks;
using UnityEngine;

public static class ResManager
{
    public static async UniTask<GameObject> Instantiate(System.Type type)
    {
        var resource = await Resources.LoadAsync<GameObject>("Prefabs/UI/Remote/" + type.Name);
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
