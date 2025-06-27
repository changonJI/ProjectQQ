using Cysharp.Threading.Tasks;
using QQ;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : DontDestroySingleton<GameManager>
{
    public CinemachineCamera virtualCam;
    private readonly Vector3 camPos = new Vector3(0, 0, -100); 

    public void Init() { }

    public async UniTask InitCamera()
    {
        await UniTask.WaitForEndOfFrame();

        SetCamera(Camera.main.GetComponent<CinemachineBrain>());

        //방어코드
        while (virtualCam == null)
        {
            await UniTask.Yield();

            SetCamera(Camera.main.GetComponent<CinemachineBrain>());
        }
    }

    public void SetCamera(CinemachineBrain brain)
    {
        virtualCam = brain.ActiveVirtualCamera as CinemachineCamera;
    }

    public void SetCameraTarget(Transform target)
    {
        if (virtualCam == null) return;

        virtualCam.Follow = target;
        virtualCam.LookAt = target;
    }

    #region Playerprefs
    /// <summary>
    /// playerprefs 초기화
    /// </summary>
    public void ClearPlayerData() => PlayerPrefs.DeleteAll();

    /// <summary>
    /// playerprefs에 저장
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
    public void GamePause(bool isPaused)
    {
        Time.timeScale = isPaused ? 0 : 1;
    }

    #endregion

    #region Scene

    /// <summary>
    /// 동기 씬 로드
    /// </summary>
    public void LoadScene(SceneType sceneType)
    {
        if (SceneExists(sceneType.ToString()))
        {
            LoadingSceneManager.LoadScene(sceneType);
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
    
}
