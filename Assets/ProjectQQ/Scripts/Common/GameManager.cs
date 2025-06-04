using System;
using QQ;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : DontDestroySingleton<GameManager>
{
    #region Playerprefs

    /// <summary>
    /// playerprefs �ʱ�ȭ
    /// </summary>
    public void ClearPlayerData() => PlayerPrefs.DeleteAll();

    /// <summary>
    /// playerprefs�� ����
    /// </summary>
    public void SavePlayerData(PlayerDataType dataType, string text = null, int iNum = -1, float fNum = 0f)
    {
        switch (dataType)
        {
            case PlayerDataType.UserName:
                PlayerPrefs.SetString(dataType.ToString(), text);
                break;
            
            case PlayerDataType.FirstPlay:
                PlayerPrefs.SetInt(dataType.ToString(), iNum);
                break;
        }
    }
    
    /// <summary>
    /// string ������ �ε�
    /// </summary>
    public string GetStringPlayerData(PlayerDataType dataType)
    {
        string data = string.Empty;
        
        switch (dataType)
        {
            case PlayerDataType.UserName :
                data = PlayerPrefs.GetString(dataType.ToString());
                break;
            
            default:
                break;
        }
        
        if(dataType != PlayerDataType.UserName && data == string.Empty)
            LogHelper.LogError("������ Ÿ���� Ȯ���ϼ���.");
        
        return data;
    }
    
    /// <summary>
    /// int ������ �ε�
    /// </summary>
    public int GetIntPlayerData(PlayerDataType dataType)
    {
        int data = -1;
        
        switch (dataType)
        {
            case PlayerDataType.FirstPlay :
                data = PlayerPrefs.GetInt(dataType.ToString());
                break;
            
            default:
                break;
        }
        
        if(data == -1)
            LogHelper.LogError("������ Ÿ���� Ȯ���ϼ���.");
        
        return data;
    }

    #endregion

    #region Time

    private bool isPaused = false;
    
    /// <summary>
    /// ���� ���� - ��� ����
    /// </summary>
    public void TogglePause()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0 : 1;
    }

    /// <summary>
    /// ���� ���� - bool �� �̿�
    /// </summary>
    /// <param name="isPaused"></param>
    public void GamePause(bool isPaused)
    {
        Time.timeScale = isPaused ? 0 : 1;
    }

    #endregion

    #region Scene

    /// <summary>
    /// ���� �� �ε�
    /// </summary>
    public static void LoadScene(SceneType scene)
    {
        if (SceneExists(scene.ToString()))
        {
            SceneManager.LoadScene(scene.ToString());
        }
        else
        {
            Debug.LogError($"[SceneLoader] �� '{scene}' ��(��) Build Settings�� �������� �ʽ��ϴ�.");
        }
    }

    /// <summary>
    /// �񵿱� �� �ε�
    /// </summary>
    public static void LoadSceneAsync(SceneType scene, Action<bool> onComplete = null)
    {
        if (SceneExists(scene.ToString()))
        {
            AsyncOperation operation = SceneManager.LoadSceneAsync(scene.ToString());
            operation.completed += _ => onComplete?.Invoke(true);
        }
        else
        {
            Debug.LogError($"[SceneLoader] �� '{scene}' ��(��) Build Settings�� �������� �ʽ��ϴ�.");
            onComplete?.Invoke(false);
        }
    }

    /// <summary>
    /// �� ���� ���� Ȯ��
    /// </summary>
    private static bool SceneExists(string sceneName)
    {
        int sceneCount = SceneManager.sceneCountInBuildSettings;
        for (int i = 0; i < sceneCount; i++)
        {
            string path = SceneUtility.GetScenePathByBuildIndex(i);
            string name = System.IO.Path.GetFileNameWithoutExtension(path);
            if (name == sceneName)
                return true;
        }
        return false;
    }

    #endregion
    
}
