using UnityEngine;

namespace QQ
{
    public class EffectSystem : BaseGameObject
    {
        public override GameObjectType Type => GameObjectType.SFX;

        // 초기화 변수
        protected float processTime;
        protected Actor owner;
        protected Transform target;

        // 세팅 변수
        protected float duration;
        protected Vector3 startPos;
        protected Vector3 endPos;
        protected float speed;
        protected int damage;

        protected Vector3 dir;
        protected bool IsFinished => processTime >= duration;

        #region 유니티 생명주기 함수
        protected override void OnInit() 
        {
            SetLayer();

            owner = PoolManager.Instance.actor;
            target = owner.GetTarget();
        }

        protected override void OnStart() 
        {
            SetTable();
        }

        protected override void OnFocus()
        {
            processTime = 0f;
            InitPos();

            if (target != null)
                dir = (owner.GetTarget().localPosition - startPos).normalized;
        }

        protected override void OnLostFocus() 
        {
            processTime = 0f;
        }

        protected override void OnUpdate() {}

        protected override void OnFixedUpdate()
        {
            // 업데이트 작업
            if (IsFinished)
            {
                PoolManager.Instance.ReleaseObject(gameObject);
            }
            else
            {
                processTime += Time.fixedDeltaTime;
            }
        }

        protected override void OnLateUpdate() { }

        protected override void OnDestroyed() { }

        protected override void OnTriggerEnter2Ded(Collider2D other) {}
        #endregion

        private void SetLayer()
        {
            var mesh = transform.GetComponent<SpriteRenderer>();

            // sorting layer 설정
            mesh.sortingLayerID = SortingLayer.NameToID(SortingLayerName.Effect.ToString());
        }

        private void SetTable()
        {
            //TODO : tableID 변수 사용
            // SkillTableData tableData = SkillTableManager.Instance.Get(tableID);

            duration = 5f; // roll animTIme
            speed = 10f;
            damage = 10;
        }

        public void InitPos()
        {
            startPos = owner.transform.localPosition;
            startPos.y += 9f;//offset 값
            transform.localPosition = startPos;
        }
    }
}