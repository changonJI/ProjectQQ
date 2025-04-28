using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace QQ
{
    public class LoadingSceneManager : MonoBehaviour
    {
        public static string nextScene;
        private static float fakeTime = 0.5f;

        [SerializeField] private UIProgressBar progressBar;

        private void Start()
        {
            LoadSceneAsync().Forget();
        }

        public static void LoadScene(string sceneName)
        {
            nextScene = sceneName;
            SceneManager.LoadScene("LoadingScene");
        }

        private async UniTask LoadSceneAsync()
        {
            await UniTask.Yield(); // 초기화 작업 대체

            AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);

            op.allowSceneActivation = false;

            float fakeTimer = 0f;

            progressBar.Init();

            while (!op.isDone)
            {
                await UniTask.Yield(); // 프레임마다 대기

                if (op.progress < 0.9f)
                {
                    progressBar.CurValue = op.progress;
                }
                else
                {
                    fakeTimer += Time.unscaledDeltaTime * fakeTime;
                    Debug.Log($"Loading : {fakeTimer}");

                    progressBar.CurValue = Mathf.Lerp(0.9f, 1f, fakeTimer);

                    if(progressBar.CurValue >= 1f)
                    {
                        op.allowSceneActivation = true;
                        return;
                    }
                }
            }
        }

    }
}
