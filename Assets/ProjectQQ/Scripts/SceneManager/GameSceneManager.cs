using UnityEngine;

namespace QQ
{
    public sealed class GameSceneManager : MonoBehaviour
    {
        private void Start()
        {
            UIDialogue.Instantiate();
        }
    }
}