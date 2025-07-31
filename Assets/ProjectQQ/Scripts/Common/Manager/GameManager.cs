using QQ;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : DontDestroySingleton<GameManager>
{
    public override void Init() { }
   
    #region Playerprefs
    /// <summary>
    /// playerprefs 초기화
    /// </summary>
    public void ClearPlayerData() => PlayerPrefs.DeleteAll();

    public bool IsValidPlayer()
    {
        return PlayerPrefs.HasKey(PlayerDataType.UserName.ToString());
    }

    /// <summary>
    /// playerprefs에 저장
    /// </summary>
    public void SavePlayerData(PlayerDataType dataType, string text = "", int iNum = -1, float fNum = 0f)
    {
        switch (dataType)
        {
            case PlayerDataType.UserName:
                PlayerPrefs.SetString(dataType.ToString(), text);
                break;
            
            case PlayerDataType.FirstPlay:
                PlayerPrefs.SetInt(dataType.ToString(), iNum);
                break;

            case PlayerDataType.Country:
                PlayerPrefs.SetString(dataType.ToString(), text);
                break;
        }
    }
    
    /// <summary>
    /// string 데이터 로드
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
            LogHelper.LogError("데이터 타입을 확인하세요.");
        
        return data;
    }
    
    /// <summary>
    /// int 데이터 로드
    /// </summary>
    public int GetIntPlayerData(PlayerDataType dataType)
    {
        int data = -1;
        
        switch (dataType)
        {
            case PlayerDataType.FirstPlay :
                data = PlayerPrefs.GetInt(dataType.ToString());
                break;
            case PlayerDataType.Country:
                data = PlayerPrefs.GetInt(dataType.ToString());
                break;

            default:
                break;
        }
        
        if(data == -1)
            LogHelper.LogError("데이터 타입을 확인하세요.");
        
        return data;
    }
    #endregion

    #region Time

    private bool isPaused = false;
    
    /// <summary>
    /// 게임 멈춤 - 토글 형식
    /// </summary>
    public void TogglePause()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0 : 1;
    }

    /// <summary>
    /// 게임 멈춤 - bool 값 이용
    /// </summary>
    /// <param name="isPaused"></param>
    public void TimeScaleChanger(bool isPaused)
    {
        Time.timeScale = isPaused ? 0 : 1;
    }

    #endregion

    #region Scene

    /// <summary>
    /// 동기 씬 로드
    /// </summary>
    public void LoadScene(SceneType sceneType, SceneEntryType entryType = SceneEntryType.Default)
    {
        if (SceneExists(sceneType.ToString()))
        {
            LoadingSceneManager.LoadScene(sceneType, entryType);
        }
        else
        {
            Debug.LogError($"[SceneLoader] 씬 '{sceneType}' 이(가) Build Settings에 존재하지 않습니다.");
        }
    }

    /// <summary>
    /// 씬 존재 여부 확인
    /// </summary>
    private bool SceneExists(string sceneName)
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
    
    #region GameCycle

    public void GamePause()
    {
        TimeScaleChanger(true);
        UIPause.Instantiate();
    }

    public void GameResume()
    {
        TimeScaleChanger(false);
        UIPause.CloseUI();
    }

    public void BackToMain()
    {
        TimeScaleChanger(false);
        
        UIGameHud.CloseUI();
        PoolManager.Instance.DestroyAll();
        
        LoadScene(SceneType.MainScene, SceneEntryType.ReturnToMenu);
    }

    #endregion
}
