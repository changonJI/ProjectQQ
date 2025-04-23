using UnityEngine;
using UnityEngine.SceneManagement;

namespace QQ
{
    public sealed class MainSceneManager : MonoBehaviour
    {
        private void Start()
        {
            LanguageDataManager.Instance.LoadData();
        }

        public void TestSceneLoad()
        {
            SceneManager.LoadScene("GameScene");
        }

        public void Test()
        {
            Debug.Log("Test");
        }
    }
}