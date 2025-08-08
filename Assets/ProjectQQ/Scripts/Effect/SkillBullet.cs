using UnityEngine;

namespace QQ
{
    /// <summary>
    /// Bullet Skill Type
    /// </summary>
    public class SkillBullet : SkillSystem
    {
        protected override void OnFocus()
        {
            base.OnFocus();

            SetAngle();
            SetMoveLinear();
        }

        protected override void OnFixedUpdate()
        {
            if (IsFinish())
            {
                PoolManager.Instance.ReleaseObject(gameObject);
            }
            else
            {
                processTime += Time.fixedDeltaTime;
            }
        }

        protected override void OnTriggerEnter2Ded(Collider2D other)
        {
            // 적 오브젝트와 충돌처리
            if (other.gameObject.layer == (int)Layer.Enemy)
            {
                BulletDamage();
            }
        }
    }
}
