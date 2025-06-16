using Spine;
using Spine.Unity;
using System.Collections.Generic;
using UnityEngine;

namespace QQ
{
    [DisallowMultipleComponent]
    public abstract class BaseGameObject : MonoBehaviour
    {
        public abstract GameObjectType Type { get; }

        private bool isLoaded = false;
        public bool IsLoaded
        {
            get { return isLoaded; }
            set { isLoaded = value; }
        }

        #region animation 관련 변수
        // 현재 애니메이션 상태값
        private AnimState curAnimState;
        // Body Skeleton
        [SerializeField] private SkeletonAnimation animBody;
        private Skin skinBody;
        private readonly Dictionary<string, Spine.Animation> dicAnimBody = new Dictionary<string, Spine.Animation>();
        // Weapon Skeleton
        [SerializeField] private SkeletonAnimation animWeapon;
        private Skin skinWeapon;
        private readonly Dictionary<string, Spine.Animation> dicAnimWeapon = new Dictionary<string, Spine.Animation>();
        #endregion

        /// <summary>
        /// BaseGameObject 생성시 항상 Init() 동작
        /// </summary>
        public BaseGameObject()
        {
            Init();
        }

        public abstract void Init();

        public abstract void SetData(int id);

        public void SetParent(Transform transform)
        {
            gameObject.transform.SetParent(transform);
        }

        /// <summary>
        /// Spine 데이터의 Layer 설정
        /// </summary>
        /// <param name="anim">스켈레톤 데이터</param>
        /// <param name="animSlot">Body, Weapon Type</param>
        private void SetLayer(SkeletonAnimation anim, AnimSlotType animSlot)
        {
            var mesh = anim.transform.GetComponent<MeshRenderer>();

            // sorting layer 설정
            mesh.sortingLayerID = SortingLayer.NameToID(SortingLayerName.Object.ToString());
            // order in layer 설정
            mesh.sortingOrder = animSlot == AnimSlotType.Body ? (int)OrderInSortingLayer.OBJBody
                                                              : (int)OrderInSortingLayer.OBJWeapon;
        }

        /// <summary>
        /// Skeleton 별 layer 세팅 및 애니메이션 초기화
        /// </summary>
        private void InitAnimation()
        {
            // Body SkeletonAnimation 초기화
            if (animBody != null)
            {
                // 초기화 안할시 skeletionanimation.skeleton이 null
                animBody.Initialize(false);

                // sorting layer, order in layer 세팅
                SetLayer(animBody, AnimSlotType.Body);

                // skeleton Data에 접근하여 animations string값 캐싱
                foreach (var anim in animBody.skeleton.Data.Animations)
                {
                    if (!dicAnimBody.ContainsKey(anim.Name))
                    {
                        dicAnimBody.Add(anim.Name, anim);
                    }
                }
            }
            else
            {
                LogHelper.LogError("animBody is not assigned.");
            }

            // Weapon SkeletonAnimation 초기화, Actor만 적용
            if (animWeapon != null && Type == GameObjectType.Actor)
            {
                // 초기화 안할시 skeletionanimation.skeleton이 null
                animWeapon.Initialize(false);

                // sorting layer, order in layer 세팅
                SetLayer(animWeapon, AnimSlotType.Weapon);

                // skeleton Data에 접근하여 animations string값 캐싱
                foreach (var anim in animWeapon.skeleton.Data.Animations)
                {
                    if (!dicAnimWeapon.ContainsKey(anim.Name))
                    {
                        dicAnimWeapon.Add(anim.Name, anim);
                    }
                }
            }
            else
            {
                LogHelper.LogError("animWeapon is not assigned.");
            }

            // 애니메이션 초기화
            SetCurAnimation(AnimState.Idle);
        }

        /// <summary>
        /// Skeleton의 customSkin 변수 초기화. InitAnimation() 이후 호출할것.(Skeleton.skeleton이 null일 수 있음)
        /// </summary>
        private void InitSkin()
        {
            // skin temp 변수 생성
            skinBody = new Skin("BodySkin");
            skinWeapon = new Skin("WeaponSkin");

            var findSkinBody = animBody.skeleton.Data.FindSkin("default");
            if (findSkinBody != null)
            {
                skinBody.AddSkin(findSkinBody);
                animBody.skeleton.SetSkin(skinBody);
            }
            else
            {
                LogHelper.LogError("default skin not found for Body.");
            }

            if (Type == GameObjectType.Actor)
            {
                var findSkinWeapon = animWeapon.skeleton.Data.FindSkin("Weapon0");

                if (findSkinWeapon != null)
                {
                    skinWeapon.AddSkin(findSkinWeapon);
                    animWeapon.skeleton.SetSkin(skinWeapon);
                }
                else
                {
                    LogHelper.LogError("weapon0 skin not found for weapon.");
                }
            }
        }

        /// <summary>
        /// 현재 스킨을 설정하는 코드.
        /// </summary>
        /// <param name="skinName"></param>
        public void SetCurSkin(string skinName)
        {
            // 현재 스킨과 비교하여 다를 경우에만 스킨을 변경
            if (animBody.skeleton.Skin.Name != skinName)
            {
                // 커스텀 스킨 초기화
                skinBody.Clear();
                // 기존 skeleton의 skin 추가
                skinBody.AddSkin(animBody.skeleton.Data.FindSkin(skinName));

                // skeleton에 스킨 적용
                animBody.skeleton.SetSkin(skinBody);
                animBody.skeleton.SetSlotsToSetupPose();
                animBody.AnimationState.Apply(animBody.skeleton);
            }
        }

