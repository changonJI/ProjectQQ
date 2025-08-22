using UnityEngine;

namespace QQ
{
    public class SkillSystem : BaseGameObject
    {
        public override GameObjectType Type => GameObjectType.SFX;

        // 초기화 변수
        protected float processTime;
        protected Actor owner;
        protected Monster target;

        // 세팅 변수
        protected Vector3 startPos;
        protected Vector3 targetPos;
        protected Vector3 dir;

        // 테이블 데이터
        protected SkillData data;

        #region 유니티 생명주기 함수
        protected override void OnInit() 
        {
            SetLayer();
            
            owner = PoolManager.Instance.actor;
        }

        protected override void OnStart() 
        {
        }

        protected override void OnFocus()
        {
            target = owner.GetTarget()?.GetComponent<Monster>() ?? null;

            if(target == null)
            {
                PoolManager.Instance.ReleaseObject(gameObject);
                return;
            }

            // 데이터 초기화(풀링 대비)
            SetTable();
            // process 시간 초기화
            processTime = 0f;
            // 위치 초기화
            InitPos();
            // colider On
            col.enabled = true;
            // 움직임 초기화
            rigid.linearVelocity = Vector3.zero;
            // 사운드 실행
            SoundManager.Instance.PlaySFX(data.soundName);
            // TODO: 프리팹 이펙트 재실행
        }

        protected override void OnLostFocus() 
        {
            // 움직임 정지
            rigid.linearVelocity = Vector3.zero;
            // collider Off
            col.enabled = false;

            processTime = 0f;
        }

        protected override void OnUpdate() {}

        protected override void OnFixedUpdate() {}

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
            data = SkillDataManager.Instance.Get(tableID);
        }

        /// <summary>
        /// 오브젝트 위치는 Instantiate에서 설정함
        /// </summary>
        protected void InitPos()
        {
            startPos = spawnPos;
            startPos.y += col.offset.y;//offset 값

            targetPos = target.transform.localPosition;
            targetPos.y += target.GetCollider().offset.y; // target의 offset 값
        }

        protected bool IsFinish()
        {
            // 데이터가 없거나 ID가 0 이하인 경우 시작 안함 처리
            if (data.id <= 0)
                return false;

            float duration = data.duration * 0.001f;
            return processTime >= duration;
        }

        /// <summary>
        /// 바라보는 방향 구하기
        /// </summary>
        protected void SetDir()
        {
            if (target != null)
            {
                dir = (targetPos - startPos).normalized;
            }
        }

        /// <summary>
        /// SetDir 이후 사용할것. 이미지 돌려야 할때 사용
        /// 내적, Cos값을 이용한 각도 구하기
        /// cosTheta = ((x1 * x2) + (y1 * y2)) / (vec3A.magnitude * vec3B.magnitude);
        /// </summary>
        protected void SetAngle()
        {
            // 0도 기준 내적값(내적 == cosTheta 값)
            float cosTheta = Vector3.Dot(Vector3.up, dir);
            // cosTheta값으로 Theta값 구하기. Acos은 radian 값이므로 rad2deg를 곱해준다.
            float theta = Mathf.Acos(cosTheta) * Mathf.Rad2Deg;

            // 시계방향으로 돌리기위해 -값을 곱해준다.
            transform.localRotation = Quaternion.Euler(0, 0, -theta);

            LogHelper.DrawLine(spawnPos, targetPos, Color.red, 1f);
        }

        /// <summary>
        /// skillType Bullet만 목표 방향으로 speed값 만큼 이동
        /// </summary>
        protected void SetMoveLinear()
        {
            rigid.linearVelocity = dir * data.speed;
        }

