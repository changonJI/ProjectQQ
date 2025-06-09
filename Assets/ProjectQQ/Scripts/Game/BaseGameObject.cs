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
            mesh.sortingLayerID = SortingLayer.NameToID(SortingLayerName.Object.ToString());

            mesh.sortingOrder = animSlot == AnimSlotType.Body ? (int)OrderInSortingLayer.OBJBody
                                                              : (int)OrderInSortingLayer.OBJWeapon;
        }

        /// <summary>
        /// Skeleton의 customSkin 변수 초기화
        /// </summary>
        private void InitSkin()
        {
            skinBody = new Skin("BodySkin");
            skinWeapon = new Skin("WeaponSkin");
        }

        /// <summary>
        /// Skeleton 별 layer 세팅 및 애니메이션 초기화
        /// </summary>
        private void InitAnimation()
        {
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

            if (animWeapon != null)
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

            SetCurAnimation(AnimState.Idle);
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
            InitSkin();
            InitAnimation();

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