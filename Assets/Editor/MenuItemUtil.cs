using UnityEditor;
using UnityEditor.SceneManagement;

public static class MenuItemUtil
{
    private const string ctrl = "%";
    private const string alt = "&";
    private const string shift = "#";

#if UNITY_EDITOR
    #region MenuItemName : Util
    [MenuItem("Util/OpenGameScene " + alt + shift + "1", false, 0)]
    private static void OpenGameScene()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/MainScene.unity");
    }
    #endregion

#endif
}
