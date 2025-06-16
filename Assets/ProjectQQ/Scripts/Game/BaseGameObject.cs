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

        #region animation ���� ����
        // ���� �ִϸ��̼� ���°�
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
        /// BaseGameObject ������ �׻� Init() ����
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
        /// Spine �������� Layer ����
        /// </summary>
        /// <param name="anim">���̷��� ������</param>
        /// <param name="animSlot">Body, Weapon Type</param>
        private void SetLayer(SkeletonAnimation anim, AnimSlotType animSlot)
        {
            var mesh = anim.transform.GetComponent<MeshRenderer>();

            // sorting layer ����
            mesh.sortingLayerID = SortingLayer.NameToID(SortingLayerName.Object.ToString());
            // order in layer ����
            mesh.sortingOrder = animSlot == AnimSlotType.Body ? (int)OrderInSortingLayer.OBJBody
                                                              : (int)OrderInSortingLayer.OBJWeapon;
        }

        /// <summary>
        /// Skeleton �� layer ���� �� �ִϸ��̼� �ʱ�ȭ
        /// </summary>
        private void InitAnimation()
        {
            // Body SkeletonAnimation �ʱ�ȭ
            if (animBody != null)
            {
                // �ʱ�ȭ ���ҽ� skeletionanimation.skeleton�� null
                animBody.Initialize(false);

                // sorting layer, order in layer ����
                SetLayer(animBody, AnimSlotType.Body);

                // skeleton Data�� �����Ͽ� animations string�� ĳ��
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

            // Weapon SkeletonAnimation �ʱ�ȭ, Actor�� ����
            if (animWeapon != null && Type == GameObjectType.Actor)
            {
                // �ʱ�ȭ ���ҽ� skeletionanimation.skeleton�� null
                animWeapon.Initialize(false);

                // sorting layer, order in layer ����
                SetLayer(animWeapon, AnimSlotType.Weapon);

                // skeleton Data�� �����Ͽ� animations string�� ĳ��
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

            // �ִϸ��̼� �ʱ�ȭ
            SetCurAnimation(AnimState.Idle);
        }

        /// <summary>
        /// Skeleton�� customSkin ���� �ʱ�ȭ. InitAnimation() ���� ȣ���Ұ�.(Skeleton.skeleton�� null�� �� ����)
        /// </summary>
        private void InitSkin()
        {
            // skin temp ���� ����
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
        /// ���� ��Ų�� �����ϴ� �ڵ�.
        /// </summary>
        /// <param name="skinName"></param>
        public void SetCurSkin(string skinName)
        {
            // ���� ��Ų�� ���Ͽ� �ٸ� ��쿡�� ��Ų�� ����
            if (animBody.skeleton.Skin.Name != skinName)
            {
                // Ŀ���� ��Ų �ʱ�ȭ
                skinBody.Clear();
                // ���� skeleton�� skin �߰�
                skinBody.AddSkin(animBody.skeleton.Data.FindSkin(skinName));

                // skeleton�� ��Ų ����
                animBody.skeleton.SetSkin(skinBody);
                animBody.skeleton.SetSlotsToSetupPose();
                animBody.AnimationState.Apply(animBody.skeleton);
            }
        }

        /// <summary>
        /// Body,Weapon�� �ش� slot���� Attachment�� �����ϴ� �ڵ�
        /// </summary>
        /// <param name="type">AnimSlotType</param>
        /// <param name="slotName">Spine Slot Name</param>
        /// <param name="attachmentName">Spine AtachemntName</param>
        public void SetCurAttachment(AnimSlotType type, string slotName, string attachmentName)
        {
            SetAttachment(type == AnimSlotType.Body ? animBody : animWeapon, slotName, attachmentName);
        }

        /// <summary>
        /// SetCurAttachment()���� ȣ��Ǵ� �ڵ�.
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
        /// SetAnimation�� �����ϴ� �ڵ�
        /// </summary>
        /// <param name="state">���� ���°�</param>
        /// <param name="timeScale">�ð���</param>
        public void SetCurAnimation(AnimState state, float timeScale = 1f)
        {
            if (curAnimState == state)
                return;

            SetAnimation(AnimSlotType.Body, GetAnimName(state), GetAnimLoop(state), timeScale);
            SetAnimation(AnimSlotType.Weapon, GetAnimName(state), GetAnimLoop(state), timeScale);

            curAnimState = state;
        }

        /// <summary>
        /// animation ���� �ڵ�
        /// </summary>
        /// <param name="type">Body, Weapon</param>
        /// <param name="animName">ĳ�̵� animation name��</param>
        /// <param name="loop">�ݺ�üũ</param>
        /// <param name="timeScale">�ð���</param>
        private void SetAnimation(AnimSlotType type, string animName, bool loop, float timeScale = 1)
        {
            if (type == AnimSlotType.Body && animBody != null)
            {
                if (dicAnimBody.TryGetValue(animName, out var anim))
                {
                    // TrackEntry TimeScale ���� �����ϸ�, ���Ӽ����� TimeScale�� ������ ���� �ʰ� ���������� �����Ѵ�.
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
        /// ���� ���º� Spine animation name�� return
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
        /// ���� ���º� Spine animation Loop �� return
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

        #region ����Ƽ �����ֱ� �Լ�
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