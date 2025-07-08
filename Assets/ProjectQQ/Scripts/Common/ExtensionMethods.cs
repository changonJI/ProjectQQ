using UnityEngine;

namespace QQ
{
    public static class ExtensionMethods
    {
        public static bool IsValidRange(this object[] objects, int index)
        {
            if (objects == null)
            {
                LogHelper.LogError("objects is null");

                return false;
            }

            if (objects.Length > index)
            {
                return true;
            }
            else
            {
                LogHelper.LogError("Over Arrange");

                return false;
            }

        }

        public static T AddComponent<T>(this GameObject obj, BaseGameObject owner) where T : MonoBehaviour, IOwnable
        {
            T component = obj.AddComponent<T>();
            component.Init(owner);

            return component;
        }

        public static string ToText(this int id, LanguageType type = LanguageType.UI)
        {
            int getId = id + (int)type * 100000;
            if (id <= 0)
            {
                LogHelper.LogError("id is less than or equal to 0");
                return string.Empty;
            }
            else
            {
                return LanguageDataManager.Instance.Get(getId, (ConturyType)GameManager.Instance.GetIntPlayerData(PlayerDataType.Country));
            }
        }
    }
}