using UnityEngine;

namespace QQ 
{
    public sealed class TestEmpty : MonoBehaviour
    {
        [ContextMenu("TestMain")]
        private void TestMain()
        {
            Debug.Log("testMain");
        }

        [ContextMenu("Test1")]
        private void Test1()
        {
            Debug.Log("test1");
        }

        [ContextMenu("Test2")]
        private void Test2()
        {
            Debug.Log("test2");
        }

        [ContextMenu("Test3")]
        private void Test3()
        {
            Debug.Log("test3");
        }

        [ContextMenu("Test4")]
        private void Test4()
        {
            Debug.Log("test4");
        }

    }
}
