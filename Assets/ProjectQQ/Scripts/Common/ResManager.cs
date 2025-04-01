using System.Threading.Tasks;
using UnityEngine;

public static class ResManager
{
    public static async Task<GameObject> CreateUI(System.Type type)
    {
        var request = Resources.LoadAsync<GameObject>("Prefabs/Remote/" + type.Name);

        await request;
#if UNITY_EDITOR
        if (request.asset == null)
            Debug.LogError($"{nameof(type)} null");
#endif

        return GameObject.Instantiate(request.asset as GameObject);
    }
}