        /// <summary>
        /// Body,Weapon의 해당 slot에서 Attachment을 설정하는 코드
        /// </summary>
        /// <param name="type">AnimSlotType</param>
        /// <param name="slotName">Spine Slot Name</param>
        /// <param name="attachmentName">Spine AtachemntName</param>
        public void SetCurAttachment(AnimSlotType type, string slotName, string attachmentName)
        {
            SetAttachment(type == AnimSlotType.Body ? animBody : animWeapon, slotName, attachmentName);
        }

        /// <summary>
        /// SetCurAttachment()에서 호출되는 코드.
        /// </summary>
        /// <param name="anim">Body or Weapon Skeleton</param>
        /// <param name="slotName">Spine Slot Name</param>
        /// <param name="attachmentName">Spine AtachemntName</param>
        private void SetAttachment(SkeletonAnimation anim, string slotName, string attachmentName)
        {
            var slot = anim.skeleton.Data.FindSlot(slotName);
            if (slot == null)
            {
                LogHelper.LogError($"Slot '{slotName}' not found in skeleton data.");
                return;
            }

            var attach = anim.skeleton.GetAttachment(slot.Name, attachmentName);
            if (attach == null)
            {
                LogHelper.LogError($"Attachment '{attachmentName}' not found in slot '{slotName}'.");
                return;
            }

            anim.skeleton.SetAttachment(slotName, attachmentName);
            anim.skeleton.SetSlotsToSetupPose();
            anim.AnimationState.Apply(animBody.skeleton);
        }

        /// <summary>
        /// SetAnimation을 실행하는 코드
        /// </summary>
        /// <param name="state">현재 상태값</param>
        /// <param name="timeScale">시간값</param>
        public void SetCurAnimation(AnimState state, float timeScale = 1f)
        {
            if (curAnimState == state)
                return;

            SetAnimation(AnimSlotType.Body, GetAnimName(state), GetAnimLoop(state), timeScale);
            SetAnimation(AnimSlotType.Weapon, GetAnimName(state), GetAnimLoop(state), timeScale);

            curAnimState = state;
        }

        /// <summary>
        /// animation 설정 코드
        /// </summary>
        /// <param name="type">Body, Weapon</param>
        /// <param name="animName">캐싱된 animation name값</param>
        /// <param name="loop">반복체크</param>
        /// <param name="timeScale">시간값</param>
        private void SetAnimation(AnimSlotType type, string animName, bool loop, float timeScale = 1)
        {
            if (type == AnimSlotType.Body && animBody != null)
            {
                if (dicAnimBody.TryGetValue(animName, out var anim))
                {
                    // TrackEntry TimeScale 값을 설정하면, 게임설정의 TimeScale의 영향을 받지 않고 독립적으로 세팅한다.
                    animBody.state.SetAnimation(0, anim, loop).TimeScale = timeScale;
                    animBody.loop = loop;
                    animBody.timeScale = timeScale;
                }
                else
                {
                    LogHelper.LogError($"Animation '{animName}' not found in Body animations.");
                }
            }
            else if (type == AnimSlotType.Weapon && animWeapon != null)
            {
                if (dicAnimWeapon.TryGetValue(animName, out var anim))
                {
                    animWeapon.state.SetAnimation(0, anim, loop).TimeScale = timeScale;
                    animWeapon.loop = loop;
                    animWeapon.timeScale = timeScale;
                }
                else
                {
                    LogHelper.LogError($"Animation '{animName}' not found in Weapon animations.");
                }
            }
        }

        /// <summary>
        /// 현재 상태별 Spine animation name값 return
        /// </summary>
        private string GetAnimName(AnimState state)
        {
            switch (state)
            {
                case AnimState.Idle:
                    return "idle";
                case AnimState.Run:
                    return "run";
                case AnimState.Roll:
                    return "roll";
                default:
                    LogHelper.LogError($"Unknown animation state: {state}");
                    return string.Empty;
            }
        }

        /// <summary>
        /// 현재 상태별 Spine animation Loop 값 return
        /// </summary>
        private bool GetAnimLoop(AnimState state)
        {
            switch (state)
            {
                case AnimState.Idle:
                    return true;
                case AnimState.Run:
                    return true;
                case AnimState.Roll:
                    return false;
                default:
                    LogHelper.LogError($"Unknown animation state: {state}");
                    return false;
            }
        }

        #region 유니티 생명주기 함수
        protected virtual void Awake()
        {
            InitAnimation();
            InitSkin();

            OnAwake();
        }

        abstract protected void OnAwake();

        protected virtual void Start()
        {
            OnStart();
        }
        abstract protected void OnStart();

        protected virtual void OnEnable()
        {
            OnEnabled();
        }
        abstract protected void OnEnabled();

        protected virtual void OnDisable()
        {
            OnDisabled();
        }
        abstract protected void OnDisabled();

        protected virtual void Update()
        {
            OnUpdate();
        }

        abstract protected void OnUpdate();

        protected virtual void FixedUpdate()
        {
            OnFixedUpdate();
        }
        abstract protected void OnFixedUpdate();

        protected virtual void LateUpdate()
        {
            OnLateUpdate();
        }
        abstract protected void OnLateUpdate();

        protected virtual void OnDestroy()
        {
            OnDestroyed();
        }
        abstract protected void OnDestroyed();

        #endregion
    }
}