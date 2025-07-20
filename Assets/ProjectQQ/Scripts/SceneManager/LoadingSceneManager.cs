using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace QQ
{
    public enum SceneEntryType
    {
        Default,        // 기본
        EnterStage,     // 스테이지 진입 시 (→ Dialogue 호출)
        ReloadScene,    // 재시작
        ReturnToMenu    // 메인으로 돌아감
    }
    
    public class LoadingSceneManager : MonoBehaviour
    {
        public const string mainSceneName = "MainScene";
        public const string gameSceneName = "GameScene";
        public const string loadingSceneName = "LoadingScene";

        public static string nextScene;
        public static SceneEntryType entryType = SceneEntryType.Default;

        // 중복 씬 로드 방지용
        private static bool canLoad = false;

        private static float fakeTime = 0.5f;

        [SerializeField] private UIProgressBar progressBar;
        
        public static void LoadScene(SceneType sceneType, SceneEntryType sceneEntryType = SceneEntryType.Default)
        {
            switch (sceneType)
            {
                case SceneType.MainScene:
                    nextScene = mainSceneName;
                    break;
                case SceneType.LoadingScene:
                    nextScene = loadingSceneName;
                    break;
                case SceneType.GameScene:
                    nextScene = gameSceneName;
                    break;
            }

            entryType = sceneEntryType;

            UIIndicator.Instantiate();
            SceneManager.LoadScene(loadingSceneName);
        }

        private void Awake()
        {
            // 초기화 작업
            canLoad = true;
            progressBar.Init(0f, 1f);

            if (entryType == SceneEntryType.Default)
                entryType = SceneEntryType.EnterStage;
        }

        private void Start()
        {
            switch (entryType)
            {
                case SceneEntryType.EnterStage:
                    Init().Forget(); // UIDialogue → GameScene
                    break;

                default:
                    LoadSceneAsync(nextScene).Forget(); // 바로 씬 로딩
                    break;
            }
        }

        private async UniTaskVoid Init()
        {
            // fakeTime
            await UniTask.WaitForSeconds(0.5f);

            UIIndicator.CloseUI();

            // CloseUI 대기
            await UniTask.Yield();

            UIDialogue.Instantiate(okAction: () => LoadSceneAsync(gameSceneName).Forget());

        }

        private async UniTaskVoid LoadSceneAsync(string sceneName)
        {
            // Indicator On
            UIIndicator.Instantiate();

            if (!canLoad) return;

            canLoad = false;

            // 초기화 작업 대체
            await UniTask.Yield(); 

            AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);

            op.allowSceneActivation = false;

            float fakeTimer = 0f;

            progressBar.Init();

            while (!op.isDone)
            {
                // 프레임마다 대기
                await UniTask.Yield();

                if (op.progress < 0.9f)
                {
                    progressBar.CurValue = op.progress;
                }
                else
                {
                    fakeTimer += Time.unscaledDeltaTime * fakeTime;
                    
                    progressBar.CurValue = Mathf.Lerp(0.9f, 1f, fakeTimer);

                    if (progressBar.CurValue >= 1f)
                    {
                        op.allowSceneActivation = true;

                        // UIRoot 초기화
                        if (UIRoot.Instance != null)
                        {
                            await UIRoot.Instance.ClearUI();
                        }
                        else
                        {
                            Debug.LogWarning("UIRoot.Instance is null. Skipping ClearUI().");
                        }

                        UIIndicator.CloseUI();

                        return;
                    }
                }
            }
        }
    }
}
