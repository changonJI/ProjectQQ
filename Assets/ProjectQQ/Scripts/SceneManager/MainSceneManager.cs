using UnityEngine;

namespace QQ
{
    public sealed class MainSceneManager : MonoBehaviour
    {
        private void Awake()
        {
            LanguageDataManager.Instance.LoadData();
        }

        private void Start()
        {
            UIMainScene.Instantiate();
        }
    }
}