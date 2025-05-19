using UnityEngine;

namespace QQ
{
    public static class ExtensionMethods
    {
        public static bool IsValidRange(this object[] objects, int index)
        {
            if (objects == null)
            {
#if UNITY_EDITOR
                Debug.LogError("objects is null");
#endif
                return false;
            }

            if(objects.Length > index)
            {
                return true;
            }
            else
            {
#if UNITY_EDITOR
                Debug.LogError("Over Arrange");
#endif
                return false;
            }
                
        }
    }

}