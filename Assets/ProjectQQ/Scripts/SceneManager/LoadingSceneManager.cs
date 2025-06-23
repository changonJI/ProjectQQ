using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace QQ
{
    public class LoadingSceneManager : MonoBehaviour
    {
        public const string mainSceneName = "MainScene";
        public const string gameSceneName = "GameScene";
        public const string loadingSceneName = "LoadingScene";

        public static string nextScene;

        // 중복 씬 로드 방지용
        private static bool canLoad = false;

        private static float fakeTime = 0.5f;

        [SerializeField] private UIProgressBar progressBar;
        
        public static void LoadScene(string sceneName)
        {
            nextScene = sceneName;

            UIIndicator.Instantiate();

            SceneManager.LoadScene("LoadingScene");
        }

        private void Awake()
        {
            // 초기화 작업
            canLoad = true;
            progressBar.Init(0f, 1f);
        }

        private void Start()
        {
            OnStart().Forget();
        }

        private async UniTaskVoid OnStart()
        {
            // fakeTime
            await UniTask.WaitForSeconds(0.5f);

            UIIndicator.CloseUI();

            // CloseUI 대기
            await UniTask.Yield();

            LoadSceneAsync(nextScene).Forget();
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
                        await UIRoot.Instance.ClearUI();

                        UIIndicator.CloseUI();

                        return;
                    }
                }
            }
        }
    }
}
