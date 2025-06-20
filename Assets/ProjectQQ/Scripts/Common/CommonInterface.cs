using UnityEngine;

namespace QQ
{
    public interface IOwnable
    {
        public void Init(BaseGameObject ownerObj);
        public BaseGameObject Owner { get; }
    }
}