        /// <summary>
        /// 주체는 Actor, 데미지 적용, 만약 optionType이 explosion이라면 폭발 스킬 재 출력
        /// </summary>
        protected void BulletDamage()
        {
            // 충돌을 했는데 타겟이 존재하지 않는다면 release 처리
            if (target == null)
            {
                PoolManager.Instance.ReleaseObject(gameObject);
                return;
            }

            // 폭발 스킬인경우 투사체가 사라지고 폭발스킬 재 출력
            if(data.mainOptionType == SkillOptionType.Explosion)
            {
                PoolManager.Instance.ReleaseObject(gameObject);

                int optionValue = (int)(data.mainOptionValue * 0.001f);
                SkillManager.Instance.UseSkill(optionValue, transform.localPosition).Forget();
                return;
            }

            // 데미지 적용
            if (target.TryGetComponent<IDamageable>(out var damageable))
            {
                int damage = (int)(GetSkillTypeData(SkillOptionType.Damage) * 0.001f);
                damageable.TakeDamage(damage, transform.position);

                --data.blowCount;

                if(data.blowCount == 0)
                    PoolManager.Instance.ReleaseObject(gameObject);
            }
        }

        // 주체는 Actor, 범위형 데미지
        protected void ExplodeDamage()
        {
            Collider[] hits = Physics.OverlapSphere(
                transform.position,
                data.range * 0.001f,
                LayerMask.GetMask(Layer.Enemy.ToString())
            );

            foreach (var hit in hits)
            {
                IDamageable target = hit.GetComponent<IDamageable>();
                if (target != null)
                {
                    int damage = (int)(GetSkillTypeData(SkillOptionType.Damage) * 0.001f);
                    target.TakeDamage(damage, transform.position);
                }
            }

            // 폭발 후 오브젝트 제거
            PoolManager.Instance.ReleaseObject(gameObject);
        }

        /// <summary>
        /// 주체는 Actor, 스킬 type에 따라 스탯 추가
        /// </summary>
        /// <param name="type"></param>
        public void AddStat(SkillOptionType type)
        {
            owner.TakeStatus(type, GetSkillTypeData(type));
        }

        public int GetSkillTypeData(SkillOptionType type)
        {
            int value = 0;

            switch (type)
            {
                case SkillOptionType.Damage:
                    value = data.mainOptionType == SkillOptionType.Damage ?
                    data.mainOptionValue :
                    0;

                    value += data.subOptionType == SkillOptionType.Damage ?
                        data.subOptionValue :
                        0;
                    break;

                case SkillOptionType.Xp_Pull:
                    value = data.mainOptionType == SkillOptionType.Xp_Pull ?
                        data.mainOptionValue :
                        0;
                    value += data.subOptionType == SkillOptionType.Xp_Pull ?
                        data.subOptionValue :
                        0;
                    break;

                case SkillOptionType.Heal:
                    value = data.mainOptionType == SkillOptionType.Heal ?
                        data.mainOptionValue :
                        0;
                    value += data.subOptionType == SkillOptionType.Heal ?
                        data.subOptionValue :
                        0;
                    break;

                case SkillOptionType.MoveSpdUp:
                case SkillOptionType.MoveSpdDown:
                    value = data.mainOptionType == SkillOptionType.MoveSpdUp ?
                        data.mainOptionValue :
                        0;
                    value += data.subOptionType == SkillOptionType.MoveSpdUp ?
                        data.subOptionValue :
                        0;
                    break;

                case SkillOptionType.Stun:
                    value = data.mainOptionType == SkillOptionType.Stun ?
                        data.mainOptionValue :
                        0;
                    value += data.subOptionType == SkillOptionType.Stun ?
                        data.subOptionValue :
                        0;
                    break;

                case SkillOptionType.Invincible:
                    value = data.mainOptionType == SkillOptionType.Invincible ?
                        data.mainOptionValue :
                        0;
                    value += data.subOptionType == SkillOptionType.Invincible ?
                        data.subOptionValue :
                        0;
                    break;

                case SkillOptionType.None:
                case SkillOptionType.Explosion:
                default:
                    break;
            }

            return value;
        }
    }
}