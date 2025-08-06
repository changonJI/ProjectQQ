using UnityEngine;

namespace QQ
{
    public class Stage : BaseGameObject
    {
        public override GameObjectType Type => GameObjectType.Stage;

        private MonsterSpawner monsterSpawner;
        private GridManager grid;

        [SerializeField] short chapter;
        [SerializeField] short stage;

        protected override void OnDestroyed() { }

        protected override void OnFixedUpdate() { }

        protected override void OnFocus() { }

        protected override void OnInit()
        {
            monsterSpawner = gameObject.AddComponent<MonsterSpawner>(this);
            grid = gameObject.GetComponent<GridManager>();

            if (0 == chapter || 0 == stage)
            {
                LogHelper.LogError($"{gameObject.name} 스테이지 프리팹에 스테이지 chapter, stage 설정 안됨");
            }
        }

        protected override void OnLateUpdate() { }

        protected override void OnLostFocus() { }

        protected override void OnStart()
        {
            Pathfinder.Instance.Grid = GetComponent<GridManager>();
        }

        protected override void OnTriggerEnter2Ded(Collider2D other) { }

        protected override void OnUpdate()
        {
        }

        public void SetMonsterSpawner(float camHalfW, float camHalfH)
        {
            // 몬스터 스폰 세팅
            if (null != monsterSpawner)
            {
                monsterSpawner.SetStage(chapter, stage, grid, camHalfW, camHalfH);
            }
        }
    }
}